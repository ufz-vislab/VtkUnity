using UnityEngine;
using System.Collections;
using Kitware.VTK;

public class VTKFilterClip : VTKFilter 
{

	public override void Reset()
	{
		vtkFilter = vtkClipDataSet.New ();
	}
	
	protected override void ValidateInput ()
	{
		//TODO
	}
	
	public override void SetPlaymodeParameters ()
	{
		playmodeParameters = new ListOfPlaymodeParameter ();
		playmodeParameters.Add(new PlaymodeParameter("numPoints", "int", 1.0f));
		playmodeParameters.Add(new PlaymodeParameter("radius", "int", 1.0f));
		playmodeParameters.Add(new PlaymodeParameter("center", "Vector3", 1.0f));
		playmodeParameters.Add (new PlaymodeParameter ("selectedDataArray", "int", 1.0f));
	}
	
	protected override void CalculateFilter ()
	{
		outputType = VTK.DataType.PolyData;
		
		vtkPlane plane = vtkPlane.New ();
		plane.SetOrigin (0, 75, -4000);
		plane.SetNormal (1.0, 0.0, 0.0);

		if(node.parent.filter.outputType == VTK.DataType.PolyData)
		{
			vtkFilter = vtkClipPolyData.New();
		}

		if(node.parent.filter.outputType == VTK.DataType.UnstructuredGrid)
		{
			vtkFilter = vtkClipDataSet.New();
		}

		vtkFilter.SetInputConnection (node.parent.filter.vtkFilter.GetOutputPort());
		((vtkClipDataSet)vtkFilter).SetClipFunction (plane);
		vtkFilter.Update ();
	}	
}
