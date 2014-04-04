using UnityEngine;
using System.Collections;
using System.Reflection;

[ExecuteInEditMode]
public class ControllerGameObject : MonoBehaviour 
{
	[HideInInspector]
	public enum mode
	{
		None,
		Menu,
		Parameter,
		Index
	}

	[HideInInspector]
	public enum menu
	{
		None,
		Filter,
		Properties
	}

	[HideInInspector]
	public GameObject flyStick;
	[HideInInspector]
	public VTKRoot root;
	[HideInInspector]
	public VTKNode node;
	[HideInInspector]
	public VTKFilter filter;
	[HideInInspector]
	public ListOfPlaymodeParameter playmodeParameters;
	[HideInInspector]
	public VTKProperties properties;

	public PlaymodeParameter playmodeParameter;
	[HideInInspector]
	public string pressedKey;

	public mode activeMode = mode.None;
	public menu activeMenu = menu.None;
	public int activeParameter = 0;
	public int activeIndex = 0;

	public void Start()
	{
		Initialize ();
	}

	public void Initialize ()
	{
		this.flyStick = GameObject.Find ("FlyStick");
		this.root = gameObject.transform.parent.gameObject.GetComponent<VTKRoot> ();
		
		string filterName = gameObject.name.Remove (0, gameObject.name.LastIndexOf (",") + 1);		
		this.node = root.root.GetNode(filterName);
		
		this.filter = node.filter;
		this.properties = node.properties;
		
		activeMode = mode.None;
		activeMenu = menu.None;
		activeParameter = 0;
		activeIndex = 0;
		playmodeParameter = null;
	}

	public void Update()
	{
		if(flyStick != null)
		{
			if(flyStick.GetComponent<ControllerFlyStick>().go == gameObject)
			{
				//Check for input
				if(Input.inputString != "")
				{
					pressedKey = Input.inputString;
				}

				//Select mode
				if(pressedKey == "a" || pressedKey == "b" || pressedKey == "c")
				{
					SelectMode();
					pressedKey = "";
					return;
				}

				//Select menu / parameter / index
				if(activeMode != mode.None && 
				   (pressedKey == "l" || pressedKey == "r"))
				{
					switch(activeMode)
					{
					case mode.Menu:
						SelectMenu();
						break;
					case mode.Parameter:
						SelectParameter();
						break;
					case mode.Index:
						SelectIndex();
						break;
					}

					pressedKey = "";
					return;
				}

				//Change value
				if(activeMode != mode.None && 
				   activeMenu != menu.None && 
				   playmodeParameter != null && 
				   (pressedKey == "u" || pressedKey == "d"))
				{
					switch(pressedKey)
					{
					case "u":
						switch(playmodeParameter.type)
						{
						case "int":
							IncreaseInt();
							break;
						case "Vector2":
							IncreaseVector2();
							break;
						case "Vector3":
							IncreaseVector3();
							break;
						}
						break;
					case "d":
						switch(playmodeParameter.type)
						{
						case "int":
							DecreaseInt();
							break;
						case "Vector2":
							DecreaseVector2();
							break;
						case "Vector3":
							DecreaseVector3();
							break;
						}
						break;
					}

					pressedKey = "";
					return;
				}
			}
			else
			{
				activeMode = mode.None;
				activeMenu = menu.None;
				activeParameter = 0;
				activeIndex = 0;
				playmodeParameter = null;
			}
		}
	}

	public void SelectMode()
	{
		switch(pressedKey)
		{
		case "a":
			activeMode = mode.Menu;
			playmodeParameter = null;
			activeParameter = 0;
			activeIndex = 0;
			break;
		case "b":
			if(activeMenu != menu.None)
			{
				activeMode = mode.Parameter;
				activeParameter = 0;
				playmodeParameter = playmodeParameters.Get(activeParameter);
				activeIndex = 0;
			}
			break;
		case "c":
			if(playmodeParameter.type == "Vector2" || playmodeParameter.type == "Vector3")
			{
				activeMode = mode.Index;
			}
			break;
		}
	}
	
	public void SelectMenu()
	{
		switch(pressedKey)
		{
		case "l":
			playmodeParameters = filter.playmodeParameters;
			activeMenu = menu.Filter;
			break;
		case "r":
			playmodeParameters = properties.playmodeParameters;
			activeMenu = menu.Properties;
			break;
		}
	}
	
	public void SelectParameter()
	{
		switch(pressedKey)
		{
		case "l":
			if(activeParameter > 0)
			{
				activeParameter -= 1;
			}
			break;
		case "r":
			if((activeParameter + 1 ) < playmodeParameters.Count())
			{
				activeParameter += 1;
			}
			break;
		}

		playmodeParameter = playmodeParameters.Get(activeParameter);
	}

