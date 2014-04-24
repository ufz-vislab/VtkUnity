using UnityEngine;
using System.Collections;
using Kitware.VTK;

[ExecuteInEditMode]
public class VTKFilterDataSetSurface : VTKFilter 
{
	public override void SetPlaymodeParameters (){}

	protected override void ValidateInput(){}

	protected override void CalculateFilter()
	{
		outputType = VTK.DataType.PolyData;
		
		vtkFilter = Kitware.VTK.vtkDataSetSurfaceFilter.New ();
		
		vtkFilter.SetInputConnection(node.parent.filter.vtkFilter.GetOutputPort());
		
		vtkFilter.Update ();
	}
}
