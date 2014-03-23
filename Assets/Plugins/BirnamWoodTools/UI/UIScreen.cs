using UnityEngine;
using System.Collections;

/// <summary>
/// This class acts as an additional abstraction of Unity's Screen class.
/// It's designed to be utilized with any UI/auto sizing objects.
/// </summary>
public class UIScreen : MonoBehaviour {

	public float PixelToUnits;
    public Vector2 DesignedResolution;

    public static Vector2 AspectRatio;

	// Use this for initialization
	void Awake () 
    {
        AspectRatio = new Vector2((float)Screen.width / DesignedResolution.x,
                (float)Screen.height / DesignedResolution.y);

		//Adjust the camera's orthographic size incase it is a different aspect ratio.
		//Depending on the orientation you need a different calculation.
		//Ensures that Sprites remain the correct size.
		//Camera.main.orthographicSize = (DesignedResolution.y / 2.0f) / PixelToUnits * AspectRatio.y;
	}
}
