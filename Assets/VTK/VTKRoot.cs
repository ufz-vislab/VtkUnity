using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Kitware.VTK;

/*
 * Handle vtk-filters for a given vtk-file
 * */
[ExecuteInEditMode]
public class VTKRoot : MonoBehaviour
{
	//Editor stuff
	[HideInInspector]
	public bool selectedFileIsValid = false;

	//File stuff
	[HideInInspector]
	public string filepath;

	//Reader stuff
	[HideInInspector]
	public vtkXMLPolyDataReader polyDataReader;
	[HideInInspector]
	public vtkXMLUnstructuredGridReader unstructuredGridReader;

	[HideInInspector]
	public Vector3 startPosition;

	//Node stuff
	[HideInInspector]
	public string[] supportedFilters;
	[HideInInspector]
	public int selectedFilter;
	[HideInInspector]
	public VTKNode rootNode = new VTKNode(null, null, null);
	[HideInInspector]
	public VTK.DataType dataType;
	[HideInInspector]
	public VTKNode activeNode;
	[HideInInspector]
	public DictionaryVtkToUnity gameObjects = new DictionaryVtkToUnity ();

	public void OnEnable()
	{
		if (selectedFileIsValid)
		{
			Preload();
		}
	}

	public void Initialize()
	{
		string rootName = VTK.GetFileName (filepath);
		gameObject.name =  rootName;

		supportedFilters = VTK.GetSupportedFiltersByName ();

		//Initialize file reader
		if(filepath.EndsWith(".vtp"))
		{
			dataType = VTK.DataType.PolyData;

			polyDataReader = vtkXMLPolyDataReader.New ();
			polyDataReader.SetFileName(filepath);
			polyDataReader.Update();
		}

		if(filepath.EndsWith(".vtu"))
		{
			dataType = VTK.DataType.UnstructuredGrid;

			unstructuredGridReader = vtkXMLUnstructuredGridReader.New();
			unstructuredGridReader.SetFileName(filepath);
			unstructuredGridReader.Update();
		}

		//Initialize root node
		Debug.Log("Initialize root node");
		rootNode.name = rootName;
		rootNode.filter = gameObject.AddComponent<VTKFilterRootNode> ();
		rootNode.properties = gameObject.AddComponent<VTKProperties> ();
	
		PreloadNode(rootNode);

		activeNode = rootNode;
	}

	public void Preload()
	{
		//Preload all other nodes
		PreloadNode (rootNode);
	}

	public void PreloadNode(VTKNode node)
	{
		Debug.Log ("Preload data for " + node.name);

		string objectName = VTK.GetGameObjectName(node);

		//Set filter
		if(node.isRoot)
		{
			if(dataType == VTK.DataType.PolyData)
			{
				polyDataReader = vtkXMLPolyDataReader.New ();
				polyDataReader.SetFileName(filepath);
				polyDataReader.Update();

				node.filter.vtkFilter = polyDataReader;
				node.filter.outputType = VTK.DataType.PolyData;
			}

			if(dataType == VTK.DataType.UnstructuredGrid)
			{
				unstructuredGridReader = vtkXMLUnstructuredGridReader.New();
				unstructuredGridReader.SetFileName(filepath);
				unstructuredGridReader.Update();

				node.filter.vtkFilter = unstructuredGridReader;
				node.filter.outputType = VTK.DataType.UnstructuredGrid;
			}
		}
		else
		{
			node.filter.node = node;
			node.filter.UpdateInput();
			node.filter.SetPlaymodeParameters();
		}

		//Set properties
		node.properties.node = node;
		node.properties.SetPlaymodeParameters();
		node.properties.Read ();

		//Set vtkToUnity
		VtkToUnity vtu;

		if(gameObjects.TryGetValue (objectName, out vtu))
		{
			gameObjects.Set(objectName, new VtkToUnity(node.filter.vtkFilter.GetOutputPort(), 
				FindGameObject(objectName)));

			node.UpdateProperties(); //Some filters need stuff from properties
		}

		//Set controller script
		ControllerGameObject cg = node.filter.gameObject.GetComponent<ControllerGameObject>();
		if(cg != null)
		{
			cg.node = node;
			cg.Initialize();
		}

		//Do it for the kids
		if(node.hasChildren)
		{
			foreach (VTKNode child in node.children)
			{
				//Set parent reference
				child.parent = node;

				PreloadNode(child);
			}
		}
	}

