using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorVTKFilterGeometry : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		EditorUtility.SetDirty (target);
	}
}
