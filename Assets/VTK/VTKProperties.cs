using UnityEngine;
using System.Collections.Generic;
using Kitware.VTK;

[ExecuteInEditMode]
public class VTKProperties : MonoBehaviour 
{
	[HideInInspector]
	public VTKNode node;
	[HideInInspector]
	public string[] dataArrays;
	[HideInInspector]
	public int selectedDataArray;
	[HideInInspector]
	public string[] Lut = {"BLUE_RED", "RED_BLUE", "RAINBOW"};
	[HideInInspector]
	public int selectedLut;
	[HideInInspector]
	public string[] colorTypes = {"solid color", "data"};
	[HideInInspector]
	public int selectedColorType;
	[HideInInspector]
	public ListOfPlaymodeParameter playmodeParameters;


	public void SetPlaymodeParameters()
	{
		this.playmodeParameters = new ListOfPlaymodeParameter ();
		this.playmodeParameters.Add (new PlaymodeParameter("selectedDataArray", "int", 1.0f));
		this.playmodeParameters.Add (new PlaymodeParameter("selectedLut", "int", 1.0f));
		this.playmodeParameters.Add (new PlaymodeParameter("selectedColorType", "int", 1.0f));
	}

	public void Read()
	{
		VTKRoot root = gameObject.GetComponent<VTKRoot> ();

		VTK.DataType dataType = root.dataType;

		vtkXMLPolyDataReader polyDataReader;

		vtkXMLUnstructuredGridReader unstructuredGridReader;

		if( dataType == VTK.DataType.PolyData )
		{
			polyDataReader = root.polyDataReader;

			dataArrays = new string[polyDataReader.GetNumberOfPointArrays() 
			+ polyDataReader.GetNumberOfCellArrays()];

			//Cell data
			for (int i = 0; i < polyDataReader.GetNumberOfCellArrays(); i++) 
			{
				dataArrays[i] = polyDataReader.GetCellArrayName(i) + " [C]";
			}

			//Point data
			for (int i = 0; i < polyDataReader.GetNumberOfPointArrays(); i++) 
			{
				dataArrays[polyDataReader.GetNumberOfCellArrays() + i] = 
				polyDataReader.GetPointArrayName(i) + " [P]";
			}
		}

		if(dataType == VTK.DataType.UnstructuredGrid)
		{
			unstructuredGridReader = root.unstructuredGridReader;

			dataArrays = new string[unstructuredGridReader.GetNumberOfPointArrays() 
			+ unstructuredGridReader.GetNumberOfCellArrays()];

			//Cell data
			for (int i = 0; i < unstructuredGridReader.GetNumberOfCellArrays(); i++) 
			{
				dataArrays[i] = unstructuredGridReader.GetCellArrayName(i) + " [C]";
			}
			
			//Point data
			for (int i = 0; i < unstructuredGridReader.GetNumberOfPointArrays(); i++) 
			{
				dataArrays[unstructuredGridReader.GetNumberOfCellArrays() + i] = 
				unstructuredGridReader.GetPointArrayName(i) + " [P]";
			}
		}
	}

	public void ValidateInput()
	{
		if(selectedDataArray < 0)
			selectedDataArray = 0;
		if(selectedDataArray > dataArrays.Length - 1)
			selectedDataArray = dataArrays.Length - 1;

		if(selectedLut < 0)
			selectedLut = 0;
		if(selectedLut > Lut.Length - 1)
			selectedLut = Lut.Length - 1;

		if(selectedColorType < 0)
			selectedColorType = 0;
		if(selectedColorType > colorTypes.Length - 1)
			selectedColorType = colorTypes.Length - 1;
	}

	public void UpdateInput()
	{
		ValidateInput ();
		node.UpdateProperties ();
	}
}
