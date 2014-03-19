using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class VTKFilter : MonoBehaviour 
{
	[HideInInspector]
	public Kitware.VTK.vtkAlgorithm vtkFilter;
	[HideInInspector]
	public VTKNode node;
	[HideInInspector]
	public VTK.FilterType InputType = VTK.FilterType.NotSet;
	[HideInInspector]
	public VTK.FilterType OutputType = VTK.FilterType.NotSet;

	//Here set InputType and OutputType
	protected abstract void OnEnable();

	//Here goes the filter stuff
	public abstract void UpdateFilter(Kitware.VTK.vtkAlgorithm input);
}
