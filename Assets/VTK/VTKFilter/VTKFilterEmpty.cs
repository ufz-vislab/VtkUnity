using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterEmpty : VTKFilter 
{
	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.NotSet;
		base.OutputType = VTK.FilterType.NotSet;
	}

	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		return input;
	}
}
