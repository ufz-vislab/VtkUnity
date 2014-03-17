using UnityEngine;
using System.Collections.Generic;

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

	[HideInInspector]
	public Vector3 startPosition;

	[HideInInspector]
	public Kitware.VTK.vtkXMLPolyDataReader polyDataReader;

	[HideInInspector]
	public Kitware.VTK.vtkXMLUnstructuredGridReader unstructuredGridReader; 

	//Filter stuff
	[HideInInspector]
	public string[] allFilters;
	[HideInInspector]
	public int selectedFilter;
	[HideInInspector]
	public VTKNode root = new VTKNode(null, null, null); //Empty node as root
	[HideInInspector]
	public VTK.FilterType readerType;
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
		filename = GetFileName (filepath);

		if(gameObject.name != filename)
			gameObject.name = filename;

		//Initialize vtk-file-reader
		if(readerType == VTK.FilterType.PolyData)
		{
			polyDataReader = Kitware.VTK.vtkXMLPolyDataReader.New ();
			polyDataReader.SetFileName(filepath);
			polyDataReader.Update();
		}

		if(readerType == VTK.FilterType.UnstructuredGrid)
		{
			unstructuredGridReader = Kitware.VTK.vtkXMLUnstructuredGridReader.New();
			unstructuredGridReader.SetFileName(filepath);
			unstructuredGridReader.Update();
		}

		allFilters = VTK.GetFiltersByName ();

		bool initialUse = false;

		//If it is a new object, create a vtkToUnity for root node
		if (unityObjects.Count == 0) 
		{
			Debug.Log("Creating entry for root node");

			root.name = filename;

			//Set filter
			root.filter = gameObject.AddComponent<VTKFilterEmpty> ();
			root.filter.OutputType = readerType;
		
			if(readerType == VTK.FilterType.PolyData)
				root.filter.UpdateFilter(polyDataReader.GetOutputPort());

			if(readerType == VTK.FilterType.UnstructuredGrid)
				root.filter.UpdateFilter(unstructuredGridReader.GetOutputPort());

			//Set properties for root
			root.properties = gameObject.AddComponent<VTKProperties> ();
			root.properties.Read();

			//Show root
			if(readerType == VTK.FilterType.PolyData)
			{
				VtkToUnity vtkToUnity = new VtkToUnity(root.filter.output, gameObject);
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
			InitializeAll (root);
			Debug.Log ("Done");

			//Hide root object
			if(root.hasChildren)
			{
				gameObject.GetComponent<MeshFilter>().mesh = null;
			}
		}

		SetActiveNode (root); //Clear old scripts
	}

	/*
	 * Create node for new filter
	 * Compute new filter based on parent output
	 * Hide root unity object
	 * */
	public void AddFilter(string filterName)
	{
		VTKNode newNode = activeNode.AddChild (new VTKNode ((VTKFilter)gameObject.AddComponent (filterName), activeNode, gameObject.AddComponent<VTKProperties>()));

		if (newNode == null)
						return;

		//Handle properties
		if(newNode.filter.OutputType == VTK.FilterType.UnstructuredGrid)
		{
			newNode.properties = null;
		}
		else
		{
			newNode.properties.Read ();
		}

		//Hide filter editor
		newNode.filter.hideFlags = HideFlags.HideInInspector;

		//Set new node as active
		SetActiveNode (newNode);

		//Apply new filter settings
		Modifie (activeNode);

		//Clear root object
		if(root.hasChildren)
		{
			//gameObject.GetComponent<MeshFilter>().renderer.enabled = false;
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
				unityObjects.Set(GetUnityObjectName(root), new VtkToUnity(root.filter.output, gameObject));
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

	/*
	 * Update or extend existing unity object or create new one
	 * */
	public void UpdateUnityObject(VTKNode node)
	{
		string name = GetUnityObjectName (node);

		Debug.Log ("UpdateUnityObject: " + name);

		VtkToUnity vtkToUnity;

		if (unityObjects.TryGetValue(name, out vtkToUnity)) //Update existing unity object
		{
			vtkToUnity.triangleFilter.SetInputConnection(node.filter.output);
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
				Debug.Log("Create new outputobject " + name);
				//Create new gameobject
				GameObject go =  new GameObject(name);

				//Put new gameobject as child
				go.transform.parent = gameObject.transform;

				//Create new vtkToUnity and apply filter
				vtkToUnity = new VtkToUnity(node.filter.output, go);

				//Add it to the list
				unityObjects.Add(go.name, vtkToUnity);
			} 
			else //Extending
			{
				Debug.Log("Extending: " + parentName + " to " + name);

				//Get unity object to extend
				GameObject go = FindUnityObject(parentName);

				//Get vtkToUnity
				unityObjects.TryGetValue(parentName, out vtkToUnity);

				//Rename unity object
				go.name = name;

				//Remove old dictionary entry
				unityObjects.Remove(parentName);

				//Add new dictionary entry
				unityObjects.Add(name, vtkToUnity);
			}
		}

		UpdateProperties (node);
	}

	public void UpdateProperties(VTKNode node)
	{
		string name = GetUnityObjectName (node);

		Debug.Log ("Update properties: " + name);

		VtkToUnity vtkToUnity; 

		unityObjects.TryGetValue (name, out vtkToUnity);

		vtkToUnity.triangleFilter.SetInputConnection(node.filter.output);

		//Set properties
		VTKProperties properties = node.properties;
		
		if (properties.selectedColorType == 0) //solid color
		{ 
			vtkToUnity.ColorBy (Color.magenta);
		} 
		else 
		{
			if (properties.selectedColorType == 1) //data
			{
				string data = properties.dataArray[properties.selectedDataArray];
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

		//Calculate filter output based on parent filter
		node.filter.UpdateFilter (node.parent.filter.output);

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
	 * Create vtkToUnity objects for every unity gameobject
	 * */
	public void InitializeAll(VTKNode node)
	{
		string name = GetUnityObjectName(node);

		//Calculate filter for the given node
		if (node.isRoot) 
		{
			node.filter.OutputType = readerType;

			if(readerType == VTK.FilterType.PolyData)
				node.filter.UpdateFilter(polyDataReader.GetOutputPort());

			if(readerType == VTK.FilterType.UnstructuredGrid)
				node.filter.UpdateFilter(unstructuredGridReader.GetOutputPort());
		}
		else
		{
			node.filter.UpdateFilter (node.parent.filter.output);
		}

		//Read properties
		node.properties.Read ();

		//Set vtkToUnity for the given node
		VtkToUnity vtkToUnity;
		
		if(unityObjects.TryGetValue (name, out vtkToUnity))
		{
			unityObjects.Set(name, new VtkToUnity(node.filter.output, FindUnityObject(name)));
			UpdateProperties(node);
		}

		//Do it for the kids
		foreach (VTKNode child in node.children) 
		{
			InitializeAll(child);
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
