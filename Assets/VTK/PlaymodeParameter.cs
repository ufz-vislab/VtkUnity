using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlaymodeParameter 
{
	public string name;
	public string type;
	public float range;

	public PlaymodeParameter(){}

	public PlaymodeParameter(string name, string type, float range)
	{
		this.name = name;
		this.type = type;
		this.range = range;
	}
}
