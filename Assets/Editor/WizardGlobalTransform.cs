using UnityEngine;
using UnityEditor;
using System.Collections;

public class WizardGlobalTransform : ScriptableWizard
{
	public float scale = 0.01f;
	float oldScale = 0.01f;
	public string transformName = "Global Transform";

	GameObject[] gos = null;
	Bounds bounds;
	bool transformSelected = false;

	[MenuItem ("UFZ/Global Transformation")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<WizardGlobalTransform>
			("Apply global transformation", "Apply Transform!").Calculate();
	}

	void Calculate()
	{
		gos = Selection.gameObjects;
		if(gos.Length == 0)
		{
			isValid = false;
			errorString = "No GameObjects selected in the Hierarchy Window!";
		}
		else if(gos.Length == 1 && gos[0].name == transformName)
		{
			transformSelected = true;
			scale = gos[0].transform.localScale.x;
			oldScale = scale;
			OnWizardUpdate();
			return;
		}

		bool boundsInited = false;
		foreach(GameObject go in gos)
		{
			Renderer[] goRenderers = go.GetComponentsInChildren<Renderer>();
			foreach(Renderer renderer in goRenderers)
			{
				if(!boundsInited)
				{
					bounds = new Bounds(renderer.bounds.center, renderer.bounds.size);
					boundsInited = true;
				}
				bounds.Encapsulate(renderer.bounds.min);
				bounds.Encapsulate(renderer.bounds.max);
			}
		}
	}

	void OnWizardCreate()
	{
		if(transformSelected)
		{
			if(scale != oldScale)
			{
				Debug.Log("Rescaled global transform.");
				float factor = oldScale / scale;
				gos[0].transform.position = gos[0].transform.position / factor;
				gos[0].transform.localScale = new Vector3(scale, scale, scale);
			}
		}
		else
		{
			Debug.Log("Bounds center: " + bounds.center);
			GameObject transformGo = new GameObject(transformName);
			foreach(GameObject go in gos)
				go.transform.parent = transformGo.transform;

			transformGo.transform.position = -bounds.center * scale;
			transformGo.transform.localScale = new Vector3(scale, scale, scale);
		}
	}

	void OnWizardUpdate()
	{
		if(transformSelected)
			helpString = "Global Transform will be rescaled.";
		else
			helpString = "All selected objects will be appended to a transformation\n GameObject named "
				+ transformName + " with a scaling of " + scale + ".";
	}
}
