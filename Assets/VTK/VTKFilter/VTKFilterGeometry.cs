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

	protected override void ValidateInput(){}

	protected override void CalculateFilter()
	{
		outputType = VTK.DataType.PolyData;
		
		vtkFilter = Kitware.VTK.vtkGeometryFilter.New();
		
		vtkFilter.SetInputConnection(node.parent.filter.vtkFilter.GetOutputPort());

		vtkFilter.Update ();
	}
}
