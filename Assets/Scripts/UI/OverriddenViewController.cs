using UnityEngine;

using gametheory.UI;

public class OverriddenViewController : UIViewController 
{
	#region Overridden Methods
	protected override void OnAwake ()
	{
		//TODO: Call any initialize methods here
		//such as setting up the localization dictionary
		base.OnAwake ();
	}
	protected override void OnActivate ()
	{
		#if UNITY_ANDROID
		Screen.fullScreen = false;
		#endif
		base.OnActivate();

		//TODO: perform any startup logic here
		//such as determining if a login, or home page 
		//be displayed
	}
	#endregion
}
