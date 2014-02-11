using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class VTKObjectManager : MonoBehaviour {

	[HideInInspector]
	public string filepath;
	[HideInInspector]
	public string filename;

	[HideInInspector]
	public Vector3 startPosition;

	[HideInInspector]
	public Kitware.VTK.vtkXMLPolyDataReader polyDataReader;

	[HideInInspector]
	public Kitware.VTK.vtkAlgorithmOutput output;

	[HideInInspector]
	public VtkToUnity vtkToUnity;

	[HideInInspector]
	public string[] pointArrays;
	[HideInInspector]
	public string[] cellArrays;
	[HideInInspector]
	public int selectedPointArray;
	[HideInInspector]
	public int selectedCellArray;
	[HideInInspector]
	public string[] typesOfColor = {"solid color", "point data", "cell data"};
	[HideInInspector]
	public int selectedColorType;

	[HideInInspector]
	public string[] allFilters;
	[HideInInspector]
	public int selectedFilter;
	[HideInInspector]
	public List<VTKFilter> filters;

	[HideInInspector]
	public bool selectedFileIsValid = false;

	public void OnEnable()
	{
		if (selectedFileIsValid) 
		{
			Initialize ();
			ComputeFilters();
		}
	}

	public void Initialize()
	{
		Debug.Log (gameObject.name + ": initialize");

		filename = gameObject.name;

		polyDataReader = Kitware.VTK.vtkXMLPolyDataReader.New ();
		polyDataReader.SetFileName(filepath);
		polyDataReader.Update();

		allFilters = VTKFilters.GetFiltersByName ();
		
		output = Kitware.VTK.vtkAlgorithmOutput.New ();
		output = polyDataReader.GetOutputPort ();

		vtkToUnity = new VtkToUnity(output, gameObject);
		vtkToUnity.ColorBy (Color.red);
		vtkToUnity.Update ();

		ReadVTKData ();
	}

	public void ReadVTKData()
	{
		Debug.Log (gameObject.name + ": read VTKData");
		
		//Get point and cell arrays for dropdown menu
		pointArrays = new string[polyDataReader.GetNumberOfPointArrays()];

		for (int i = 0; i < polyDataReader.GetNumberOfPointArrays(); i++) 
		{
			pointArrays[i] = polyDataReader.GetPointArrayName(i);
		}

		cellArrays = new string[polyDataReader.GetNumberOfCellArrays()];
		
		for (int i = 0; i < polyDataReader.GetNumberOfCellArrays(); i++) 
		{
			cellArrays[i] = polyDataReader.GetCellArrayName(i);
		}
	}
	
	public void AddFilter(string filterName)
	{
		Debug.Log (gameObject.name + ": add " + filterName);

		filters.Add ( (VTKFilter)gameObject.AddComponent(filterName));
	}
	
	public void RemoveFilter(VTKFilter f)
	{
		Debug.Log (gameObject.name + ": remove " + f.GetType().ToString());

		filters.Remove (f);
		DestroyImmediate (f);

		ComputeFilters ();
	}
	
	public void ComputeFilters()
	{
		Debug.Log (gameObject.name + ": compute filter");

		gameObject.transform.position = startPosition;

		output = polyDataReader.GetOutputPort ();

		foreach (VTKFilter f in filters) 
		{
			output = f.ApplyFilter(output);
		}

		if (selectedColorType == 0) //solid color
		{
			vtkToUnity.ColorBy (Color.red);
		} 
		else if (selectedColorType == 1) //point data
		{
			vtkToUnity.ColorBy (pointArrays[selectedPointArray], VtkToUnity.VtkColorType.POINT_DATA);
		} 
		else if (selectedColorType == 2) //cell data
		{
			vtkToUnity.ColorBy (cellArrays[selectedCellArray], VtkToUnity.VtkColorType.CELL_DATA);
		}

		//TODO make me changeable -> editor
		vtkToUnity.SetLut (VtkToUnity.LutPreset.RED_BLUE);
		vtkToUnity.triangleFilter.SetInputConnection(output);

		vtkToUnity.Update();

		//TODO dont use a fixed position
		vtkToUnity.go.transform.Translate(0f, 0f, 0f);
	}
}
