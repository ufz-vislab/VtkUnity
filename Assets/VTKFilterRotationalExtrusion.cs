using UnityEngine;
using System;

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

	[HideInInspector]
	public Kitware.VTK.vtkRotationalExtrusionFilter filter;

	public override Kitware.VTK.vtkAlgorithmOutput ApplyFilter(Kitware.VTK.vtkAlgorithmOutput input)
	{
		filter = Kitware.VTK.vtkRotationalExtrusionFilter.New ();

		filter.SetInputConnection (input);

		filter.SetResolution (resolution);
		filter.SetAngle (angle);
		filter.SetTranslation (translation);
		filter.SetDeltaRadius (deltaRadius);
		filter.SetCapping (Convert.ToInt32(capping));
	
		return filter.GetOutputPort ();
	}
}
