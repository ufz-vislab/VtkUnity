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
		/*
		EditorGUILayout.LabelField ("DataSet: " + properties.pointArrays[properties.selectedPointArray]);

		script.DataSet = properties.pointArrays[properties.selectedPointArray];
		EditorGUILayout.EndHorizontal ();
		*/
		if(EditorGUI.EndChangeCheck())
		{
			VTKRoot root = script.gameObject.GetComponent<VTKRoot>();
			root.Modifie(root.activeNode);
		}

		EditorUtility.SetDirty (target);
	}
}
