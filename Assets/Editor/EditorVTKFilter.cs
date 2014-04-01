using UnityEngine;
using UnityEditor;

public abstract class EditorVTKFilter : Editor 
{
	[HideInInspector]
	public VTKFilter script;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		EditorGUI.BeginChangeCheck ();

		Content ();

		if(EditorGUI.EndChangeCheck())
		{
			script.UpdateInput();
		}

		EditorUtility.SetDirty (target);
	}

	public abstract void Content();
}
