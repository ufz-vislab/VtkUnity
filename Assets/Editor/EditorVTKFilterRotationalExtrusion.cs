using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKFilterRotationalExtrusion))]
public class EditorVTKFilterRotationalExtrusion : Editor 
{
	public override void OnInspectorGUI()
	{
		VTKFilterRotationalExtrusion script = (VTKFilterRotationalExtrusion)target;

		DrawDefaultInspector ();

//		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Resolution:");
		script.resolution = EditorGUILayout.IntField(script.resolution);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Capping:");
		script.capping = EditorGUILayout.Toggle(script.capping);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Angle:");
		script.angle = EditorGUILayout.FloatField(script.angle);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Translation:");
		script.translation = EditorGUILayout.FloatField(script.translation);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Delta Radius:");
		script.deltaRadius = EditorGUILayout.FloatField(script.deltaRadius);
		EditorGUILayout.EndHorizontal ();

		/*
		if (EditorGUI.EndChangeCheck ()) 
		{
			VTKRoot root = script.gameObject.GetComponent<VTKRoot>();
			root.Modifie(root.activeNode);
		}
*/
		EditorUtility.SetDirty (target);
	}
}
