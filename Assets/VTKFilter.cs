using UnityEngine;
using System.Collections;

public abstract class VTKFilter : MonoBehaviour 
{
	public abstract Kitware.VTK.vtkAlgorithmOutput ApplyFilter(Kitware.VTK.vtkAlgorithmOutput input);
}
