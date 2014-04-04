using UnityEngine;
using System.Collections;
using System.Reflection;
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
			"VTKFilterDataSetSurface",
			"VTKFilterGeometry",
			"VTKFilterRotationalExtrusion",
			"VTKFilterThreshold",
			"VTKFilterTransform"
		};

		return filters;
	}
}
