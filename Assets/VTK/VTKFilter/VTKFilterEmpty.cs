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

	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input){}
}
