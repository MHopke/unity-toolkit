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

	public UIViewController currentController;
	#endregion

	#region Private Variables
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
		else if(instance.previousController)
		{
			//If the targetted UIViewController is the previous one
			//the two controllers are switched. Otherwise a new one needs
			//loaded from memory.
			if(controllerId == instance.previousController.name)
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
				instance.LoadNewController(controllerId);

				Resources.UnloadUnusedAssets();
			}
		}
		else
		{
			//Set the previous controller
			instance.SetPreviousController();

			//Load the new controller
			instance.LoadNewController(controllerId);
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

	#region Accessors
	public static GUISkin Skin
	{
		get { return instance._skin; }
	}
	#endregion
}
