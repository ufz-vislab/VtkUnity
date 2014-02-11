using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKFilterContour))]
public class EditorVTKFilterContour : Editor 
{
	public override void OnInspectorGUI()
	{
		VTKFilterContour script = (VTKFilterContour)target;

		DrawDefaultInspector();

		if (GUILayout.Button ("Apply")) 
		{
			script.ApplyFilter();
		}

		EditorUtility.SetDirty (target);
	}
}
