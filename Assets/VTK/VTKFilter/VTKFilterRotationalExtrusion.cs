using UnityEngine;
using System;
using Kitware.VTK;

[ExecuteInEditMode]
public class VTKFilterRotationalExtrusion : VTKFilter 
{
	[HideInInspector]
	public int resolution = 12;
	[HideInInspector]
	public float angle = 360.0f;
	[HideInInspector]
	public float translation = 0.0f;
	[HideInInspector]
	public float deltaRadius = 0.0f;
	[HideInInspector]
	public bool capping = true;

	protected void Reset ()
	{
		resolution = 12;
		angle = 360;
		translation = 0.0f;
		deltaRadius = 0.0f;
		vtkFilter = vtkRotationalExtrusionFilter.New ();
	}

	public override void UpdateFilter(Kitware.VTK.vtkAlgorithm input)
	{
		vtkFilter = vtkRotationalExtrusionFilter.New ();

		vtkFilter.SetInputConnection (input.GetOutputPort());

		((vtkRotationalExtrusionFilter)vtkFilter).SetResolution (resolution);
		((vtkRotationalExtrusionFilter)vtkFilter).SetAngle (angle);
		((vtkRotationalExtrusionFilter)vtkFilter).SetTranslation (translation);
		((vtkRotationalExtrusionFilter)vtkFilter).SetDeltaRadius (deltaRadius);
		((vtkRotationalExtrusionFilter)vtkFilter).SetCapping (Convert.ToInt32(capping));

		vtkFilter.Update ();
	}
}
