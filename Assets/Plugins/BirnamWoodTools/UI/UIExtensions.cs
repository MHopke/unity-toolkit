using UnityEngine;

public static class AutoSized
{
	public static Vector2 designedSize = new Vector2(640,960);
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

public class CustomToggle
{
	string label;

	Rect labelRect;
	Rect toggleRect;

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

	public Rect Area
	{
		get { return labelRect; }
	}
}

public class IntField
{
	string label;
	string field;

	int defaultValue;
	int maxValue;
	int minValue;
	int value;

	Rect rect;
	Rect labelRect;

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

	public Rect Area
	{
		get { return rect; }
	}
}

public class DoubleField
{
	string label;
	string field;

	double defaultValue;
	double maxValue;
	double minValue;
	double value;

	Rect rect;
	Rect labelRect;

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

	public Rect Area
	{
		get { return rect; }
	}
}

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
		if(!custom && styleName == "")
			styleName = name;
	}
	#endregion
}

[System.Serializable]
public class TransitionSettings
{
	#region Public Variables
	//In pixels per second
	public float Speed;
	//public float TargetScale;

	//How much the screen moves upon entry
	public Vector2 MovementIn;
	//How much the screen moves upon exit
	public Vector2 MovementOut;

	public MovementType Type;
	#endregion

	#region Constructors
	public TransitionSettings(){}

	public TransitionSettings(Vector2 start, Vector2 end, float speed, MovementType type)
	{
		MovementIn = start;
		MovementOut = end;
		Speed = speed;
		Type = type;
	}
	#endregion
}

[System.Serializable]
public class FadeSettings
{
	#region Public Variables
	public bool Triggered;

	public float Speed;
	public float Delay;

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

[System.Serializable]
public class ButtonSettings
{
	#region Events
	public event System.Action clickEvent;
	#endregion

	#region Public Variables
	public string ControllerId;

	public ChangeUIView Header;
	public ChangeUIView Content;
	public ChangeUIView Footer;

	public UIBase[] _elementsToActivate;
	public UIBase[] _elementsToDeactivate;
	#endregion

	#region Click Methods
	public void Click()
	{
		if(ControllerId == "")
		{
			//Change UIScreens
			Header.ChangeScreen(UIView.Section.HEADER);
			Content.ChangeScreen(UIView.Section.CONTENT);
			Footer.ChangeScreen(UIView.Section.FOOTER);
		} else
			UINavigationController.NavigateToController(ControllerId);

		int i = 0;
		for(i=0; i < _elementsToActivate.Length; i++)
			_elementsToActivate[i].Activate();
		for(i = 0; i < _elementsToDeactivate.Length; i++)
			_elementsToDeactivate[i].Deactivate(true);

		//Send click event
		if(clickEvent != null)
			clickEvent();
	}
	#endregion
}

[System.Serializable]
public class ChangeUIView
{
	#region Public Variables
	public bool Change;
	public UIView View;
	#endregion

	#region Change Methods
	public void ChangeScreen(UIView.Section section)
	{
		if(Change)
			UIViewController.ChangeView(View, section);
	}
	#endregion
}

#region Enumerations
public enum MovementType { NONE = 0, LERP, LINEAR, ROTATE, SCALE };
public enum RotatePoint {TOPLEFT = 0, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT};
#endregion
