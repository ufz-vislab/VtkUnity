using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEditor;

public static class VTK 
{
	public enum DataType
	{
		PolyData,
		UnstructuredGrid
	}

	public static string[] GetSupportedFiltersByName()
	{
		List<string> filters = new List<string>();

		string path = Application.dataPath+"/VTK/VTKFilter/";
		DirectoryInfo dirInfo = new DirectoryInfo (path);
		FileInfo[] filesInfo = dirInfo.GetFiles("*.cs");

		for(int i = 0; i < filesInfo.Length; i++)
		{
			filters.Add( filesInfo[i].Name.Remove(filesInfo[i].Name.LastIndexOf("."), 3));
		}

		//Remove dummy filter for root node
		filters.Remove("VTKFilterRootNode");

		return filters.ToArray();
	}

	public static string GetGameObjectName(VTKNode node)
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

	public static string GetFileName(string filepath)
	{
		return filepath.Remove(0, filepath.LastIndexOf("/")+1);
	}
}
