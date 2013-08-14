using UnityEngine;
using System.Collections;

/*
 * Loads a vtkPolyData from an XML file (vtp).
 *
 **/
public class TestVtkFileReader : MonoBehaviour
{
	string filename = "Vtk-Data/Box.vtp";

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

		VtkToUnity vtkToUnity = new VtkToUnity(reader.GetOutputPort(), filename);
		vtkToUnity.ColorBy("Elevation", VtkToUnity.VtkDataType.POINT_DATA);
		vtkToUnity.Update();
        vtkToUnity.go.transform.Translate(-2f, 0f, 0f);
		
		Kitware.VTK.vtkContourFilter contours = Kitware.VTK.vtkContourFilter.New();
		contours.SetInputConnection(vtkToUnity.triangleFilter.GetOutputPort());
		contours.SetInputArrayToProcess(0,0,0, (int)Kitware.VTK.vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, "Elevation");
		for(int i = 0; i < 10; ++i)
			contours.SetValue(i, i / 10.0);
		VtkToUnity vtkToUnityContours = new VtkToUnity(contours.GetOutputPort(), "Contours");
		vtkToUnityContours.Update();
		vtkToUnityContours.go.transform.Translate(-4f, 0f, 0f);
	}
}
