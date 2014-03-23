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

	public static UIBase GetElementFromView(string element, string view)
	{
		for(int i = 0; i < instance._views.Count; i++)
		{
			if(instance._views[i] && view == instance._views[i].name)
				return instance._views[i].RetrieveUIElement(element);
		}

		return null;
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

	public void GainFocus()
	{
		for(int i = 0; i < _views.Count; i++)
		{
			if(_views[i])
				_views[i].GainedFocus();
		}
	}

	public void LoseFocus()
	{
		for(int i = 0; i < _views.Count; i++)
		{
			if(_views[i])
				_views[i].LostFocus();
		}
	}
	#endregion
}
