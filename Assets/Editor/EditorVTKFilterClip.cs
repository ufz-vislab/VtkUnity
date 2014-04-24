using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VTKFilterClip))]
public class EditorVTKFilterClip : EditorVTKFilter 
{
	public override void Content ()
	{
		script = (VTKFilterClip)target;
	}
}
