using UnityEngine;
using System.Collections.Generic;
using Kitware.VTK;

/*
 * Handle vtk-filters for a given vtk-file
 * */

[ExecuteInEditMode]
public class VTKRoot : MonoBehaviour
{
	//File stuff
	[HideInInspector]
	public string filepath;
	[HideInInspector]
	public string filename;

	//Reader stuff
	[HideInInspector]
	public vtkXMLPolyDataReader polyDataReader;
	[HideInInspector]
	public vtkXMLUnstructuredGridReader unstructuredGridReader;

	[HideInInspector]
	public Vector3 startPosition;

	//Filter stuff
	[HideInInspector]
	public string[] supportedFilters;
	[HideInInspector]
	public int selectedFilter;
	[HideInInspector]
	public VTKNode root = new VTKNode(null, null, null);
	[HideInInspector]
	public VTK.ReaderType readerType;
	[HideInInspector]
	public VTKNode activeNode;
	[HideInInspector]
	public DictionaryVtkToUnity unityObjects = new DictionaryVtkToUnity ();

	//Editor stuff
	[HideInInspector]
	public bool selectedFileIsValid = false;

	public void OnEnable()
	{
		if (selectedFileIsValid)
		{
			Initialize ();
		}
	}

	public void Initialize()
	{
		if(filename == null)
			filename = GetFileName (filepath);

		if(gameObject.name != filename)
			gameObject.name = filename;

		//Initialize file reader
		if(readerType == VTK.ReaderType.PolyData)
		{
			polyDataReader = vtkXMLPolyDataReader.New ();
			polyDataReader.SetFileName(filepath);
			polyDataReader.Update();
		}

		if(readerType == VTK.ReaderType.UnstructuredGrid)
		{
			unstructuredGridReader = vtkXMLUnstructuredGridReader.New();
			unstructuredGridReader.SetFileName(filepath);
			unstructuredGridReader.Update();
		}

		supportedFilters = VTK.GetSupportedFiltersByName ();

		bool initialUse = false;

		//If it is a new object, create a vtkToUnity for root node
		if (unityObjects.Count == 0)
		{
			Debug.Log("Create root node");

			root.name = filename;

			root.filter = gameObject.AddComponent<VTKFilterRootNode> ();

			if(readerType == VTK.ReaderType.PolyData)
			{
				root.filter.vtkFilter = polyDataReader;
			}

			if(readerType == VTK.ReaderType.UnstructuredGrid)
			{
				root.filter.vtkFilter = unstructuredGridReader;
			}

			root.properties = gameObject.AddComponent<VTKProperties> ();
			root.properties.node = root;
			root.properties.SetPlaymodeParameters();
			root.properties.Read();

			//Show root
			if(readerType == VTK.ReaderType.PolyData)
			{
				VtkToUnity vtkToUnity = new VtkToUnity(root.filter.vtkFilter.GetOutputPort(), gameObject);
				vtkToUnity.ColorBy (Color.magenta);
				vtkToUnity.Update ();

				unityObjects.Add(root.name, vtkToUnity);
			}

			activeNode = root;

			initialUse = true;
		}

		//Preload everthing if the szene is restarted
		if(!initialUse)
		{
			//TODO workaround, da parentreferenzen in den nodes verloren gehen, childreferenzen aber nicht oO?
			Debug.Log ("Set parent references...");
			SetParentReferences (root);
			Debug.Log ("Done");

			Debug.Log("Initialize all nodes, filters and objects...");
			PreloadData (root);
			Debug.Log ("Done");

			//Hide root object
			if(root.hasChildren)
			{
				if (gameObject.GetComponent<MeshFilter>())
					gameObject.GetComponent<MeshFilter>().mesh = null;
			}
		}

		SetActiveNode (root);
	}

	/*
	 * Create node for new filter
	 * Compute new filter
	 * Hide root unity object
	 * */
	public void AddFilter(string filterName)
	{
		VTKNode newNode = activeNode.AddChild (new VTKNode ((VTKFilter)gameObject.AddComponent (filterName), activeNode, gameObject.AddComponent<VTKProperties>()));

		if (newNode == null)
						return;
		
		newNode.filter.node = newNode;
		newNode.filter.SetPlaymodeParameters ();

		newNode.properties.node = newNode;
		newNode.properties.SetPlaymodeParameters ();
		newNode.properties.Read ();
	
		//Hide editor
		newNode.filter.hideFlags = HideFlags.HideInInspector;
		
		SetActiveNode (newNode);
		
		Modifie (activeNode);

		//Clear root object
		if(root.hasChildren)
		{
			if(gameObject.renderer)
				gameObject.renderer.enabled = false;
		}
	}

