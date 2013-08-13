using UnityEngine;
using System.Collections;

/*
 * Loads a vtkPolyData from an XML file (vtp).
 *
 **/
public class TestVtkFileReader : MonoBehaviour
{

	public string filename = "Box.vtp";

	VtkToUnity vtkToUnity;

	void Start ()
	{
		vtkToUnity = new VtkToUnity(filename);
        vtkToUnity.go.transform.Translate(-2f, 0f, 0f);
	}
}
