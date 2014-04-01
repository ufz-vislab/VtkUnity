using UnityEngine;
using System.Collections;
using Kitware.VTK;

[ExecuteInEditMode]
public class VTKFilterDataSetSurface : VTKFilter 
{
	public override void SetPlaymodeParameters (){}

	public override void ValidateInput(){}

	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input)
	{
		vtkFilter = Kitware.VTK.vtkDataSetSurfaceFilter.New ();
		
		vtkFilter.SetInputConnection(input.GetOutputPort());
		
		vtkFilter.Update ();
	}
}
