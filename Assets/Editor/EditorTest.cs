using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Test))]
public class EditorTest : Editor 
{
	public override void OnInspectorGUI()
	{
		Test script = (Test)target;
		
		DrawDefaultInspector();
		
		if (GUILayout.Button ("Do Shit")) 
		{
			script.PropertieTest();
		}
		
		EditorUtility.SetDirty (target);
	}

}
