using UnityEngine;

/// <summary>
/// This class controls which UIViewControllers are currently active and
/// will automatically load & unload UIViewControllers from memory.
/// </summary>
[RequireComponent(typeof(InputHandler))]
public class UINavigationController : MonoBehaviour 
{
	#region Events
	//Fired when the previous controller is destroyed
	public static event System.Action<string> unloadedControllerEvent;
	//Fired when a new controller has been loaded from memory
	public static event System.Action<string> loadedNewControllerEvent;
	//Fired when a controller starts its transition in
	public static event System.Action<string> transitionInStartedEvent;
	//Fired when a controller finishes its transition in
	public static event System.Action<string> transitionDidFinishEvent;
	#endregion

	#region Public Variables
	//The Custom GUISkin used by the game's UI.
	public GUISkin _skin;

	public UIFadeTransition _fadeTransition;

	public UIViewController currentController;

	public UIViewController[] _controllers;
	#endregion

	#region Private Variables
	string _targetControllerId;

	static UINavigationController instance = null;
	#endregion

	#region Unity Methods
	void Awake()
	{
		if(!instance)
		{
			instance = this;
		} else
			Destroy(gameObject);
	}

	void Start()
	{
		//Scale fonts to proper size. This can't be done in editor because it will persist
		//after the game has run.
		#if !UNITY_EDITOR
		if(_skin != null)
		{
			for(int i = 0; i < _skin.customStyles.Length; i++)
				_skin.customStyles[i].fontSize = Mathf.RoundToInt(UIScreen.AspectRatio.x * (float)_skin.customStyles[i].fontSize);

			//_skin.label.fontSize = Mathf.RoundToInt((AspectRatio.x * (float)_skin.label.fontSize));
		}
		#endif

		UIFadeTransition.transitionInFinished += TransitionInFinished;
		UIFadeTransition.transitionOutFinished += TransitionOutFinished;

		if(currentController)
		{
			currentController.Activate();

			//Trigger the loadedNewControllerEvent so that UI elements
			//can be properly linked together.
			if(loadedNewControllerEvent != null)
				loadedNewControllerEvent(currentController.name);
		}
	}
	#endregion

	#region Methods
	public static void NavigateToController(string controllerId)
	{
		//Debug.Log("id: " + controllerId);
		if(controllerId == instance.currentController.name)
			return;
		else
		{
			instance._targetControllerId = controllerId;

			if(instance._fadeTransition)
				instance._fadeTransition.Transition();
			else
				instance.SwitchControllers();
		}
	}
	public static void NavigateToController(UIViewController controller)
	{
		//Debug.Log("id: " + controllerId);
		if(controller.name == instance.currentController.name)
			return;
		else
		{
			instance._targetControllerId = controller.name;

			if(instance._fadeTransition)
				instance._fadeTransition.Transition();
			else
				instance.SwitchControllers();
		}
	}

	void SwitchControllers()
	{
		currentController.Deactivate();

		ActivateControllerWithId(_targetControllerId);
	}

	public static UIViewController GetViewController(string id)
	{
		for(int i = 0; i < instance._controllers.Length; i++)
		{
			if(instance._controllers[i].name == id)
			{
				return instance._controllers[i];
			}
		}
		return null;
	}

	void ActivateControllerWithId(string Id)
	{
		for(int i = 0; i < _controllers.Length; i++)
		{
			if(_controllers[i] && _controllers[i].name == Id)
			{
				currentController = _controllers[i];
				_controllers[i].Activate();

				if(loadedNewControllerEvent != null)
					loadedNewControllerEvent(Id);

				break;
			}
		}
	}
	#endregion

	#region UIFadeTransition Event Listeners
	void TransitionInFinished()
	{
		SwitchControllers();
		UINavigationController.CurrentController.LoseFocus();

		if(transitionInStartedEvent != null)
			transitionInStartedEvent(_targetControllerId);
	}
	void TransitionOutFinished()
	{
		UINavigationController.CurrentController.GainFocus();

		if(transitionDidFinishEvent != null)
			transitionDidFinishEvent(_targetControllerId);
	}
	#endregion

	#region Audio Methods
	public static void PlayGenericClick()
	{
		if(instance.audio && !instance.audio.isPlaying)
			instance.audio.Play();
	}
	#endregion

	#region Accessors
	public static GUISkin Skin
	{
		get { return instance._skin; }
	}
	public static UIViewController CurrentController
	{
		get { return instance.currentController; }
	}
	#endregion
}