	/*
	 * Remove node
	 * Remove drop-down nodes
	 * */
	public void RemoveNode(VTKNode node)
	{
		if (node.isRoot)
						return;

		Debug.LogWarning("Remove: " + node.name);
		VTKNode parent = node.parent;

		//Delete all unity objects node is part of
		for(int i = gameObject.transform.childCount - 1; i > -1; i--)
		{
			GameObject go = gameObject.transform.GetChild(i).gameObject;

			if(go.name.Contains(node.name))
			{
				unityObjects.Remove(go.name);
				DestroyImmediate(go);
			}
		}

		//Delete node
		root.RemoveChild (node);

		//Update parent node
		if (parent.isRoot)
		{
			if(!parent.hasChildren)
			{
				gameObject.renderer.enabled = true;
				unityObjects.Set(GetUnityObjectName(root), new VtkToUnity(root.filter.vtkFilter.GetOutputPort(), gameObject));
				UpdateProperties(root);
			}
		}
		else
		{
			if(!parent.hasChildren) //Update filter and vtkToUnity
			{
				Modifie(parent);
			}
		}

		activeNode = parent;
	}

	public void SetActiveNode(VTKNode node)
	{
		if(activeNode == null)
			activeNode = root;

		//Hide stuff from old node
		if (!activeNode.isRoot)
		{
			activeNode.filter.hideFlags = HideFlags.HideInInspector;
		}

		activeNode.properties.hideFlags = HideFlags.HideInInspector;

		//Set active node
		activeNode = node;

		//Show stuff for active node
		if (!activeNode.isRoot) //Show filter
		{
			activeNode.filter.hideFlags = HideFlags.None;
		}

		if(!activeNode.hasChildren) //Show properties
		{
			activeNode.properties.hideFlags = HideFlags.None;
		}
	}

	//TODO refactoring!!!
	/*
	 * Update or extend existing unity object or create new one
	 * */
	public void UpdateUnityObject(VTKNode node)
	{
		string unityObjectName = GetUnityObjectName (node);

		Debug.Log ("UpdateUnityObject: " + unityObjectName);

		VtkToUnity vtkToUnity;

		if (unityObjects.TryGetValue(unityObjectName, out vtkToUnity)) //Update existing unity object
		{
			vtkToUnity.triangleFilter.SetInputConnection(node.filter.vtkFilter.GetOutputPort());
		}
		else //New filter
		{
			/*
			 * New vtkToUnity and new unity gameobject if:
			 * 1: Parent node already has children
			 * 2: Parent node is root
			 *
			 * Extend exsisting vtkToUnity and unity gameobject if:
			 * neighter 1 nor 2
			 * */

			VTKNode parent = node.parent;
			string parentName = GetUnityObjectName(parent);

			if(parent.isRoot || parent.children.Count > 1) //New
			{
				Debug.Log("Create new outputobject " + unityObjectName);

				//Create new gameobject
				GameObject go =  new GameObject(unityObjectName);

				go.transform.parent = gameObject.transform;

				//Create new vtkToUnity and apply filter
				vtkToUnity = new VtkToUnity(node.filter.vtkFilter.GetOutputPort(), go);

				//Add it to the list
				unityObjects.Add(go.name, vtkToUnity);
			}
			else //Extend / reduce
			{
				//Get vtkToUnity
				if(unityObjects.TryGetValue(parentName, out vtkToUnity))
				{
					Debug.Log("Extend " + parentName + " to " + unityObjectName);

					//Get unity object to extend
					GameObject go = FindUnityObject(parentName);

					//Rename unity object
					go.name = unityObjectName;

					//Remove old dictionary entry
					unityObjects.Remove(parentName);

					//Add new dictionary entry
					unityObjects.Add(unityObjectName, vtkToUnity);
				}
				else
				{
					Debug.Log("Reduce ??? to " + unityObjectName);

					//Create new gameobject
					GameObject go =  new GameObject(unityObjectName);

					//Put new gameobject as child
					go.transform.parent = gameObject.transform;

					//Create new vtkToUnity and apply filter
					vtkToUnity = new VtkToUnity(node.filter.vtkFilter.GetOutputPort(), go);

					//Add it to the list
					unityObjects.Add(go.name, vtkToUnity);
				}

			}
		}

		UpdateProperties (node);
	}

