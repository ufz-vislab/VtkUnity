using UnityEngine;
using System.Collections;

/*
 * Takes unstructured grid and generates poly data out of it
 * */
[ExecuteInEditMode]
public class VTKFilterGeometry : VTKFilter 
{
	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.UnstructuredGrid;
		base.OutputType = VTK.FilterType.PolyData;
	}

	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		Kitware.VTK.vtkGeometryFilter filter = Kitware.VTK.vtkGeometryFilter.New();
		
		filter.SetInputConnection(input);

		filter.Update ();

		base.SetVtkFilter (filter);

		return filter.GetOutputPort();
	}
}
