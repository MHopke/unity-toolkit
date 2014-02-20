using UnityEngine;
using System.Collections;

public class UILabelButton : UILabel 
{
	#region Public Variables
	public ButtonSettings _buttonSettings;
	#endregion

	#region DrawMethods
	protected override void OnGUI()
	{
		useGUILayout = false;

		GUI.skin = UINavigationController.Skin;

		GUI.depth = depth;

		if(GUI.Button(drawRect, text, (customStyle.custom) ? customStyle.style : customStyle.styleName))
			_buttonSettings.Click();
	}
	#endregion
}
