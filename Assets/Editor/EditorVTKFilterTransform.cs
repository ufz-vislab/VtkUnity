using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKFilterTransform))]
public class EditorVTKFilterTransform : EditorVTKFilter
{
	public override void Content()
	{
		script = (VTKFilterTransform)target;

		//Translate
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Translate:");
		((VTKFilterTransform)script).translateX = EditorGUILayout.FloatField (((VTKFilterTransform)script).translateX);
		((VTKFilterTransform)script).translateY = EditorGUILayout.FloatField (((VTKFilterTransform)script).translateY);
		((VTKFilterTransform)script).translateZ = EditorGUILayout.FloatField (((VTKFilterTransform)script).translateZ);
		EditorGUILayout.EndHorizontal ();

		//Rotate
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Rotate:");
		((VTKFilterTransform)script).rotateX = EditorGUILayout.FloatField (((VTKFilterTransform)script).rotateX);
		((VTKFilterTransform)script).rotateY = EditorGUILayout.FloatField (((VTKFilterTransform)script).rotateY);
		((VTKFilterTransform)script).rotateZ = EditorGUILayout.FloatField (((VTKFilterTransform)script).rotateZ);
		EditorGUILayout.EndHorizontal ();

		//Scale
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Scale:");
		((VTKFilterTransform)script).scaleX = EditorGUILayout.FloatField (((VTKFilterTransform)script).scaleX);
		((VTKFilterTransform)script).scaleY = EditorGUILayout.FloatField (((VTKFilterTransform)script).scaleY);
		((VTKFilterTransform)script).scaleZ = EditorGUILayout.FloatField (((VTKFilterTransform)script).scaleZ);
		EditorGUILayout.EndHorizontal ();
	}
}
