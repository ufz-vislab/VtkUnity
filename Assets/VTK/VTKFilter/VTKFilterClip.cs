using UnityEngine;
using System.Collections;
using Kitware.VTK;

public class VTKFilterClip : VTKFilter 
{
	[HideInInspector]
	public float originX = 0.0f;
	[HideInInspector]
	public float originY = 0.0f;
	[HideInInspector]
	public float originZ = 0.0f;
	[HideInInspector]
	public float normalX = 0.0f;
	[HideInInspector]
	public float normalY = 0.0f;
	[HideInInspector]
	public float normalZ = 0.0f;

	public override void Reset()
	{
		originX = 0.0f;
		originY = 0.0f;
		originZ = 0.0f;

		normalX = 0.0f;
		normalY = 0.0f;
		normalZ = 0.0f;
	}
	
	protected override void ValidateInput (){}
	
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
		plane.SetOrigin (originX, originY, originZ);
		plane.SetNormal (normalX, normalY, normalZ);

		if(node.parent.filter.outputType == VTK.DataType.PolyData)
		{
			vtkFilter = vtkClipPolyData.New();
			vtkFilter.SetInputConnection (node.parent.filter.vtkFilter.GetOutputPort());
			((vtkClipPolyData)vtkFilter).SetClipFunction (plane);
		}

		if(node.parent.filter.outputType == VTK.DataType.UnstructuredGrid)
		{
			vtkFilter = vtkClipDataSet.New();
			vtkFilter.SetInputConnection (node.parent.filter.vtkFilter.GetOutputPort());
			((vtkClipDataSet)vtkFilter).SetClipFunction (plane);
		}
		
		vtkFilter.Update ();
	}	
}
