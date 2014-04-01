using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ControllerFlyStick : MonoBehaviour 
{
	public GameObject go = null;
	
	public void OnTriggerEnter(Collider col)
	{
		this.go = col.gameObject;
	}
	
	public void OnTriggerExit()
	{
		this.go = null;
	}
}
