using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class VTKNode 
{
	public string name;
	public VTKFilter filter;
	public VTKNode parent;
	public List<VTKNode> children;
	public bool isRoot;
	public VTKProperties properties;
	public bool hasChildren
	{
		get{
				if(children != null)
				{
					if(children.Count > 0)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
	}

	public VTKNode(VTKFilter filter, VTKNode parent, VTKProperties properties)
	{
		this.filter = filter;
		this.parent = parent;
		this.children = new List<VTKNode> ();
		this.isRoot = (parent == null) ? true : false;
		this.name = (isRoot) ? name : filter.GetType ().ToString ().Remove (0, 9) + GetFilterNumber();
		this.properties = properties;
	}

	public VTKNode AddChild(VTKNode node)
	{
		/*
		if (!VTK.ApplicableFilters (this, node))
		{
			Object.DestroyImmediate(node.filter);
			Object.DestroyImmediate(node.properties);

			return null;
		}*/

		if (children == null) 
		{
			children = new List<VTKNode>();
		}

		this.children.Add (node);

		return node;
	}

	/*
	 * Also removes drop-down filters
	 * */
	public void RemoveChild(VTKNode node)
	{
		if (node.isRoot)
			return;

		VTKNode parent = node.parent;

		//Drop-down filters
		if(node.hasChildren)
		{
			for(int i = 0; i < node.children.Count; i++)
			{
				RemoveChild(node.children[i]);
			}
		}

		//Remove filter script from editor
		Object.DestroyImmediate(node.filter);

		//Remove properties script from editor
		Object.DestroyImmediate(node.properties);

		//Remove node
		parent.children.Remove (node);
	}

	public void UpdateFilter()
	{
		if(isRoot)
			return;

		//Update this filter
		filter.UpdateInput();

		//UpdateChildren
		if(hasChildren)
		{
			foreach (VTKNode child in children)
			{
				child.UpdateFilter();
			}
		}
		
		//Update gameobject
		VtkToUnity vtu;
		VTKRoot root = filter.gameObject.GetComponent<VTKRoot>();
		string gameObjectName = VTK.GetGameObjectName (this);

		if(root.gameObjects.TryGetValue (gameObjectName, out vtu))
		{
			/*
			vtu.triangleFilter.SetInputConnection(filter.vtkFilter.GetOutputPort());
			vtu.Update();
			*/
			UpdateProperties();
		}
	}

	public void UpdateProperties()
	{
		Debug.Log ("Update properties: " + VTK.GetGameObjectName (this));
		
		VtkToUnity vtkToUnity;
		VTKRoot root = filter.gameObject.GetComponent<VTKRoot>();

		root.gameObjects.TryGetValue (VTK.GetGameObjectName (this), out vtkToUnity);
		
		vtkToUnity.triangleFilter.SetInputConnection(filter.vtkFilter.GetOutputPort());

		//Properties
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

	//TODO brauch ich das überhaupt noch?
	public VTKNode GetNode(VTKNode toFind)
	{
		//If there are no children get out
		if (!this.hasChildren)
			return null;
		
		//If toFind is child of this object return it
		if (this.children.Contains (toFind)) 
		{
			return this.children[children.IndexOf(toFind)];
		}
		
		//Recursivly search the children
		VTKNode found = null;
		
		for (int i = 0; i < this.children.Count && found == null; i++) 
		{
			found = this.children[i].GetNode(toFind);
		}
		
		return found;
	}

	//TODO brauch ich das noch?
	public VTKNode GetNode(string name)
	{
		if (this.name == name)
						return this;

		if (!this.hasChildren)
						return null;

		VTKNode found = null;

		if(this.hasChildren)
		{
			for(int i = 0; i < this.children.Count; i++)
			{
				found = children[i].GetNode(name);
			}
		}

		return found;
	}

	//TODO beim löschen von kindern wird die nummer in bestimmten fällen nicht richtig gesetzt
	public int GetFilterNumber()
	{
		int number = filter.gameObject.GetComponents (filter.GetType ()).Length;
		
		return number;
	}
}
