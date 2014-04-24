using UnityEngine;
using System.Collections;
using Kitware.VTK;
using System.Reflection;

[System.Serializable]
public abstract class VTKFilter : MonoBehaviour 
{
	[HideInInspector]
	public vtkAlgorithm vtkFilter;
	[HideInInspector]
	public VTK.DataType outputType;
	[HideInInspector]
	public VTKNode node;
	[HideInInspector]
	public ListOfPlaymodeParameter playmodeParameters = null;

	//Here goes the filter stuff
	protected abstract void CalculateFilter();

	protected abstract void ValidateInput();

	public abstract void SetPlaymodeParameters();

	public virtual void UpdateInput()
	{
		ValidateInput ();
		CalculateFilter();
	}
}
