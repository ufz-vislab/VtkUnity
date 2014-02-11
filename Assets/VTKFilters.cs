using UnityEngine;
using System.Collections;

public static class VTKFilters 
{
	public static string[] GetFiltersByName()
	{
		string[] filters = new string[]{
			"VTKFilterTransform",
			"VTKFilterRotationalExtrusion"
		};

		return filters;
	}
}
