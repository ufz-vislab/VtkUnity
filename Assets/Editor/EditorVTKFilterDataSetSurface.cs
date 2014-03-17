using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(VTKFilterDataSetSurface))]
public class EditorVTKFilterDataSetSurface : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		EditorUtility.SetDirty (target);
	}
}
