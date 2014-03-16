using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(VTKRoot))]
public class EditorVTKRoot : Editor 
{
	public GUIStyle treeNodeStyle; //Own style to have buttons as labels

	public VTKRoot script;
	
	public override void OnInspectorGUI()
	{
		treeNodeStyle = new GUIStyle(EditorStyles.label);

		script = (VTKRoot)target;
		
		DrawDefaultInspector ();

		SelectFileMenu ();

		EditorGUILayout.Separator ();

		if (script.selectedFileIsValid) 
		{
			FilterMenu();
		}

		EditorUtility.SetDirty (target);
	}

	/*
	 * Menu for selecting a vtk-file
	 * */
	public void SelectFileMenu()
	{
		EditorGUILayout.BeginHorizontal ();
		script.filepath = EditorGUILayout.TextField ("File:", script.filepath);
		
		if(GUILayout.Button("Select File"))
		{
			script.filepath = EditorUtility.OpenFilePanel("Select File:", 
			                                              System.IO.Path.Combine(Application.streamingAssetsPath, "Vtk-Data/"), "*");
			
			if(script.filepath.EndsWith(".vtp") || script.filepath.EndsWith(".vtu")) 
			{
				//Set data type
				if(script.filepath.EndsWith(".vtp"))
					script.readerType = VTK.FilterType.PolyData;

				if(script.filepath.EndsWith(".vtu"))
					script.readerType = VTK.FilterType.UnstructuredGrid;

				script.selectedFileIsValid = true;
				script.Initialize();
				script.startPosition = script.gameObject.transform.position;
			}
			else
			{
				script.selectedFileIsValid = false;
				Debug.LogWarning("Select .vtp or .vtu file");
			}
			
		}
		EditorGUILayout.EndHorizontal ();
	}

	/*
	 * Menu for vtk-filter tree
	 * */
	public void FilterMenu()
	{
		EditorGUILayout.LabelField ("Filters:");

		EditorGUILayout.Separator ();

		//Options for the active node
		EditorGUILayout.BeginHorizontal ();		
		EditorGUILayout.LabelField ("Selected node: " + script.activeNode.name);

		if (GUILayout.Button ("Remove")) 
		{
			script.RemoveNode (script.activeNode);
		}
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		script.selectedFilter = EditorGUILayout.Popup (script.selectedFilter, script.allFilters);
		
		if (GUILayout.Button ("Add")) 
		{
			script.AddFilter (script.allFilters[script.selectedFilter]);
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.Separator();

		//Tree
		CreateSubTree(script.root, 0);
	}

	/*
	 * Creates tree entries for a given node and its children
	 * */
	private void CreateSubTree(VTKNode n, int space)
	{		
		//Show the node

		//TODO der block kann als disposable implementiert werden

		//Change color of the node if it is the selected one
		if(script.activeNode == n)
			treeNodeStyle.normal.textColor = Color.green;

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Space (space);
		if (GUILayout.Button (n.name, treeNodeStyle))
		{
			script.SetActiveNode(n);
		}
		EditorGUILayout.EndHorizontal();

		//Reset color for other nodes
		if(script.activeNode == n)
			treeNodeStyle.normal.textColor = Color.black;

		//Show children
		if (n.hasChildren) 
		{
			for(int i = 0; i < n.children.Count; i++)
			{
				CreateSubTree(n.children[i], space + 20);
			}
		} 
	}
}