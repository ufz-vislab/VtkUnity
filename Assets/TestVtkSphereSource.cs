using UnityEngine;
using System.Collections;

/*
 * Creates a vtkSphereSource and converts it to a Unity GameObject.
 * Pressing up/down keyboard arrows changes the sphere resolution.
 *
 **/
public class TestVtkSphereSource : MonoBehaviour
{
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
}
