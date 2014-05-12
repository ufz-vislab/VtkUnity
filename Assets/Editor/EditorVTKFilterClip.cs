using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VTKFilterClip))]
public class EditorVTKFilterClip : EditorVTKFilter 
{
	public override void Content ()
	{
		script = (VTKFilterClip)target;

		//Origin
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Origin:");
		((VTKFilterClip)script).originX = EditorGUILayout.FloatField (((VTKFilterClip)script).originX);
		((VTKFilterClip)script).originY = EditorGUILayout.FloatField (((VTKFilterClip)script).originY);
		((VTKFilterClip)script).originZ = EditorGUILayout.FloatField (((VTKFilterClip)script).originZ);
		EditorGUILayout.EndHorizontal ();

		//Normal
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Normal:");
		((VTKFilterClip)script).normalX = EditorGUILayout.FloatField (((VTKFilterClip)script).normalX);
		((VTKFilterClip)script).normalY = EditorGUILayout.FloatField (((VTKFilterClip)script).normalY);
		((VTKFilterClip)script).normalZ = EditorGUILayout.FloatField (((VTKFilterClip)script).normalZ);
		EditorGUILayout.EndHorizontal ();
	}
}
