using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VTKFilterStreamlines))]
public class EditorVTKFilterStreamlines : EditorVTKFilter 
{
	public override void Content ()
	{
		script = (VTKFilterStreamlines)target;

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Data array:");
		((VTKFilterStreamlines)script).selectedDataArray = EditorGUILayout.Popup (((VTKFilterStreamlines)script).selectedDataArray, script.gameObject.GetComponent<VTKProperties>().dataArrays);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Number of points:");
		((VTKFilterStreamlines)script).numPoints = EditorGUILayout.IntField(((VTKFilterStreamlines)script).numPoints);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Radius:");
		((VTKFilterStreamlines)script).radius = EditorGUILayout.FloatField(((VTKFilterStreamlines)script).radius);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Center:");
		((VTKFilterStreamlines)script).center = EditorGUILayout.Vector3Field("", ((VTKFilterStreamlines)script).center, null);
		EditorGUILayout.EndHorizontal ();
	}
}
