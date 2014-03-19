using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterDataSetSurface : VTKFilter 
{
	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.UnstructuredGrid;
		base.OutputType = VTK.FilterType.PolyData;
	}
	
	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input)
	{
		Kitware.VTK.vtkDataSetSurfaceFilter filter = Kitware.VTK.vtkDataSetSurfaceFilter.New ();
		
		filter.SetInputConnection(input.GetOutputPort());
		
		filter.Update ();

		base.vtkFilter = filter;
	}
}
