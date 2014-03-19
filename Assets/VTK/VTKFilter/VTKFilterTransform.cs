using UnityEngine;

/*
 * Translate, rotate or scale
 * */

[ExecuteInEditMode]
public class VTKFilterTransform : VTKFilter 
{
	[HideInInspector]
	public float translateX = 0.0f;
	[HideInInspector]
	public float translateY = 0.0f;
	[HideInInspector]
	public float translateZ = 0.0f;

	[HideInInspector]
	public float rotateX = 0.0f;
	[HideInInspector]
	public float rotateY = 0.0f;
	[HideInInspector]
	public float rotateZ = 0.0f;

	[HideInInspector]
	public float scaleX = 1.0f;
	[HideInInspector]
	public float scaleY = 1.0f;
	[HideInInspector]
	public float scaleZ = 1.0f;

	[HideInInspector]
	public Kitware.VTK.vtkTransform vtkTransform;

	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.PolyData;
		base.OutputType = VTK.FilterType.PolyData;
	}

	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input)
	{
		Kitware.VTK.vtkTransformFilter filter = Kitware.VTK.vtkTransformFilter.New ();

		vtkTransform = Kitware.VTK.vtkTransform.New ();

		filter.SetInputConnection (input.GetOutputPort());

		SetTranslation ();
		SetRotation ();
		SetScale ();

		filter.SetTransform (vtkTransform);

		filter.Update ();

		vtkFilter = filter;
	}

	public void SetTranslation ()
	{
		//TODO change to vtk data?
		gameObject.transform.Translate (translateX, translateY, translateZ);

		//vtkTransform.Translate (translateX, translateY, translateZ);
	}

	public void SetRotation()
	{
		vtkTransform.RotateX (rotateX);
		vtkTransform.RotateY (rotateY);
		vtkTransform.RotateZ (rotateZ);
	}

	public void SetScale()
	{
		vtkTransform.Scale (scaleX, scaleY, scaleZ);
	}
}