	/*
	 * Creates node new node
	 * Creates or update gameobject
	 * */
	public void AddNode(string filterName)
	{
		VTKNode node = activeNode.AddChild (new VTKNode ((VTKFilter)gameObject.AddComponent (filterName), 
			activeNode, gameObject.AddComponent<VTKProperties>()));

		if (node == null)
			return;

		Debug.Log ("Add node " + node.name);

		node.filter.node = node;
		node.filter.SetPlaymodeParameters ();
		node.properties.node = node;
		node.properties.SetPlaymodeParameters ();
		node.properties.Read ();
		node.UpdateFilter();
		
		SetActiveNode (node);
		
		//Create or update gameobject for new node
		if(!node.parent.isRoot)
		{
			//If parent has no children, update parent
			if(node.parent.children.Count == 1)
			{
				//Destroy old gameobject
				gameObjects.Remove(VTK.GetGameObjectName(node.parent));
				GameObject.DestroyImmediate(GameObject.Find(VTK.GetGameObjectName(node.parent)));
			}
		}
		
		CreateGameObject (node);
	}

	/*
	 * Removes node (drop-down)
	 * */
	public void RemoveNode(VTKNode node)
	{
		if (node.isRoot)
			return;

		VTKNode parent = node.parent;

		SetActiveNode (parent);

		//Delete all gameobjects node is part of
		for(int i = gameObject.transform.childCount - 1; i > -1; i--)
		{
			GameObject go = gameObject.transform.GetChild(i).gameObject;

			if(go.name.Contains(node.name))
			{
				gameObjects.Remove(go.name);
				DestroyImmediate(go);
			}
		}

		//Delete node
		parent.RemoveChild (node);

		//Update parent node
		
		//If no children left, show parent
		if(!parent.hasChildren && !parent.isRoot)
		{
			if(parent.filter.outputType == VTK.DataType.PolyData)
			{
				CreateGameObject(parent);
			}
		}
	}

	/*
	 * Sets the given node active
	 * Hides scripts from the current node
	 * Shows scripts for the new node
	 * */
	public void SetActiveNode(VTKNode node)
	{
		if(activeNode == null)
			activeNode = rootNode;

		//Hide scripts old node
		if (!activeNode.isRoot)
		{
			activeNode.filter.hideFlags = HideFlags.HideInInspector;
		}

		activeNode.properties.hideFlags = HideFlags.HideInInspector;

		//Set active node
		activeNode = node;

		//Show scripts new node
		if (!activeNode.isRoot)
		{
			activeNode.filter.hideFlags = HideFlags.None;
		}

		if(!activeNode.hasChildren) //Show properties
		{
			activeNode.properties.hideFlags = HideFlags.None;
		}
	}

	public void CreateGameObject(VTKNode node)
	{
		string gameObjectName = VTK.GetGameObjectName(node);
		Debug.Log ("Create gameobject " + gameObjectName);
		
		if(node.filter.outputType == VTK.DataType.PolyData)
		{
			//Create gameobject
			VtkToUnity vtu = new VtkToUnity(node.filter.vtkFilter.GetOutputPort(), 
				VTK.GetGameObjectName (node));
			vtu.go.transform.parent = gameObject.transform;
			vtu.ColorBy (Color.magenta);
			vtu.Update ();

			gameObjects.Add(vtu.go.name, vtu);

			if(node.filter.outputType == VTK.DataType.PolyData)
			{
				//Add mesh for controller support
				GameObject go = FindGameObject(gameObjectName);
				MeshCollider mc = go.AddComponent<MeshCollider>();
				mc.isTrigger = true;
				ControllerGameObject cg = go.AddComponent<ControllerGameObject>();
				cg.node = node;
				cg.Initialize();
			}
		}
	}

	[ContextMenu("Reset filters")]
	void ResetFilters()
	{
		if(rootNode.hasChildren)
		{
			ResetFilter(rootNode);

			foreach(VTKNode child in rootNode.children)
			{
				child.UpdateFilter();
			}
		}
	}

	private void ResetFilter(VTKNode node)
	{
		node.filter.Reset();

		if(node.hasChildren)
		{
			foreach(VTKNode child in node.children)
			{
				ResetFilter(child);
			}
		}
	}

	public GameObject FindGameObject(string name)
	{
		if (name == VTK.GetGameObjectName (rootNode))
						return gameObject;

		Transform found = gameObject.transform.Find (name);

		if(found != null)
		{
			return found.gameObject;
		}
		else
		{
			return null;
		}
	}
}
