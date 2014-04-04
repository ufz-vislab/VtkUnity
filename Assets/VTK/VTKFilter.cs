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
	public VTKNode node;
	[HideInInspector]
	public ListOfPlaymodeParameter playmodeParameters = null;

	//Here goes the filter stuff
	public abstract void UpdateFilter(Kitware.VTK.vtkAlgorithm input);

	public abstract void ValidateInput();

	public abstract void SetPlaymodeParameters();

	public virtual void UpdateInput()
	{
		ValidateInput ();
		VTKRoot root = gameObject.GetComponent<VTKRoot> ();
		root.Modifie(node);
	}
}
