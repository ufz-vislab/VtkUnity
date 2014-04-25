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
	
	public override void Reset()
	{
		numContours = 10;
		range = Vector2.zero;
		vtkFilter = vtkContourFilter.New();
		selectedDataArray = 0;
	}

	protected override void ValidateInput()
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

		/* TODO die reichweiten sollten schon hier abgefragt und gesetzt werden, dass für so aber zu
		  *abstürtzen
		  */
		//Get allowed data range
		string dataArray = gameObject.GetComponent<VTKProperties>().dataArrays[selectedDataArray];
		dataArray = dataArray.Remove(dataArray.IndexOf("[") - 1);

		vtkDataSet dataSet = vtkDataSet.SafeDownCast (node.parent.filter.vtkFilter.GetOutputDataObject (0));
			
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
		
		// Check for allowed data range
		if (range.x < dataRange[0])
			range.x = (float)dataRange[0];
		if (range.x > dataRange[1])
			range.x = (float)dataRange[1];
		if (range.y < dataRange[0])
			range.y = (float) dataRange[0];
		if (range.y > dataRange[1])
			range.y = (float) dataRange[1];
	}
	
	public override void SetPlaymodeParameters()
	{
		playmodeParameters = new ListOfPlaymodeParameter ();
		playmodeParameters.Add (new PlaymodeParameter("selectedDataArray", "int", 1.0f));
		playmodeParameters.Add (new PlaymodeParameter("numContours", "int", 1.0f));
		playmodeParameters.Add (new PlaymodeParameter("range", "Vector2", 1.0f));
	}

	protected override void CalculateFilter()
	{
		outputType = VTK.DataType.PolyData;

		vtkFilter = vtkContourFilter.New ();
		
		string dataArray = gameObject.GetComponent<VTKProperties>().dataArrays[selectedDataArray];
		dataArray = dataArray.Remove(dataArray.IndexOf("[") - 1);

		vtkFilter.SetInputConnection(node.parent.filter.vtkFilter.GetOutputPort());
			
		vtkFilter.SetInputArrayToProcess(0, 0, 0, 
			(int)vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, dataArray);

		((vtkContourFilter)vtkFilter).GenerateValues(numContours, range.x, range.y);
		((vtkContourFilter)vtkFilter).ComputeScalarsOn();
		vtkFilter.Update();
	}
}
