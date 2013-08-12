using UnityEngine;
using System.Collections;

/*
 * Loads a vtkPolyData from an XML file (vtp).
 * 
 **/
public class TestVtkFileReader : MonoBehaviour
{

    public string filename = "hasselvorsperre.vtp";

    VtkToUnity vtkToUnity;

	void Start ()
    {
        vtkToUnity = new VtkToUnity(filename);
	}
}
