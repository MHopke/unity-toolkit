using UnityEngine;
using System.Collections;

/// <summary>
/// This class acts as an additional abstraction of Unity's Screen class.
/// It's designed to be utilized with any UI/auto sizing objects.
/// </summary>
public class UIScreen : MonoBehaviour {

	#region Public Variables
	public bool _portrait;

	public float PixelToUnits;

    public Vector2 DesignedResolution;

	[HideInInspector]
    public Vector2 _aspectRatio;
	#endregion

	#region Private Variables
	static UIScreen instance = null;
	#endregion

	// Use this for initialization
	void Awake () 
    {
		if(instance == null)
		{
			_aspectRatio = new Vector2((float)Screen.width / DesignedResolution.x,
				(float)Screen.height / DesignedResolution.y);

			//Adjust the camera's orthographic size incase it is a different aspect ratio.
			//Depending on the orientation you need a different calculation.
			//Ensures that Sprites remain the correct size.
			//Camera.main.orthographicSize = (DesignedResolution.y / 2.0f) / PixelToUnits * AspectRatio.y;

			instance = this;
		} else
			Destroy(gameObject);
	}

	#region Methods
	public static void AdjustForResolution(ref Rect rect)
	{
		rect.x *= AspectRatio.x;
		rect.y *= AspectRatio.y;

		if(UIScreen.Portrait)
		{
			rect.width *= AspectRatio.y;
			rect.height *= AspectRatio.y;
		} else
		{
			rect.width *= AspectRatio.x;
			rect.height *= AspectRatio.x;
		}
	}
	#endregion

	#region Accessors
	public static bool Portrait
	{
		get { return instance._portrait; }
	}
	public static Vector2 AspectRatio
	{
		get { return instance._aspectRatio; }
	}
	#endregion
}
