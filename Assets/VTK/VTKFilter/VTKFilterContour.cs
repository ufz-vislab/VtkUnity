using System;
using UnityEngine;
using Kitware.VTK;

[ExecuteInEditMode]
public class VTKFilterContour : VTKFilter
{
	public int numContours;
	public Vector2 range;

	protected double[] dataRange = new double[2];

	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.PolyData;
		base.OutputType = VTK.FilterType.PolyData;
	}

	protected void Reset()
	{
		numContours = 10;
		range = Vector2.zero;
		vtkFilter = vtkContourFilter.New();
	}

	protected void OnValidate()
	{
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

		VTKRoot root = gameObject.GetComponent<VTKRoot>();
		root.Modifie(root.activeNode);
	}

	public override void UpdateFilter(vtkAlgorithm input)
	{
		vtkFilter.SetInputConnection(input.GetOutputPort());
		vtkDataSet dataSet = vtkDataSet.SafeDownCast (input.GetOutputDataObject (0));
		vtkFilter.SetInputArrayToProcess(0, 0, 0, (int)vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, "PRESSURE1_average");

		if(dataSet != null)
		{
			vtkPointData pointData = dataSet.GetPointData();
			if(pointData != null)
			{
				pointData.SetActiveScalars("PRESSURE1_average");
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
