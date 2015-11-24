using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
	/// <summary>
	/// Use this class to provide specific "app-like" functionality.
	/// </summary>
	public class OverridenAppNav : AppNavigationController 
	{
		#region Overridden Methods
		protected override void OnActivate ()
		{
	#if UNITY_ANDROID
			Screen.fullScreen = false;
	#endif

			base.OnActivate ();
		}
		#endregion
	}
}