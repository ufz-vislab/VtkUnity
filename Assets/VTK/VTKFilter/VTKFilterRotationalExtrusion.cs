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

	protected override void OnEnable ()
	{
		base.InputType = VTK.FilterType.PolyData;
		base.OutputType = VTK.FilterType.PolyData;
	}

	protected override Kitware.VTK.vtkAlgorithmOutput GenerateOutput(Kitware.VTK.vtkAlgorithmOutput input)
	{
		Kitware.VTK.vtkRotationalExtrusionFilter filter = Kitware.VTK.vtkRotationalExtrusionFilter.New ();

		filter.SetInputConnection (input);

		filter.SetResolution (resolution);
		filter.SetAngle (angle);
		filter.SetTranslation (translation);
		filter.SetDeltaRadius (deltaRadius);
		filter.SetCapping (Convert.ToInt32(capping));
		filter.Update ();

		base.SetVtkFilter (filter);

		return filter.GetOutputPort ();
	}
}
