using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterContour : VTKFilter 
{
	[HideInInspector]
	public int numContours = 0;
	[HideInInspector]
	public double[] range;

	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.UnstructuredGrid;
		base.OutputType = VTK.FilterType.PolyData;
	}

	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		Kitware.VTK.vtkContourFilter filter = Kitware.VTK.vtkContourFilter.New ();

		Kitware.VTK.vtkDataSet dataSet = Kitware.VTK.vtkDataSet.SafeDownCast (base.parentVtkFilter.GetOutputDataObject (0));

		if(dataSet != null)
		{
			Kitware.VTK.vtkPointData pointData = dataSet.GetPointData();

			if(pointData != null)
			{
				range = pointData.GetScalars().GetRange();
			}
		}

		filter.GenerateValues (numContours, range[0], range[1]);

		filter.Update ();

		base.SetVtkFilter (filter);

		return filter.GetOutputPort();
	}
}
