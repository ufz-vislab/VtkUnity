using UnityEngine;
using System.Collections;

public static class VTK 
{
	public enum FilterType
	{
		NotSet,
		PolyData,
		UnstructuredGrid
	}

	public static string[] GetFiltersByName()
	{
		string[] filters = new string[]
		{
			"VTKFilterContour",
			"VTKFilterGeometry",
			"VTKFilterRotationalExtrusion",
			"VTKFilterTransform"
		};

		return filters;
	}

	public static bool ApplicableFilters(VTKNode parent, VTKNode child)
	{
		if(parent.filter.OutputType == FilterType.NotSet)
		{
			Debug.LogWarning("Output type for " + parent.name + " not set");
			return false;
		}

		if(child.filter.InputType == FilterType.NotSet)
		{
			Debug.LogWarning("Input type for " + child.name + " not set");
			return false;
		}

		if (parent.filter.OutputType == child.filter.InputType) 
		{
			return true;
		}
		else
		{
			Debug.LogWarning("Output of " + parent.name + " [" + parent.filter.OutputType 
			                 + "] and input of " + child.name + " " +
			                 	"[" + child.filter.InputType + "] not applicable");

			return false;
		}


	}
}
