using UnityEngine;
using System.Collections.Generic;

/*
 * Node that contains a filter, a parent reference and a list of childfilters
 * */

[System.Serializable]
public class VTKNode 
{
	public string name;
	public VTKFilter filter;
	public VTKNode parent;
	public List<VTKNode> children;
	public bool hasChildren
	{
		get{return (children == null || children.Count == 0) ? false : true;}
	}
	public bool isRoot;
	public VTKProperties properties;

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
	 * Only call on root
	 * */
	public void RemoveChild(VTKNode node)
	{
		if (node.isRoot)
			return;

		Debug.LogWarning ("remove in node: " + node.name);
		//Get node
		VTKNode toRemove = GetChild (node);

		//Get parent
		VTKNode parent = toRemove.parent;

		//Drop-down filters
		if(toRemove.hasChildren)
		{
			for(int i = 0; i < toRemove.children.Count; i++)
			{
				RemoveChild(toRemove.children[i]);
			}
		}

		//Remove filter script from editor
		Object.DestroyImmediate(toRemove.filter);

		//Remove properties script from editor
		Object.DestroyImmediate(toRemove.properties);

		//Remove node
		parent.children.Remove (toRemove);
	}

	/* 
	 * Search the entire tree
	 * */
	public VTKNode GetChild(VTKNode toFind)
	{
		//If there are no children get out
		if (!hasChildren)
			return null;
		
		//If toFind is child of this object return it
		if (this.children.Contains (toFind)) 
		{
			return children[children.IndexOf(toFind)];
		}
		
		//Recursivly search the children
		VTKNode found = null;
		
		for (int i = 0; i < children.Count && found == null; i++) 
		{
			found = children[i].GetChild(toFind);
		}
		
		return found;
	}

	/*
	 * Count amount of filters of the given type on gameobject + 1
	 * */
	public int GetFilterNumber()
	{
		int number = filter.gameObject.GetComponents (filter.GetType ()).Length;
		
		return number;
	}
}
