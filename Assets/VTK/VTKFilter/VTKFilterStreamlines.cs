using UnityEngine;
using System.Collections;
using Kitware.VTK;

[ExecuteInEditMode]
public class VTKFilterStreamlines : VTKFilter 
{
	[HideInInspector]
	public int numPoints;
	[HideInInspector]
	public float radius;
	[HideInInspector]
	public Vector3 center;
	[HideInInspector]
	public int selectedDataArray;
	
	protected void Reset()
	{
		center = Vector3.zero;
		vtkFilter = vtkStreamTracer.New ();
		selectedDataArray = 0;
	}
	
	protected override void ValidateInput ()
	{
		//TODO
		numPoints = 100;
		radius = 80;
		center.x = 0;
		center.y = 75;
		center.z = -4000;
		selectedDataArray = 80;
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
		vtkPointSource points = vtkPointSource.New ();
		points.SetNumberOfPoints (100/*numPoints*/);
		points.SetRadius (80/*radius*/);
		points.SetCenter (0, 75, -4000/*center.x, center.y, center.z*/);
		
		string dataArray = gameObject.GetComponent<VTKProperties>().dataArrays[selectedDataArray];
		dataArray = dataArray.Remove(dataArray.IndexOf("[") - 1);

		vtkFilter = vtkStreamTracer.New ();
		vtkFilter.SetInputConnection (node.parent.filter.vtkFilter.GetOutputPort());
		vtkFilter.SetInputArrayToProcess(0, 0, 0, 
			(int)vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, 
			"velocity_average"/*dataArray*/);
		((vtkStreamTracer)vtkFilter).SetSourceConnection (/*points*/points.GetOutputPort ());
		((vtkStreamTracer)vtkFilter).SetInterpolatorTypeToDataSetPointLocator ();
		vtkFilter.Update ();
	}	
}
