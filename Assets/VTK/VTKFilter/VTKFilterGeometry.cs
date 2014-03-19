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

	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input)
	{
		Kitware.VTK.vtkGeometryFilter filter = Kitware.VTK.vtkGeometryFilter.New();
		
		filter.SetInputConnection(input.GetOutputPort());

		filter.Update ();

		base.vtkFilter = filter;
	}
}
