using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKFilterRotationalExtrusion))]
public class EditorVTKFilterRotationalExtrusion : EditorVTKFilter 
{
	public override void Content()
	{
		script = (VTKFilterRotationalExtrusion)target;
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Resolution:");
		((VTKFilterRotationalExtrusion)script).resolution = EditorGUILayout.IntField(((VTKFilterRotationalExtrusion)script).resolution);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Capping:");
		((VTKFilterRotationalExtrusion)script).capping = EditorGUILayout.Toggle(((VTKFilterRotationalExtrusion)script).capping);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Angle:");
		((VTKFilterRotationalExtrusion)script).angle = EditorGUILayout.FloatField(((VTKFilterRotationalExtrusion)script).angle);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Translation:");
		((VTKFilterRotationalExtrusion)script).translation = EditorGUILayout.FloatField(((VTKFilterRotationalExtrusion)script).translation);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Delta Radius:");
		((VTKFilterRotationalExtrusion)script).deltaRadius = EditorGUILayout.FloatField(((VTKFilterRotationalExtrusion)script).deltaRadius);
		EditorGUILayout.EndHorizontal ();		
	}
}
