using UnityEngine;
using System.Collections;
using gametheory.UI;

namespace gametheory.Utilities
{
	/// <summary>
	/// Allows a GameObject to have its position in the same manner as UI elements.
	/// </summary>
	public class TransformAdjuster : MonoBehaviour 
	{
	    #region Public Vars
	    public ScaleSettings ScaleSetting;
	    #endregion

	    #region Unity Methods
		// Use this for initialization
		void Start () 
		{
	        UIScreen.Instance.AdjustTransform(transform, ScaleSetting);
		}
	    #endregion
	}
}