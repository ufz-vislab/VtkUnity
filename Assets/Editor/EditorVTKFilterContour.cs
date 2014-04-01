using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VTKFilterContour))]
public class EditorVTKFilterContour : EditorVTKFilter 
{
	public override void Content()
	{
		script = (VTKFilterContour)target;

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Data array:");
		((VTKFilterContour)script).selectedDataArray = EditorGUILayout.Popup (((VTKFilterContour)script).selectedDataArray, script.gameObject.GetComponent<VTKProperties>().dataArrays);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Contours:");
		((VTKFilterContour)script).numContours = EditorGUILayout.IntField (((VTKFilterContour)script).numContours);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Range min:");
		((VTKFilterContour)script).range[0] = EditorGUILayout.FloatField(((VTKFilterContour)script).range[0]);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Range max:");
		((VTKFilterContour)script).range[1] = EditorGUILayout.FloatField(((VTKFilterContour)script).range[1]);
		EditorGUILayout.EndHorizontal ();
	}
}
