//#define LOG
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class represents a collection of UIViews. Only one
/// UIViewController can be active at a time.
/// </summary>
public class UIViewController : MonoBehaviour 
{
	#region Public Variables
	public List<UIView> _views;
	#endregion

	#region Private Variables
	static UIViewController instance = null;
	#endregion

	#region Activate, Deactivate Methods
	public void Activate()
	{
		for(int i = 0; i < _views.Count; i++)
		{
			if(_views[i] && !_views[i]._skipActivation)
				_views[i].Activate();
		}

		instance = this;

		#if LOG
		Debug.Log(name + " activated.");
		#endif
	}

	public void Deactivate()
	{
		for(int i = 0; i < _views.Count; i++)
		{
			if(_views[i])
				_views[i].FlagForExit();
		}

		#if LOG
		Debug.Log(name + " deactivated.");
		#endif
	}
	#endregion

	#region Methods
	//Present view methods
	public static void PresentUIView(UIView view)
	{
		view.Activate();
	}
	public static void PresentUIView(string view)
	{
		UIView temp = instance.GetUIView(view);

		if(temp)
			temp.Activate();
	}
	public static void PresentUIViewWithTransition(string view, Transition transition)
	{
		UIView temp = instance.GetUIView(view);

		if(temp)
			temp.Activate(transition);
	}
	//Remove view methods
	public static void RemoveUIView(UIView view)
	{
		view.Deactivate();
	}
	public static void RemoveUIView(string view)
	{
		UIView temp = instance.GetUIView(view);

		if(temp)
			temp.Deactivate();
	}
	public static void RemoveUIViewWithTransition(string view, Transition transition)
	{
		UIView temp = instance.GetUIView(view);

		if(temp)
			temp.FlagForExit(transition);
	}

	public static UIBase GetElementFromView(string element, string view)
	{
		for(int i = 0; i < instance._views.Count; i++)
		{
			if(instance._views[i] && view == instance._views[i].name)
				return instance._views[i].RetrieveUIElement(element);
		}

		return null;
	}

	public static bool ElementPersists(string element)
	{
		for(int i = 0; i < instance._views.Count; i++)
		{
			if(instance._views[i] && !instance._views[i]._skipActivation && instance._views[i].HasUIElement(element))
				return true;
		}

		return false;
	}

	public UIView GetUIView(string name)
	{
		for (int i = 0; i < _views.Count; i++)
		{
			if (_views[i] && _views[i].name == name)
				return _views[i];
		}

		return null;
	}

	bool HasUIView(string name)
	{
		for (int i = 0; i < _views.Count; i++)
			if (_views[i] && _views[i].name == name)
				return true;

		return false;
	}

	public static  void GainFocus()
	{
		for(int i = 0; i < instance._views.Count; i++)
		{
			if(instance._views[i])
				instance._views[i].GainedFocus();
		}
	}

	public static void LoseFocus()
	{
		for(int i = 0; i < instance._views.Count; i++)
		{
			if(instance._views[i])
				instance._views[i].LostFocus();
		}
	}
	#endregion
}
