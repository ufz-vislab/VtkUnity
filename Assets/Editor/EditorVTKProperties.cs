using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VTKProperties))]
public class EditorVTKProperties : Editor 
{
	public VTKProperties script;
	
	public override void OnInspectorGUI()
	{
		script = (VTKProperties)target;
	
		script.ReadProperties ();

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
		EditorGUILayout.LabelField ("Point Data:");
		script.selectedPointArray = EditorGUILayout.Popup(script.selectedPointArray, script.pointArrays);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Cell Data:");
		script.selectedCellArray = EditorGUILayout.Popup(script.selectedCellArray, script.cellArrays);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Lut:");
		script.selectedLut = EditorGUILayout.Popup (script.selectedLut, script.Lut);
		EditorGUILayout.EndHorizontal ();

		if (EditorGUI.EndChangeCheck ()) 
		{
			VTKObjectRoot root = script.gameObject.GetComponent<VTKObjectRoot>();
			root.UpdateProperties(root.activeNode);
		}
	}
}
