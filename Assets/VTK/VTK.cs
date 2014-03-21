using UnityEngine;
using System.Collections;
using UnityEditor;

public static class VTK 
{
	public enum ReaderType
	{
		PolyData,
		UnstructuredGrid
	}

	public static string[] GetSupportedFiltersByName()
	{
		string[] filters = new string[]
		{
			"VTKFilterContour",
			"VTKFilterGeometry",
			"VTKFilterRotationalExtrusion",
			"VTKFilterTransform",
			"VTKFilterDataSetSurface"
		};

		return filters;
	}
}
