using UnityEngine;
using System.Collections;

public class TestVtkRunInEditor : MonoBehaviour
{
	public int resolution = 8;
	int oldResolution;

	Kitware.VTK.vtkSphereSource SphereSource;
	VtkToUnity vtkToUnity = null;

	void Start ()
	{
		Generate();
	}

	[ContextMenu("Reset Vtk")]
	void Reset()
	{
		DestroyImmediate(vtkToUnity.go);
		vtkToUnity = null;
	}

	[ContextMenu("Generate Vtk")]
	void Generate()
	{
		if(SphereSource == null)
			SphereSource = Kitware.VTK.vtkSphereSource.New();
		if (vtkToUnity == null)
		{
			vtkToUnity = new VtkToUnity(SphereSource.GetOutputPort(), "VTK Run In Editor");
			vtkToUnity.ColorBy(Color.green);
		}
		SphereSource.SetPhiResolution(resolution);
		SphereSource.SetThetaResolution(resolution);
		SphereSource.SetRadius(1);
		SphereSource.Update();

		vtkToUnity.Update();
	}
	
	void Update ()
	{
		if (resolution != oldResolution)
		{
			SphereSource.SetPhiResolution(resolution);
			SphereSource.SetThetaResolution(resolution);
			vtkToUnity.Update();
			oldResolution = resolution;
		}
	}
}
