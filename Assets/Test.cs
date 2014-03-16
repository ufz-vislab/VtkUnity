using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Test : MonoBehaviour 
{
	public void Start()
	{
		Debug.Log ("Start");
	}

	public void PropertieTest()
	{
		string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Vtk-Data/Box.vtp"); 

		Kitware.VTK.vtkXMLPolyDataReader polyDataReader = Kitware.VTK.vtkXMLPolyDataReader.New ();

		polyDataReader.SetFileName (filepath);

		//Point data
		string[] pointArrays = new string[polyDataReader.GetNumberOfPointArrays()];
		
		for (int i = 0; i < polyDataReader.GetNumberOfPointArrays(); i++) 
		{
			pointArrays[i] = polyDataReader.GetPointArrayName(i);
		}
		
		//Cell data
		string[] cellArrays = new string[polyDataReader.GetNumberOfCellArrays()];
		
		for (int i = 0; i < polyDataReader.GetNumberOfCellArrays(); i++) 
		{
			cellArrays[i] = polyDataReader.GetCellArrayName(i);
		}
	}

	public void vtuTest()
	{
		string filepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Vtk-Data/urach_borehole_kn_statistics_15.vtu"); 
		
		//reader
		Kitware.VTK.vtkXMLUnstructuredGridReader reader = Kitware.VTK.vtkXMLUnstructuredGridReader.New ();
		
		if (reader.CanReadFile(filepath) == 0)
		{
			Debug.Log(filepath + " could not be loaded by Vtk!");
			return;
		}
		
		reader.SetFileName(filepath);
		reader.Update(); 

		Debug.LogWarning ("before geo");
		Kitware.VTK.vtkGeometryFilter filter = Kitware.VTK.vtkGeometryFilter.New ();

		filter.SetInputConnection(reader.GetOutputPort());
		filter.Update ();
		Debug.LogWarning ("after geo");

		VtkToUnity vtu = new VtkToUnity(filter.GetOutputPort(), "Vtk-Data/result.vtu"); 
		vtu.ColorBy("DISPLACEMENT_Y1_minimum", VtkToUnity.VtkColorType.POINT_DATA);
		vtu.SetLut(VtkToUnity.LutPreset.RED_BLUE);
		//vtkToUnityPoints.ColorBy(Color.red);
		vtu.Update();
		vtu.go.transform.Translate(2f, 0f, 0f);
	}
}
