using UnityEngine;
using System.Collections;

public class TestVTUFileReader : MonoBehaviour
{

	void Start ()
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

		VtkToUnity vtu = new VtkToUnity(reader.GetOutputPort(), "Vtk-Data/urach_borehole_kn_statistics_15.vtu"); 
		vtu.ColorBy("Elevation", VtkToUnity.VtkColorType.POINT_DATA);
		vtu.SetLut(VtkToUnity.LutPreset.RED_BLUE);
		//vtkToUnityPoints.ColorBy(Color.red);
		vtu.Update();
		vtu.go.transform.Translate(2f, 0f, 0f);
	}
}