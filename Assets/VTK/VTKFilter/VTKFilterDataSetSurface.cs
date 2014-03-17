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
	
	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		Kitware.VTK.vtkDataSetSurfaceFilter filter = Kitware.VTK.vtkDataSetSurfaceFilter.New ();
		
		filter.SetInputConnection(input);
		
		filter.Update ();

		base.SetVtkFilter (filter);
		
		return filter.GetOutputPort();
	}
}
