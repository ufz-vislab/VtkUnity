using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterContour : VTKFilter 
{
	[HideInInspector]
	public int numContours = 10;

	public double[] range;

	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.PolyData;
		base.OutputType = VTK.FilterType.PolyData;
	}

	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input)
	{
		Kitware.VTK.vtkContourFilter filter = Kitware.VTK.vtkContourFilter.New ();

		filter.SetInputConnection (input.GetOutputPort());

		Kitware.VTK.vtkDataSet dataSet = Kitware.VTK.vtkDataSet.SafeDownCast (input.GetOutputDataObject (0));

		filter.SetInputArrayToProcess(0, 0, 0, (int)Kitware.VTK.vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, node.properties.dataArray [0]);

		if(dataSet != null)
		{
			Kitware.VTK.vtkPointData pointData = dataSet.GetPointData();
		
			if(pointData != null)
			{
				//Debug.LogWarning(pointData.ToString());
				//Debug.LogWarning(pointData.GetScalars().ToString());
				//Debug.LogWarning(pointData.GetScalars().GetRange().ToString());
				//pointData.Update();
				//range = pointData.GetScalars().GetRange();
			}
		}

		filter.GenerateValues (1, 0, 0);
		/*
		filter.ComputeNormalsOn();
		filter.ComputeScalarsOn ();
		*/
		//filter.GenerateValues (10, 0, 1);

		filter.Update ();

		base.vtkFilter = filter;
	}
}
