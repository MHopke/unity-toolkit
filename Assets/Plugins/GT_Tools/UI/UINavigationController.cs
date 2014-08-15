using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// This class controls the flow from one UIViewController to another. Only 1 UIViewController
    /// should be active at a time.
    /// </summary>
    public class UINavigationController : MonoBehaviour 
    {
    	#region Events
    	//Fired when a new controller has been loaded
    	public static event System.Action<string> loadedNewControllerEvent;
    	//Fired when a controller starts its transition in
    	public static event System.Action<string> transitionInStartedEvent;
    	//Fired when a controller finishes its transition in
    	public static event System.Action<string> transitionDidFinishEvent;
    	#endregion

    	#region Public Variables
    	//The Custom GUISkin used by the game's UI.
    	public GUISkin _skin;

    	public AudioClip _buttonDown;
    	public AudioClip _buttonUp;

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
    			//Scale fonts to proper size. This can't be done in editor because it will persist
    			//after the game has run.
    			#if !UNITY_EDITOR
    			if(_skin != null)
    			{
    			for(int i = 0; i < _skin.customStyles.Length; i++)
    			{
    			_skin.customStyles[i].fontSize = Mathf.RoundToInt(UIScreen.AspectRatio.x * (float)_skin.customStyles[i].fontSize);
    			_skin.customStyles[i].contentOffset.Scale(UIScreen.AspectRatio);
    			//Debug.Log(_skin.customStyles[i].name + " " +_skin.customStyles[i].fontSize);
    			}

    			//Add any standard skins that you utilize as well such as the example below
    			_skin.label.fontSize = Mathf.RoundToInt((UIScreen.AspectRatio.x * (float)_skin.label.fontSize));

    			_skin.textField.fontSize = Mathf.RoundToInt((UIScreen.AspectRatio.x * (float)_skin.textField.fontSize));
    			}
    			#endif

    			instance = this;
    		} else
    			Destroy(gameObject);
    	}

    	void Start()
    	{
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

    	void SwitchControllers()
    	{
    		currentController.Deactivate();

    		ActivateControllerWithId(_targetControllerId);
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
    		UIViewController.LoseFocus();

    		if(transitionInStartedEvent != null)
    			transitionInStartedEvent(_targetControllerId);
    	}
    	void TransitionOutFinished()
    	{
    		UIViewController.GainFocus();

    		if(transitionDidFinishEvent != null)
    			transitionDidFinishEvent(_targetControllerId);
    	}
    	#endregion

    	#region Audio Methods
    	public static void PlayButtonDown()
    	{
    		//if(/*DataManager.AudioOn &&*/ instance.audio && !instance.audio.isPlaying)
    		//instance.audio.PlayOneShot(instance._buttonDown);
    	}

    	public static void PlayButtonUp()
    	{
    		//if(/*DataManager.AudioOn &&*/ instance.audio && !instance.audio.isPlaying)
    		//instance.audio.PlayOneShot(instance._buttonUp);
    	}

    	public static void PlayGenericClick()
    	{
    		//if(DataManager.AudioOn && instance.audio && !instance.audio.isPlaying)
    		//instance.audio.Play();
    	}
    	#endregion

    	#region Accessors
    	public static GUISkin Skin
    	{
    		get { return instance._skin; }
    	}
    	#endregion
    }
}