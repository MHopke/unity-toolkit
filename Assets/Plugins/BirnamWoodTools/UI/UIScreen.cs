using UnityEngine;
using System.Collections;

/// <summary>
/// This class acts as an additional abstraction of Unity's Screen class.
/// It's designed to be utilized with any UI/auto sizing objects.
/// </summary>
public class UIScreen : MonoBehaviour {
    public Vector2 DesignedResolution;

    public static Vector2 AspectRatio;

	// Use this for initialization
	void Awake () 
    {
        AspectRatio = new Vector2((float)Screen.width / DesignedResolution.x,
                (float)Screen.height / DesignedResolution.y);
	}
}
