using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterContour : VTKFilter 
{
	[HideInInspector]
	public string DataSet;

	[HideInInspector]
	public Kitware.VTK.vtkContourFilter filter;

	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.UnstructuredGrid;
		base.OutputType = VTK.FilterType.PolyData;
	}

	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		//vtkContourFilter will select the best contouring function for the given dataset type autoatically
		filter = Kitware.VTK.vtkContourFilter.New ();
		
		filter.SetInputConnection (input);

		filter.ComputeNormalsOn ();

		/*
		filter.SetInputArrayToProcess(0,0,0, (int)Kitware.VTK.vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, "Elevation");
		for(int i = 0; i < 10; ++i)
			filter.SetValue(i, i / 10.0);
		filter.ComputeScalarsOn();
		*/
		return filter.GetOutputPort();
	}
}
