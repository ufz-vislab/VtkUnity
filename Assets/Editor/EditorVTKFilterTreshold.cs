using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VTKFilterThreshold))]
public class EditorVTKFilterTreshold : EditorVTKFilter
{
	public override void Content()
	{
		script = (VTKFilterThreshold)target;

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Data array:");
		((VTKFilterThreshold)script).selectedDataArray = EditorGUILayout.Popup (((VTKFilterThreshold)script).selectedDataArray, script.gameObject.GetComponent<VTKProperties>().dataArrays);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Range min:");
		((VTKFilterThreshold)script).range[0] = EditorGUILayout.FloatField(((VTKFilterThreshold)script).range[0]);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Range max:");
		((VTKFilterThreshold)script).range[1] = EditorGUILayout.FloatField(((VTKFilterThreshold)script).range[1]);
		EditorGUILayout.EndHorizontal ();
	}
}