	public void UpdateProperties(VTKNode node)
	{
		string unityObjectName = GetUnityObjectName (node);

		Debug.Log ("Update properties: " + unityObjectName);

		//Handle vtkToUnity
		VtkToUnity vtkToUnity;

		unityObjects.TryGetValue (unityObjectName, out vtkToUnity);

		vtkToUnity.triangleFilter.SetInputConnection(node.filter.vtkFilter.GetOutputPort());

		//Handle properties
		VTKProperties properties = node.properties;

		if (properties.selectedColorType == 0) //solid color
		{
			vtkToUnity.ColorBy (Color.magenta);
		}
		else
		{
			if (properties.selectedColorType == 1) //data
			{
				string data = properties.dataArrays[properties.selectedDataArray];
				string dataName = data.Remove(data.IndexOf("[") - 1);

				if(data.EndsWith("[C]"))
				{
					vtkToUnity.ColorBy (dataName, VtkToUnity.VtkColorType.CELL_DATA);
				}

				if(data.EndsWith("[P]"))
				{
					vtkToUnity.ColorBy (dataName, VtkToUnity.VtkColorType.POINT_DATA);
				}
			}

			vtkToUnity.SetLut ((VtkToUnity.LutPreset) properties.selectedLut);
		}

		vtkToUnity.Update ();
		//TODO dont use a fixed position
		vtkToUnity.go.transform.Translate(0f, 0f, 0f);
	}

	/*
	 * Propagate changes from the given node drop-down its children
	 * */
	public void Modifie(VTKNode node)
	{
		if (node.isRoot)
						return;

		Debug.Log ("Modifie: " + node.name);
		
		node.filter.UpdateFilter (node.parent.filter.vtkFilter);

		if(node.hasChildren)
		{
			foreach (VTKNode child in node.children)
			{
				Modifie(child);
			}
		}
		else
		{
			UpdateUnityObject(node);
		}
	}

	/*
	 * Compute all filters
	 * Create vtkToUnity objects for every unity object
	 * */
	public void PreloadData(VTKNode node)
	{
		Debug.Log ("Preload data for " + node.name);

		string unityObjectName = GetUnityObjectName(node);

		//Calculate filter
		if (node.isRoot)
		{
			if(readerType == VTK.ReaderType.PolyData)
			{
				node.filter.vtkFilter = polyDataReader;
			}

			if(readerType == VTK.ReaderType.UnstructuredGrid)
			{
				node.filter.vtkFilter = unstructuredGridReader;
			}
		}
		else
		{
			node.filter.SetPlaymodeParameters();
			node.filter.node = node;
			node.filter.UpdateFilter (node.parent.filter.vtkFilter);
		}

		//Read properties
		node.properties.node = node;
		node.properties.SetPlaymodeParameters();
		node.properties.Read ();

		//Set vtkToUnity
		VtkToUnity vtkToUnity;

		if(unityObjects.TryGetValue (unityObjectName, out vtkToUnity))
		{
			unityObjects.Set(unityObjectName, new VtkToUnity(node.filter.vtkFilter.GetOutputPort(), FindUnityObject(unityObjectName)));
			UpdateProperties(node);
		}

		//Do it for the kids
		foreach (VTKNode child in node.children)
		{
			PreloadData(child);
		}
	}

	/*
	 * Get VTK-file name
	 * */
	public string GetFileName(string filepath)
	{
		return filepath.Remove(0, filepath.LastIndexOf("/")+1);
	}

	public void SetParentReferences(VTKNode n)
	{
		if (n.hasChildren)
		{
			foreach(VTKNode child in n.children)
			{
				child.parent = n;

				SetParentReferences(child);
			}
		}
	}

	public string GetUnityObjectName(VTKNode node)
	{
		string name = "";

		VTKNode current = node;

		while (current.parent != null)
		{
			name = "," + current.name + name;
			current = current.parent;
		}

		name = current.name + name;

		return name;
	}

	public GameObject FindUnityObject(string name)
	{
		if (name == GetUnityObjectName (root))
						return gameObject;

		return gameObject.transform.Find (name).gameObject;
	}
}
