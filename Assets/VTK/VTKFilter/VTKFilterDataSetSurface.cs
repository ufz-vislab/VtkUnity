using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterDataSetSurface : VTKFilter 
{
	[HideInInspector]
	public Kitware.VTK.vtkDataSetSurfaceFilter filter;
	
	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.UnstructuredGrid;
		base.OutputType = VTK.FilterType.PolyData;
	}
	
	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		filter = Kitware.VTK.vtkDataSetSurfaceFilter.New ();
		
		filter.SetInputConnection(input);
		
		filter.Update ();
		
		return filter.GetOutputPort();
	}
}
