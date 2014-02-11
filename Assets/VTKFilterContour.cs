using UnityEngine;
using System.Collections;

public class VTKFilterContour : MonoBehaviour {

	public void ApplyFilter()
	{
		/*
		VtkToUnity vtkToUnity = gameObject.GetComponent<VTPOptions>().vtkToUnity;

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
		*/
	}
}
