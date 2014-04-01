using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKProperties))]
public class EditorVTKProperties : Editor 
{
	public VTKProperties script;
	
	public override void OnInspectorGUI()
	{
		script = (VTKProperties)target;

		DrawDefaultInspector ();

		Content ();

		EditorUtility.SetDirty (target);
	}

	public void Content()
	{
		EditorGUILayout.LabelField ("Properties:");
		
		EditorGUILayout.Separator ();
		
		EditorGUI.BeginChangeCheck ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Color Type:");
		script.selectedColorType = EditorGUILayout.Popup(script.selectedColorType, script.colorTypes);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Data:");
		script.selectedDataArray = EditorGUILayout.Popup(script.selectedDataArray, script.dataArrays);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Lut:");
		script.selectedLut = EditorGUILayout.Popup (script.selectedLut, script.Lut);
		EditorGUILayout.EndHorizontal ();

		if (EditorGUI.EndChangeCheck ()) 
		{
			script.UpdateInput();
		}
	}
}
