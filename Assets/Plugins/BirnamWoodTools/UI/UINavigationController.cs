using UnityEngine;
using System;

[RequireComponent(typeof(InputHandler))]
public class UINavigationController : MonoBehaviour 
{
	#region Events
	//Fired when the previous controller is destroyed
	public static event Action<string> unloadedControllerEvent;
	//Fired when a new controller has been loaded from memory
	public static event Action<string> loadedNewControllerEvent;
	#endregion

	#region Constants
	const float PERCENT_FOR_SCALE = 0.01f;
	#endregion

	#region Public Variables
	public string FilePath;
	//Hold the x and y resolutions that the screens were
	//originally designed at. 
	public Vector2 DESIGNED_RESOLUTION;

	//This is used to rescale graphics if the resolution
	//is different than the Designed Resolution
	public static Vector2 AspectRatio;
	#endregion

	#region Private Variables
	public UIViewController currentController;
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
		if(currentController && loadedNewControllerEvent != null)
			loadedNewControllerEvent(currentController.name);
	}
	#endregion

	#region Scale Methods
	/// <summary>
	/// Determines if the UI should be scaled.
	/// </summary>
	/// <returns><c>true</c>, if the Aspect Ratio is greater than the required amount <c>false</c> otherwise.</returns>
	public static bool ShouldScaleDimensions()
	{
		return !(Mathf.Abs(1.0f - AspectRatio.x) < PERCENT_FOR_SCALE || Mathf.Abs(1.0f - AspectRatio.y) < PERCENT_FOR_SCALE);
	}
	#endregion

	#region Control Methods
	public static void NavigateToController(string controllerId)
	{
		//Debug.Log("id: " + controllerId);
		if(controllerId == instance.currentController.name)
			return;
		else if(instance.previousController)
		{
			if(controllerId == instance.previousController.name)
			{
				UIViewController temp = instance.previousController;

				instance.SetPreviousController();

				instance.currentController = temp;

				instance.currentController.Activate();
			} else
			{
				//Delete the old controller
				instance.UnloadPreviousController();

				//Set the previous controller
				instance.SetPreviousController();

				//Load the new controller
				instance.LoadNewController(controllerId);
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

		Destroy(previousController);
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
}
