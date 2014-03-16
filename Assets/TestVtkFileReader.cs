using UnityEngine;
using System.Collections;

/*
 * Loads a vtkPolyData from an XML file (vtp).
 *
 **/
[ExecuteInEditMode]
public class TestVtkFileReader : MonoBehaviour
{
	void Start ()
	{
		string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Vtk-Data/Box.vtp"); //Application.dataPath + "/" + "Vtk-Data/Box.vtp";
		//filepath = filepath.Replace("/", "\\");
		Kitware.VTK.vtkXMLPolyDataReader reader = Kitware.VTK.vtkXMLPolyDataReader.New();
		if (reader.CanReadFile(filepath) == 0)
		{
			Debug.Log(filepath + " could not be loaded by Vtk!");
			return;
		}
		reader.SetFileName(filepath);
		reader.Update();
		
		VtkToUnity vtkToUnity = new VtkToUnity(reader.GetOutputPort(), "Vtk-Data/Box.vtp");
		vtkToUnity.ColorBy("Elevation", VtkToUnity.VtkColorType.POINT_DATA);
		vtkToUnity.SetLut(VtkToUnity.LutPreset.BLUE_RED);
		//vtkToUnity.ColorBy(Color.red);
		vtkToUnity.Update();
		vtkToUnity.go.transform.Translate(-2f, 0f, 0f);
		
		Kitware.VTK.vtkContourFilter contours = Kitware.VTK.vtkContourFilter.New();
		contours.SetInputConnection(vtkToUnity.triangleFilter.GetOutputPort());
		contours.SetInputArrayToProcess(0,0,0, (int)Kitware.VTK.vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, "Elevation");
		for(int i = 0; i < 10; ++i)
			contours.SetValue(i, i / 10.0);
		contours.ComputeScalarsOn();
		VtkToUnity vtkToUnityContours = new VtkToUnity(contours.GetOutputPort(), "Contours");
		vtkToUnityContours.ColorBy("Elevation", VtkToUnity.VtkColorType.POINT_DATA);
		vtkToUnityContours.SetLut(VtkToUnity.LutPreset.BLUE_RED);
		//vtkToUnityContours.ColorBy(Color.red);
		vtkToUnityContours.Update();
		vtkToUnityContours.go.transform.Translate(-4f, 0f, 0f);
		
		// Points
		filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Vtk-Data/Points.vtp");
		if (reader.CanReadFile(filepath) == 0)
		{
			Debug.Log(filepath + " could not be loaded by Vtk!");
			return;
		}
		reader.SetFileName(filepath);
		reader.Update();
		
		VtkToUnity vtkToUnityPoints = new VtkToUnity(reader.GetOutputPort(), "Vtk-Data/Points.vtp");
		vtkToUnityPoints.ColorBy("Elevation", VtkToUnity.VtkColorType.POINT_DATA);
		vtkToUnityPoints.SetLut(VtkToUnity.LutPreset.RED_BLUE);
		//vtkToUnityPoints.ColorBy(Color.red);
		vtkToUnityPoints.Update();
		vtkToUnityPoints.go.transform.Translate(2f, 0f, 0f);
	}
}