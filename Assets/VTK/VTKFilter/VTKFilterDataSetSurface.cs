using UnityEngine;
using System.Collections;
using Kitware.VTK;

[ExecuteInEditMode]
public class VTKFilterDataSetSurface : VTKFilter 
{
	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input)
	{
		vtkFilter = Kitware.VTK.vtkDataSetSurfaceFilter.New ();
		
		vtkFilter.SetInputConnection(input.GetOutputPort());
		
		vtkFilter.Update ();
	}
}
