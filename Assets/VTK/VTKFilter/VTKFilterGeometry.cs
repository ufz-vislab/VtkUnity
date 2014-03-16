using UnityEngine;
using System.Collections;

/*
 * Takes unstructured grid and generates poly data out of it
 * */
[ExecuteInEditMode]
public class VTKFilterGeometry : VTKFilter 
{
	[HideInInspector]
	public Kitware.VTK.vtkGeometryFilter filter;

	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.UnstructuredGrid;
		base.OutputType = VTK.FilterType.PolyData;
	}

	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		filter = Kitware.VTK.vtkGeometryFilter.New();
		
		filter.SetInputConnection(input);

		filter.Update ();

		return filter.GetOutputPort();
	}
}
