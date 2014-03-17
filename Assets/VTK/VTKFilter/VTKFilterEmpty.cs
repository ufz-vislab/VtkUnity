using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKFilterEmpty : VTKFilter 
{
	public override Kitware.VTK.vtkAlgorithm GetVtkFilter()
	{
			VTKRoot root = gameObject.GetComponent<VTKRoot>();

			if(root.readerType == VTK.FilterType.PolyData)

				return root.polyDataReader;

			if(root.readerType == VTK.FilterType.UnstructuredGrid)
				return root.unstructuredGridReader;

			return null;
	}

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
