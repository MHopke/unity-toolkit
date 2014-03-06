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
	//Fired when a controller finishes its transition out
	public static event System.Action<string> transitionOutFinishedEvent;
	//Fired when a controller finishes its transition in
	public static event System.Action<string> transitionDidFinishEvent;
	#endregion

	#region Public Variables
	//The file path that UIViewControllers will be loaded from.
	public string FilePath;

	//The Custom GUISkin used by the game's UI.
	public GUISkin _skin;

	//Dimensions that the assets were created in.
	public Vector2 DESIGNED_RESOLUTION;

	//The relative scale of the current Screen dimensions compared
	//to the DESIGNED_RESOLUTION. Used to scale UI positions & sizes.
	public static Vector2 AspectRatio;

	public UIFadeTransition _fadeTransition;

	public UIViewController currentController;
	#endregion

	#region Private Variables
	string _targetControllerId;

	UIViewController previousController;

	static UINavigationController instance = null;
	#endregion

	#region Unity Methods
	void Awake()
	{
		if(!instance)
		{
			AspectRatio = new Vector2((float)Screen.width / DESIGNED_RESOLUTION.x,
				(float)Screen.height / DESIGNED_RESOLUTION.y);

			//Adjust the camera's orthographic size incase it is a different aspect ratio.
			//Depending on the orientation you need a different calculation.
			//Ensures that Sprites remain the correct size.
			if(Screen.height > Screen.width)
				Camera.main.orthographicSize = ((float)Screen.height / (float)Screen.width) * 2.0f;
			else
				Camera.main.orthographicSize = ((float)Screen.width / (float)Screen.height) * 2.0f;

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
				_skin.customStyles[i].fontSize = Mathf.RoundToInt(AspectRatio.x * (float)_skin.customStyles[i].fontSize);

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

	void SwitchControllers()
	{
		if(instance.previousController)
		{
			if(_targetControllerId == instance.previousController.name)
			{
				UIViewController temp = instance.previousController;

				instance.SetPreviousController();

				instance.currentController = temp;

				instance.currentController.Activate(true);
			} else
			{
				//Delete the old controller
				instance.UnloadPreviousController();

				//Set the previous controller
				instance.SetPreviousController();

				//Load the new controller
				instance.LoadNewController(_targetControllerId);
			}
		}
		else
		{
			//Set the previous controller
			instance.SetPreviousController();

			//Load the new controller
			instance.LoadNewController(_targetControllerId);
		}
	}

	void SetPreviousController()
	{
		previousController = currentController;
		previousController.Deactivate();
	}

	void UnloadPreviousController()
	{
		if(unloadedControllerEvent != null)
			unloadedControllerEvent(previousController.name);

		Destroy(previousController.gameObject);

		Resources.UnloadUnusedAssets();
	}
	void LoadNewController(string controllerId)
	{
		GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load(instance.FilePath + controllerId));

		if(obj)
		{
			instance.currentController = obj.GetComponent<UIViewController>();
			instance.currentController.name = instance.currentController.name.Remove(instance.currentController.name.Length - 7, 7);
			//Debug.Log(instance.currentController.name);
			instance.currentController.Activate();

			if(loadedNewControllerEvent != null)
				loadedNewControllerEvent(controllerId);
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

		if(transitionOutFinishedEvent != null)
			transitionOutFinishedEvent(previousController.name);
	}
	void TransitionOutFinished()
	{
		UINavigationController.CurrentController.GainFocus();

		if(transitionDidFinishEvent != null)
			transitionDidFinishEvent(_targetControllerId);
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
