using UnityEngine;
using System.Collections;
using gametheory.UI;

/// <summary>
/// Allows a GameObject to have its position in the same manner as UI elements.
/// </summary>
public class TransformAdjuster : MonoBehaviour 
{
    #region Public Vars
    public ScreenSetting _screenSetting;
    #endregion

    #region Unity Methods
	// Use this for initialization
	void Start () 
	{
        UIScreen.AdjustTransform(transform, _screenSetting);
	}
    #endregion
}
