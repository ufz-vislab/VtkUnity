using UnityEngine;
using System.Collections;
using Kitware.VTK;

/*
 * Takes unstructured grid and generates poly data out of it
 * */
[ExecuteInEditMode]
public class VTKFilterGeometry : VTKFilter 
{
	public override void SetPlaymodeParameters(){}

	public override void ValidateInput(){}

	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input)
	{
		vtkFilter = Kitware.VTK.vtkGeometryFilter.New();
		
		vtkFilter.SetInputConnection(input.GetOutputPort());

		vtkFilter.Update ();
	}
}
