using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VTKFilterContour))]
public class EditorVTKFilterContour : Editor 
{
	public bool toggle = false;

	public override void OnInspectorGUI()
	{
		VTKFilterContour script = (VTKFilterContour)target;

		DrawDefaultInspector ();

		EditorGUI.BeginChangeCheck ();

		EditorGUILayout.BeginHorizontal ();
		script.numContours = EditorGUILayout.IntField (script.numContours);
		toggle = EditorGUILayout.Toggle (toggle);
			
		if(toggle)
		{
			//script.useInPlaymode.Add(script.numContours);
		}
		else
		{
			//if(script.useInPlaymode.Contains(script.numContours))
			//	script.useInPlaymode.Remove(script.numContours);
		}
		EditorGUILayout.EndHorizontal ();
				
		if (EditorGUI.EndChangeCheck ()) 
		{
			Debug.LogWarning("changed");
			VTKRoot root = script.gameObject.GetComponent<VTKRoot>();
			root.Modifie(root.activeNode);
		}

		EditorUtility.SetDirty (target);
	}
}
