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

		PropertiesMenu ();

		EditorUtility.SetDirty (target);
	}

	public void PropertiesMenu()
	{
		EditorGUILayout.LabelField ("Properties:");
		
		EditorGUILayout.Separator ();
		
		EditorGUI.BeginChangeCheck ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Color Type:");
		script.selectedColorType = EditorGUILayout.Popup(script.selectedColorType, script.typesOfColor);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Data:");
		script.selectedDataArray = EditorGUILayout.Popup(script.selectedDataArray, script.dataArray);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Lut:");
		script.selectedLut = EditorGUILayout.Popup (script.selectedLut, script.Lut);
		EditorGUILayout.EndHorizontal ();

		if (EditorGUI.EndChangeCheck ()) 
		{
			VTKRoot root = script.gameObject.GetComponent<VTKRoot>();
			root.UpdateProperties(root.activeNode);
		}
	}
}
