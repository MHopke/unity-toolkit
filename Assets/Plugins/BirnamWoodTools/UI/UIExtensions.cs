﻿using UnityEngine;

/// <summary>
/// This class creates objects that will be scaled according the the designedSize.
/// Using these enables the use of constants instead of percentages of Screen height / width.
/// </summary>
public static class AutoSized
{
	public static Vector2 designedSize = new Vector2(1024,768);

	public static Rect CreateRect(float x, float y, float width, float height)
	{
		return new Rect(Screen.width * (x / designedSize.x), Screen.height * (y / designedSize.y),
			Screen.width * (width / designedSize.x), Screen.height * (height / designedSize.y));
	}
	public static Rect CreateRect(Rect rect)
	{
		return CreateRect(rect.x, rect.y, rect.width, rect.height);
	}
	public static Vector2 CreateVector2(float x, float y)
	{
		return new Vector2(Screen.width * (x / designedSize.x), Screen.height * (y / designedSize.y));
	}
}

#region Custom UI Classes
/// <summary>
/// A wrapper class for a Unity's GUI.Toggle. Enables independent control 
/// of the toggle and the label.
/// </summary>
public class CustomToggle
{
	#region Private Variables
	string label;

	Rect labelRect;
	Rect toggleRect;
	#endregion

	#region Methods
	public CustomToggle(Rect labelRectParam, Rect toggleRectParam, string labelParam)
	{
		label = labelParam;
		labelRect = labelRectParam;
		toggleRect = toggleRectParam;
	}

	public bool Draw(bool toggle, GUIStyle style)
	{
		GUI.Label(labelRect, label, style);
		return GUI.Toggle(toggleRect, toggle,"");
	}
	#endregion

	#region Accessors
	public Rect Area
	{
		get { return labelRect; }
	}
	#endregion
}

/// <summary>
/// A wrapper class to create a standard UI intfield.
/// </summary>
public class IntField
{
	#region Private Variables
	string label;
	string field;

	int defaultValue;
	int maxValue;
	int minValue;
	int value;

	Rect rect;
	Rect labelRect;
	#endregion

	#region Methods
	public IntField(Rect labelRectParam, Rect rectParam, string labelParam, int maxParam=int.MaxValue, int minParam=int.MinValue)
	{
		label = labelParam;
		labelRect = labelRectParam;
		rect = rectParam;
		maxValue = maxParam;
		minValue = minParam;

		defaultValue = 0;

		value = 0;
	}

	public int Draw(int valueParam,GUIStyle labelStyle, GUIStyle fieldStyle)
	{
		GUI.Label(labelRect, label,labelStyle);

		field = GUI.TextField(rect, valueParam.ToString(),fieldStyle);
		if(!int.TryParse(field, out value))
			return defaultValue;
		else
		{
			if(value > maxValue)
				value = maxValue;
			else if(value < minValue)
				value = minValue;

			return value;
		}
	}
	#endregion

	#region Accessors
	public Rect Area
	{
		get { return rect; }
	}
	#endregion
}

/// <summary>
/// A wrapper class to create a standard UI double field.
/// </summary>
public class DoubleField
{
	#region Private Variables
	string label;
	string field;

	double defaultValue;
	double maxValue;
	double minValue;
	double value;

	Rect rect;
	Rect labelRect;
	#endregion

	#region Constructor
	public DoubleField(Rect labelRectParam, Rect rectParam, string labelParam, double maxParam=double.MaxValue, double minParam=double.MinValue)
	{
		label = labelParam;
		labelRect = labelRectParam;
		rect = rectParam;
		maxValue = maxParam;
		minValue = minParam;

		defaultValue = 0.0f;

		value = 0.0;
	}
	#endregion

	#region Methods
	public double Draw(double valueParam,GUIStyle labelStyle,GUIStyle fieldStyle=null)
	{
		GUI.Label(labelRect, label,labelStyle);

		field = GUI.TextField(rect, valueParam.ToString(),fieldStyle);
		if(!double.TryParse(field, out value))
			return defaultValue;
		else
		{
			if(value > maxValue)
				value = maxValue;
			else if(value < minValue)
				value = minValue;

			return value;
		}
	}
	#endregion

	#region Accessors
	public Rect Area
	{
		get { return rect; }
	}
	#endregion
}
#endregion

#region UI System Classes
/// <summary>
/// Wrapper class for variables used by UILabels.
/// </summary>
[System.Serializable]
public class CustomStyle
{
	#region Public Variables
	public bool custom;

	public string styleName;

	public GUIStyle style;
	#endregion

	#region Constructors
	public CustomStyle()
	{
		custom = false;
		styleName = "";
		style = new GUIStyle();
	}

	public CustomStyle(bool customParam, string styleNameParam, GUIStyle styleParam)
	{
		custom = customParam;
		styleName = styleNameParam;
		style = styleParam;
	}
	#endregion

	#region Initialization Methods
	public void SetDefaultStyle(string name)
	{
		if(!custom)
		{
			if(styleName == "")
				styleName = name;

			custom = true;
			style = new GUIStyle(UINavigationController.Skin.FindStyle(styleName));
		}
	}
	#endregion
}

/// <summary>
/// Wrapper class for variables used in UI Transitions.
/// </summary>
[System.Serializable]
public class Transition
{
	#region Public Variables
	//In pixels per second
	public float _speed;
	//public float TargetScale;

	//How much the screen moves upon entry
	public Vector2 _targetPosition;
	#endregion

	#region Constructors
	public Transition(){}

	public Transition(Vector2 targetPosition, float speed)
	{
		_targetPosition =targetPosition;
		_speed = speed;
	}
	#endregion
}

/// <summary>
/// Wrapper class for variables used in UIFadeComponents.
/// </summary>
[System.Serializable]
public class FadeSettings
{
	#region Public Variables
	public bool Triggered;

	public float Speed; //time to fade in seconds
	public float Delay; //delay time in seconds

	public Color TargetColor;
	#endregion

	#region Constructors
	public FadeSettings(){}

	public FadeSettings(float speed, float delay, Color target, bool triggered=false)
	{
		Speed = speed;
		Delay = delay;
		TargetColor = target;
		Triggered = triggered;
	}
	#endregion
}

#region Enumerations
public enum MovementType { NONE = 0, LERP, LINEAR, ROTATE, SCALE };
public enum RotatePoint {TOPLEFT = 0, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT};
#endregion
#endregion
