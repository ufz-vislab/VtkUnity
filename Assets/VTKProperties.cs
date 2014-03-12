using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VTKProperties : MonoBehaviour 
{
	[HideInInspector]
	public string[] pointArrays;

	[HideInInspector]
	public string[] cellArrays;

	[HideInInspector]
	public int selectedPointArray;

	[HideInInspector]
	public int selectedCellArray;

	[HideInInspector]
	public string[] Lut = {"BLUE_RED", "RED_BLUE", "RAINBOW"};

	[HideInInspector]
	public int selectedLut;

	[HideInInspector]
	public string[] typesOfColor = {"solid color", "point data", "cell data"};

	[HideInInspector]
	public int selectedColorType;

	public void ReadProperties()
	{
		VTKObjectRoot root = gameObject.GetComponent<VTKObjectRoot> ();
		
		Kitware.VTK.vtkXMLPolyDataReader polyDataReader = root.polyDataReader;
		
		//Point data
		pointArrays = new string[polyDataReader.GetNumberOfPointArrays()];
		
		for (int i = 0; i < polyDataReader.GetNumberOfPointArrays(); i++) 
		{
			pointArrays[i] = polyDataReader.GetPointArrayName(i);
		}
		
		//Cell data
		cellArrays = new string[polyDataReader.GetNumberOfCellArrays()];
		
		for (int i = 0; i < polyDataReader.GetNumberOfCellArrays(); i++) 
		{
			cellArrays[i] = polyDataReader.GetCellArrayName(i);
		}
	}
}
