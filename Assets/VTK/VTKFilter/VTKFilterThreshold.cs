using UnityEngine;
using System.Collections;
using Kitware.VTK;

[ExecuteInEditMode]
public class VTKFilterThreshold : VTKFilter 
{
	[HideInInspector]
	public Vector2 range;
	[HideInInspector]
	public int selectedDataArray = 0;

	protected void Reset()
	{
		range = Vector2.zero;
		vtkFilter = vtkThreshold.New ();
		selectedDataArray = 0;
	}

	protected override void ValidateInput ()
	{
		//TODO
	}

	public override void SetPlaymodeParameters ()
	{
		playmodeParameters = new ListOfPlaymodeParameter ();
		playmodeParameters.Add(new PlaymodeParameter("range", "Vector2", 1.0f));
		playmodeParameters.Add (new PlaymodeParameter ("selectedDataArray", "int", 1.0f));
	}

	protected override void CalculateFilter ()
	{
		vtkFilter = vtkThreshold.New ();

		string dataArray = gameObject.GetComponent<VTKProperties>().dataArrays[selectedDataArray];
		dataArray = dataArray.Remove(dataArray.IndexOf("[") - 1);

		vtkFilter.SetInputConnection (node.parent.filter.vtkFilter.GetOutputPort());
		vtkFilter.SetInputArrayToProcess(0, 0, 0, 
			(int)vtkDataObject.FieldAssociations.FIELD_ASSOCIATION_POINTS, 
			dataArray);
		((vtkThreshold)vtkFilter).SetSelectedComponent (0);
		((vtkThreshold)vtkFilter).ThresholdBetween (range [0], range [1]);
		vtkFilter.Update ();
	}
}
