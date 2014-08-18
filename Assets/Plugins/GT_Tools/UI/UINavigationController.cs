using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// Controls the navigation between one UIViewController and another.
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
    	/// <summary>
        /// The Custom GUISkin used by this scene.
        /// </summary>
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
        /// <summary>
        /// Removes the old UIViewController and presents the new one.
        /// </summary>
        /// <param name="controllerId">Controller identifier.</param>
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
        /// <summary>
        /// Removes the old UIViewController and presents the new one.
        /// </summary>
        /// <param name="controller">Controller.</param>
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
        /// <summary>
        /// Retrieves the UIViewController with the given id.
        /// </summary>
        /// <returns>The view controller. If no view controller is found it will return null.</returns>
        /// <param name="id">Identifier.</param>
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

        /// <summary>
        /// Removes the old view controller and attempts to present a new one.
        /// </summary>
    	void SwitchControllers()
    	{
    		currentController.Deactivate();

    		ActivateControllerWithId(_targetControllerId);
    	}

        /// <summary>
        /// Searches the list of view controllers and if it finds one with same Id it presents it.
        /// </summary>
        /// <param name="Id">Identifier.</param>
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

    	#region Accessors
    	public static GUISkin Skin
    	{
    		get { return instance._skin; }
    	}
    	#endregion
    }
}