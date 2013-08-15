using UnityEngine;
using System.Collections;

/*
 * Loads a vtkPolyData from an XML file (vtp).
 *
 **/
public class TestVtkFileReader : MonoBehaviour
{
	void Start ()
	{
		string filepath = Application.dataPath + "/" + "Vtk-Data/Box.vtp";
		filepath = filepath.Replace("/", "\\");
		Kitware.VTK.vtkXMLPolyDataReader reader = Kitware.VTK.vtkXMLPolyDataReader.New();
		if (reader.CanReadFile(filepath) == 0)
		{
			Debug.Log(filepath + " could not be loaded by Vtk!");
			return;
		}
		reader.SetFileName(filepath);
		reader.Update();

		VtkToUnity vtkToUnity = new VtkToUnity(reader.GetOutputPort(), "Vtk-Data/Box.vtp");
		vtkToUnity.ColorBy("Elevation", VtkToUnity.VtkDataType.POINT_DATA);
		vtkToUnity.SetLut(VtkToUnity.LutPreset.BLUE_RED);
		vtkToUnity.Update();
        vtkToUnity.go.transform.Translate(-2f, 0f, 0f);
		
		Kitware.VTK.vtkContourFilter contours = Kitware.VTK.vtkContourFilter.New();
		contours.SetInputConnection(vtkToUnity.triangleFilter.GetOutputPort());
		contours.SetInputArrayToProcess(0,0,0, (int)Kitware.VTK.vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, "Elevation");
		for(int i = 0; i < 10; ++i)
			contours.SetValue(i, i / 10.0);
		contours.ComputeScalarsOn();
		VtkToUnity vtkToUnityContours = new VtkToUnity(contours.GetOutputPort(), "Contours");
		vtkToUnityContours.ColorBy("Elevation", VtkToUnity.VtkDataType.POINT_DATA);
		vtkToUnityContours.SetLut(VtkToUnity.LutPreset.BLUE_RED);
		vtkToUnityContours.Update();
		vtkToUnityContours.go.transform.Translate(-4f, 0f, 0f);

		// Points
		filepath = Application.dataPath + "/Vtk-Data/Points.vtp";
		filepath = filepath.Replace("/", "\\");
		if (reader.CanReadFile(filepath) == 0)
		{
			Debug.Log(filepath + " could not be loaded by Vtk!");
			return;
		}
		reader.SetFileName(filepath);
		reader.Update();

		VtkToUnity vtkToUnityPoints = new VtkToUnity(reader.GetOutputPort(), "Vtk-Data/Points.vtp");
		vtkToUnityPoints.ColorBy("Elevation", VtkToUnity.VtkDataType.POINT_DATA);
		vtkToUnityPoints.SetLut(VtkToUnity.LutPreset.RED_BLUE);
		vtkToUnityPoints.Update();
		vtkToUnityPoints.go.transform.Translate(2f, 0f, 0f);
	}
}
