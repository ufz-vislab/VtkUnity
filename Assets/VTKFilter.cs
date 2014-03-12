using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class VTKFilter : MonoBehaviour 
{
	public Kitware.VTK.vtkAlgorithmOutput output;
	
	public virtual Kitware.VTK.vtkAlgorithmOutput UpdateFilter(Kitware.VTK.vtkAlgorithmOutput input)
	{
		if (input == null) 
		{
			input = Kitware.VTK.vtkAlgorithmOutput.New();
		}

		output = GenerateOutput(input);

		return output;
	}

	//Here goes whatever the filter does
	protected abstract Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input);
}
