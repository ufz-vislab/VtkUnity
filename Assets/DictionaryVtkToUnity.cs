using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DictionaryVtkToUnity 
{
	[SerializeField]
	private List<string> keys;
	
	[SerializeField]
	private List<VtkToUnity> values;
	
	public int Count
	{
		get{return keys.Count;}
	}
	
	public DictionaryVtkToUnity()
	{
		this.keys = new List<string> ();
		this.values = new List<VtkToUnity> ();
	}

	public void Set(string key, VtkToUnity value)
	{
		if(ContainsKey(key))
		{
			int index = keys.IndexOf(key);

			values[index] = value;
		}
	}
	public void Add(string key, VtkToUnity value)
	{
		keys.Add (key);
		values.Add (value);
	}
	
	public void Remove(string key)
	{
		if (ContainsKey (key)) 
		{
			int index = keys.IndexOf(key);
			
			keys.RemoveAt(index);
			values.RemoveAt(index);
		}
	}
	
	public bool TryGetValue(string key, out VtkToUnity vtkToUnity)
	{
		if (ContainsKey (key))
		{
			vtkToUnity = values [keys.IndexOf (key)];
			return true;
		}

		vtkToUnity = null;
		return false;
	}
	
	public bool ContainsKey(string key)
	{
		if (keys.Contains ((string)key))
			return true;
		
		return false;
	}
}
