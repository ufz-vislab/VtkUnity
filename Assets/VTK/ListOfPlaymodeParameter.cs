using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ListOfPlaymodeParameter
{
	public List<PlaymodeParameter> elements;

	public ListOfPlaymodeParameter()
	{
		this.elements = new List<PlaymodeParameter> ();
	}

	public int Count()
	{
		return elements.Count;
	}

	public bool ContainsName (string name)
	{
		foreach(PlaymodeParameter e in elements)
		{
			if(e.name == name)
			{
				return true;
			}
		}

		return false;
	}

	public void Add(PlaymodeParameter pp)
	{
		elements.Add (pp);
	}

	public PlaymodeParameter Get(int index)
	{
		if(index < elements.Count)
		{
			return elements[index];
		}

		return null;
	}
}
