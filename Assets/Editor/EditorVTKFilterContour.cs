using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKFilterContour))]
public class EditorVTKFilterContour : Editor 
{
	public override void OnInspectorGUI()
	{
		VTKFilterContour script = (VTKFilterContour)target;

		VTKProperties properties = script.gameObject.GetComponent<VTKProperties> ();

		DrawDefaultInspector();

		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Number of contours:");
		script.numContours = EditorGUILayout.IntField (script.numContours);
		EditorGUILayout.EndHorizontal ();

		if(EditorGUI.EndChangeCheck())
		{
			VTKRoot root = script.gameObject.GetComponent<VTKRoot>();
			root.Modifie(root.activeNode);
		}

		EditorUtility.SetDirty (target);
	}
}
