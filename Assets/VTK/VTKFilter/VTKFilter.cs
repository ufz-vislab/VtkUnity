using UnityEngine;
using System.Collections.Generic;
using Kitware.VTK;

[System.Serializable]
public abstract class VTKFilter : MonoBehaviour 
{
	[HideInInspector]
	public vtkAlgorithm vtkFilter;
	[HideInInspector]
	public VTKNode node;

	//Here goes the filter stuff
	public abstract void UpdateFilter(Kitware.VTK.vtkAlgorithm input);
}