	public void SelectIndex()
	{
		switch(pressedKey)
		{
		case "l":
			if(activeIndex > 0)
			{
				activeIndex -= 1;
			}
			break;
		case "r":
			if(playmodeParameter.type == "Vector2")
			{
				if((activeIndex + 1) < 2)
				{
					activeIndex += 1;
				}
			}

			if(playmodeParameter.type == "Vector3")
			{
				if((activeIndex + 1) < 3)
				{
					activeIndex += 1;
				}
			}
			break;
		}
	}

	public FieldInfo GetFieldInfo()
	{
		FieldInfo fi = null;

		if(activeMenu == menu.Filter)
		{
			fi = filter.GetType().GetField(playmodeParameter.name);
		}

		if(activeMenu == menu.Properties)
		{
			fi = properties.GetType().GetField(playmodeParameter.name);
		}

		return fi;
	}

	public void IncreaseInt ()
	{
		pressedKey = "";
		FieldInfo fi = GetFieldInfo ();

		if(activeMenu == menu.Filter)
		{
			fi.SetValue(filter, (int)fi.GetValue(filter) + (int)playmodeParameter.range);
			node.filter.UpdateInput ();
		}

		if(activeMenu == menu.Properties)
		{
			fi.SetValue(properties, (int)fi.GetValue(properties) + (int)playmodeParameter.range);
			node.properties.UpdateInput ();
		}
	}

	public void DecreaseInt ()
	{
		pressedKey = "";
		FieldInfo fi = GetFieldInfo ();

		if(activeMenu == menu.Filter)
		{
			fi.SetValue(filter, (int)fi.GetValue(filter) - (int)playmodeParameter.range);
			node.filter.UpdateInput ();
		}
		
		if(activeMenu == menu.Properties)
		{
			fi.SetValue(properties, (int)fi.GetValue(properties) - (int)playmodeParameter.range);
			node.properties.UpdateInput ();
		}
	}

	public void IncreaseVector2 ()
	{
		pressedKey = "";
		FieldInfo fi = GetFieldInfo ();

		if(activeMenu == menu.Filter)
		{
			Vector2 vector = (Vector2)fi.GetValue (filter);
			vector [activeIndex] += playmodeParameter.range;
			fi.SetValue(filter, vector);
			node.filter.UpdateInput ();
		}

		if(activeMenu == menu.Properties)
		{
			Vector2 vector = (Vector2)fi.GetValue (properties);
			vector [activeIndex] += playmodeParameter.range;
			fi.SetValue(properties, vector);
			node.properties.UpdateInput ();
		}
	}

	public void DecreaseVector2 ()
	{
		pressedKey = "";
		FieldInfo fi = GetFieldInfo ();
		
		if(activeMenu == menu.Filter)
		{
			Vector2 vector = (Vector2)fi.GetValue (filter);
			vector [activeIndex] -= playmodeParameter.range;
			fi.SetValue(filter, vector);
			node.filter.UpdateInput ();
		}
		
		if(activeMenu == menu.Properties)
		{
			Vector2 vector = (Vector2)fi.GetValue (properties);
			vector [activeIndex] -= playmodeParameter.range;
			fi.SetValue(properties, vector);
			node.properties.UpdateInput ();
		}
	}

	public void IncreaseVector3 ()
	{
		pressedKey = "";
		FieldInfo fi = GetFieldInfo ();
		
		if(activeMenu == menu.Filter)
		{
			Vector3 vector = (Vector3)fi.GetValue (filter);
			vector [activeIndex] += playmodeParameter.range;
			fi.SetValue(filter, vector);
			node.filter.UpdateInput ();
		}
		
		if(activeMenu == menu.Properties)
		{
			Vector3 vector = (Vector3)fi.GetValue (properties);
			vector [activeIndex] += playmodeParameter.range;
			fi.SetValue(properties, vector);
			node.properties.UpdateInput ();
		}
	}
	
	public void DecreaseVector3 ()
	{
		pressedKey = "";
		FieldInfo fi = GetFieldInfo ();
		
		if(activeMenu == menu.Filter)
		{
			Vector3 vector = (Vector3)fi.GetValue (filter);
			vector [activeIndex] -= playmodeParameter.range;
			fi.SetValue(filter, vector);
			node.filter.UpdateInput ();
		}
		
		if(activeMenu == menu.Properties)
		{
			Vector3 vector = (Vector3)fi.GetValue (properties);
			vector [activeIndex] -= playmodeParameter.range;
			fi.SetValue(properties, vector);
			node.properties.UpdateInput ();
		}
	}
}
