using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(UIBase))]
public class UIButton : ButtonComponent 
{
	#region Events
	static event System.Action disableInputEvent;
	static event System.Action enableInputEvent;

	public static void DisableUIButtons()
	{
		if(disableInputEvent != null)
			disableInputEvent();
	}
	public static void EnableUIButtons()
	{
		if(enableInputEvent != null)
			enableInputEvent();
	}
	#endregion

	#region Public Variables
	public ButtonSettings _buttonSettings;
	#endregion

	#region Overriden Methods
	public override void Activate()
	{
		disableInputEvent += RemoveListeners;
		enableInputEvent += AddListeners;

		base.Activate();
	}
	public override void Deactivate()
	{
		disableInputEvent -= RemoveListeners;
		enableInputEvent -= AddListeners;

		base.Deactivate();
	}

	protected override void SendClickEvent()
	{
		_buttonSettings.Click();

		base.FireClickEvent();
	}
	#endregion
}