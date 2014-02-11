using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKObjectManager))]
public class EditorVTKObjectManager : Editor 
{
	public VTKObjectManager script;

	public override void OnInspectorGUI()
	{
		script = (VTKObjectManager)target;
		
		DrawDefaultInspector ();

		SelectFileMenu ();

		EditorGUILayout.Separator ();

		if (script.selectedFileIsValid) 
		{
			DataArrayMenu();
			
			EditorGUILayout.Separator ();

			FilterMenu();
		}

		EditorUtility.SetDirty (target);
	}

	public void SelectFileMenu()
	{
		EditorGUILayout.BeginHorizontal ();
		script.filepath = EditorGUILayout.TextField ("File:", script.filepath);
		
		if(GUILayout.Button("Select File"))
		{
			script.filepath = EditorUtility.OpenFilePanel("Select File:", 
			                                              System.IO.Path.Combine(Application.streamingAssetsPath, "Vtk-Data/"), "*");
			
			if(script.filepath.EndsWith(".vtp") || script.filepath.EndsWith(".vtu")) 
			{
				script.selectedFileIsValid = true;
				script.Initialize();
				script.startPosition = script.gameObject.transform.position;
			}
			else
			{
				script.selectedFileIsValid = false;
				Debug.LogWarning("Select .vtp or .vtu file");
			}
			
		}
		EditorGUILayout.EndHorizontal ();
	}

	public void DataArrayMenu()
	{
		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Color Type:");
		script.selectedColorType = EditorGUILayout.Popup(script.selectedColorType, script.typesOfColor);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Point Data:");
		script.selectedPointArray = EditorGUILayout.Popup(script.selectedPointArray, script.pointArrays);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Cell Data:");
		script.selectedCellArray = EditorGUILayout.Popup(script.selectedCellArray, script.cellArrays);
		EditorGUILayout.EndHorizontal ();

		if (EditorGUI.EndChangeCheck ()) 
		{
			script.ComputeFilters();
		}
	}

	public void FilterMenu()
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Filter:");
		script.selectedFilter = EditorGUILayout.Popup (script.selectedFilter, script.allFilters);
		
		if (GUILayout.Button ("Add")) {
			script.AddFilter (script.allFilters[script.selectedFilter]);
		}
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.Separator ();
		
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Filter:");
		for (int i=0; i < script.filters.Count; i++) 
		{
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (script.filters [i].GetType ().ToString ());
			if (GUILayout.Button ("Remove")) 
			{
				script.RemoveFilter (script.filters [i]);
			}
			EditorGUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndVertical ();
	}
}
