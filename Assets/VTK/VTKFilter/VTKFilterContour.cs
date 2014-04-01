using System;
using UnityEngine;
using Kitware.VTK;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterContour : VTKFilter
{
	[HideInInspector]
	public int numContours = 10;
	[HideInInspector]
	public Vector2 range;
	[HideInInspector]
	public int selectedDataArray = 0;

	protected double[] dataRange = new double[2];
	
	protected void Reset()
	{
		numContours = 10;
		range = Vector2.zero;
		vtkFilter = vtkContourFilter.New();
	}

	public override void ValidateInput()
	{
		string[] dataArrays = gameObject.GetComponent<VTKProperties>().dataArrays;
		if(selectedDataArray < 0)
			selectedDataArray = 0;
		if(selectedDataArray > dataArrays.Length -1)
			selectedDataArray = dataArrays.Length -1;


		// Check for allowed contour numbers
		if (numContours < 1)
			numContours = 1;
		if (numContours > 30)
			numContours = 30;

		// Check for allowed data range
		if (range.x < dataRange[0])
			range.x = (float)dataRange[0];
		if (range.x > dataRange[1])
			range.x = (float)dataRange[1];
		if (range.y > dataRange[1])
			range.y = (float) dataRange[1];
		if (range.y < dataRange[0])
			range.y = (float) dataRange[0];
	}
	
	public override void SetPlaymodeParameters()
	{
		this.playmodeParameters = new ListOfPlaymodeParameter ();
		this.playmodeParameters.Add (new PlaymodeParameter("selectedDataArray", "int", 1.0f));
		this.playmodeParameters.Add (new PlaymodeParameter("numContours", "int", 1.0f));
		this.playmodeParameters.Add (new PlaymodeParameter("range", "Vector2", 1.0f));
	}

	public override void UpdateFilter(vtkAlgorithm input)
	{
		vtkFilter = vtkContourFilter.New ();

		string dataArray = gameObject.GetComponent<VTKProperties>().dataArrays[selectedDataArray];
		dataArray = dataArray.Remove(dataArray.IndexOf("[") - 1);
		
		vtkFilter.SetInputConnection(input.GetOutputPort());
		vtkDataSet dataSet = vtkDataSet.SafeDownCast (input.GetOutputDataObject (0));
		vtkFilter.SetInputArrayToProcess(0, 0, 0, (int)vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, dataArray);
		
		if(dataSet != null)
		{
			vtkPointData pointData = dataSet.GetPointData();
			if(pointData != null)
			{
				pointData.SetActiveScalars(dataArray);
				dataRange = pointData.GetScalars().GetRange();
				// Sets initial range
				if(range == Vector2.zero)
					range = new Vector2((float)dataRange[0], (float)dataRange[1]);
			}
		}
		
		((vtkContourFilter)vtkFilter).GenerateValues(numContours, range.x, range.y);
		((vtkContourFilter)vtkFilter).ComputeScalarsOn();
		vtkFilter.Update();
	}
}
