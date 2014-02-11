using UnityEngine;
using System.Collections;

public class VisualizationPipelineTest : MonoBehaviour
{
	/*
	public int resolution = 8;
	int oldResolution;
	
	Kitware.VTK.vtkSphereSource SphereSource;
	VtkToUnity vtkToUnity;
	
	void Start ()
	{
		SphereSource = Kitware.VTK.vtkSphereSource.New();
		SphereSource.SetRadius(1);
		SphereSource.Update();
		
		vtkToUnity = new VtkToUnity(SphereSource.GetOutputPort(), "VTK Sphere Source");
		vtkToUnity.ColorBy(Color.grey);
		vtkToUnity.Update();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
			resolution++;
		if (Input.GetKeyDown(KeyCode.DownArrow) && resolution > 3)
			resolution--;
		
		if (resolution != oldResolution)
		{
			SphereSource.SetPhiResolution(resolution);
			SphereSource.SetThetaResolution(resolution);
			vtkToUnity.Update();
			oldResolution = resolution;
		}
	}
	*/
}

