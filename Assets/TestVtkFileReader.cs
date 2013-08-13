using UnityEngine;
using System.Collections;

/*
 * Loads a vtkPolyData from an XML file (vtp).
 *
 **/
public class TestVtkFileReader : MonoBehaviour
{

	string filename = "Vtk-Data/Box.vtp";

	VtkToUnity vtkToUnity;

	void Start ()
	{
		string filepath = Application.dataPath + "/" + filename;
		filepath = filepath.Replace("/", "\\");
		Kitware.VTK.vtkXMLPolyDataReader reader = Kitware.VTK.vtkXMLPolyDataReader.New();
		if (reader.CanReadFile(filepath) == 0)
		{
			Debug.Log(filepath + " could not be loaded by Vtk!");
			return;
		}
		reader.SetFileName(filepath);
		reader.Update();

		vtkToUnity = new VtkToUnity(reader.GetOutputPort(), filename);
        vtkToUnity.go.transform.Translate(-2f, 0f, 0f);
		
	}
}
