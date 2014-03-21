using UnityEngine;
using System.Collections;
using System.Reflection;

[ExecuteInEditMode]
public class Controller : MonoBehaviour {

	public int selected;

	// Use this for initialization
	void Start () 
	{
		selected = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (!gameObject.activeInHierarchy)
						return;

		if(Input.GetKeyDown("up"))
		{
			VTKFilterContour filter = (VTKFilterContour)gameObject.GetComponent<VTKRoot>().activeNode.filter;

			//Gets the kind of variable
			FieldInfo fi = filter.GetType().GetField(filter.useInPlaymode[0]);

			//Set the kind of value u get before in the instance of filter
			fi.SetValue(filter, 8);

			gameObject.GetComponent<VTKRoot>().Modifie(filter.node);
		}
	}
}
