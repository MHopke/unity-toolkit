//#define DEBUG_ERRORS
#if DEBUG_ERRORS
using UnityEngine;
#endif

public class UIComponentManager 
{
	#region Private Variables
	UIFadeComponent _fadeComponent;
	UIFlashComponent _flashComponent;
	#endregion

	#region Contructors
	public UIComponentManager(UIComponent[] components)
	{
		System.Type type;
		for(int i = 0; i < components.Length; i++)
		{
			type = components[i].GetType();
			if(type == typeof(UIFadeComponent))
				_fadeComponent = components[i] as UIFadeComponent;
			else if(type == typeof(UIFlashComponent))
				_flashComponent = components[i] as UIFlashComponent;
		}

		if(_fadeComponent)
			_fadeComponent.Init();

		if(_flashComponent)
			_flashComponent.Init();
	}
	#endregion

	#region Methods
	public void ActivateComponents()
	{
		if(_fadeComponent)
			_fadeComponent.Activate();

		if(_flashComponent)
			_flashComponent.Activate();
	}
	public void DeactivateComponents()
	{
		if(_fadeComponent)
			_fadeComponent.Deactivate();

		if(_flashComponent)
			_flashComponent.Deactivate();
	}

	public void InvokeMethod(string method)
	{
		switch(method)
		{
		case "FadeIn":
			if(_fadeComponent)
				_fadeComponent.FadeIn();
			break;
		case "FadeOut":
			if(_fadeComponent)
				_fadeComponent.FadeOut();
			break;
		case "Flash":
			if(_flashComponent)
				_flashComponent.Init();
			break;
		#if DEBUG_ERRORS
		default:
			Debug.LogError("Not a recognized method");
			break;
		#endif
		}
	}

	public UIComponent GetComponent(string name)
	{
		if(name == "Fade")
			return _fadeComponent;
		else if(name == "Flash")
			return _flashComponent;
		else
			return null;
	}

	public bool HasFade()
	{
		return (_fadeComponent);
	}
	public bool HasFlash()
	{
		return (_flashComponent);
	}
	#endregion
}
