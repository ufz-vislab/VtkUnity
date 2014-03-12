using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKFilterTransform))]
public class EditorVTKFilterTransform : Editor 
{
	public override void OnInspectorGUI()
	{
		VTKFilterTransform script = (VTKFilterTransform)target;

		DrawDefaultInspector ();

		EditorGUI.BeginChangeCheck ();

		//Translate
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Translate:");
		script.translateX = EditorGUILayout.FloatField (script.translateX);
		script.translateY = EditorGUILayout.FloatField (script.translateY);
		script.translateZ = EditorGUILayout.FloatField (script.translateZ);

		EditorGUILayout.EndHorizontal ();

		//Rotate
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Rotate:");
		script.rotateX = EditorGUILayout.FloatField (script.rotateX);
		script.rotateY = EditorGUILayout.FloatField (script.rotateY);
		script.rotateZ = EditorGUILayout.FloatField (script.rotateZ);

		EditorGUILayout.EndHorizontal ();

		//Scale
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Scale:");
		script.scaleX = EditorGUILayout.FloatField (script.scaleX);
		script.scaleY = EditorGUILayout.FloatField (script.scaleY);
		script.scaleZ = EditorGUILayout.FloatField (script.scaleZ);

		EditorGUILayout.EndHorizontal ();

		if (EditorGUI.EndChangeCheck ()) 
		{
			VTKObjectRoot root = script.gameObject.GetComponent<VTKObjectRoot>();
			root.Modifie(root.activeNode);
		}

		EditorUtility.SetDirty (target);
	}
}
