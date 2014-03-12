using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterEmpty : VTKFilter 
{
	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		return input;
	}
}
