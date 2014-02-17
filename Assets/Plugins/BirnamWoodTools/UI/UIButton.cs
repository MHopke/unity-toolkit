﻿using UnityEngine;
using System.Collections;

public class UIButton : UISprite 
{
	#region Events
	public event System.Action clickEvent;
	#endregion

	#region Public Variables
	public string ControllerId;

	public ChangeUIView Header;
	public ChangeUIView Content;
	public ChangeUIView Footer;
	#endregion

	#region Private Variables
	bool disabled;

	int fingerID;
	#endregion

	#region Activation, Deactivation, Init Methods
	public override bool Activate(MovementState state=MovementState.INITIAL)
	{
		if(base.Activate(state))
		{
			fingerID = InputHandler.INVALID_FINGER;

			InputHandler.AddTouchStart(TouchStart);
			InputHandler.AddTouchEnd(TouchEnd);
			InputHandler.AddTouchMoving(TouchMoving);

			return true;
		} else
			return false;
	}
	public override bool Deactivate()
	{
		if(base.Deactivate())
		{
			InputHandler.RemoveTouchStart(TouchStart);
			InputHandler.RemoveTouchEnd(TouchEnd);
			InputHandler.RemoveTouchMoving(TouchMoving);

			return true;
		} else
			return false;
	}
	#endregion

	#region Type Methods
	public override System.Type GetBaseType()
	{
		return typeof(UIButton);
	}
	#endregion

	#region Enable Methods
	public void Enable()
	{
		if(!disabled)
			return;

		InputHandler.AddTouchStart(TouchStart);
		InputHandler.AddTouchEnd(TouchEnd);
		InputHandler.AddTouchMoving(TouchMoving);

		disabled = false;
	}
	public void Disable()
	{
		if(disabled)
			return;

		InputHandler.RemoveTouchStart(TouchStart);
		InputHandler.RemoveTouchEnd(TouchEnd);
		InputHandler.RemoveTouchMoving(TouchMoving);

		disabled = true;
	}
	#endregion

	#region Click Methods
	public void Click()
	{
		if(ControllerId == "")
		{
			//Change UIScreens
			Header.ChangeScreen(UIView.Section.HEADER);
			Content.ChangeScreen(UIView.Section.CONTENT);
			Footer.ChangeScreen(UIView.Section.FOOTER);
		} else
			UINavigationController.NavigateToController(ControllerId);

		//Send click event
		if(clickEvent != null)
			clickEvent();
	}
	#endregion

	#region Touch Methods
	void TouchStart(Vector2 touch, int id)
	{
		if(movementState == MovementState.IN_PLACE && collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch)))
			fingerID = id;
	}
	void TouchEnd(Vector2 touch, int id)
	{
		if(collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch)) && id == fingerID)
		{
			//Trigger click animation
			SetTrigger("Click");
		}
	}
	void TouchMoving(Vector2 pos, Vector2 delta, int id)
	{
		if(!collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(pos)))
			fingerID = InputHandler.INVALID_FINGER;
	}
	#endregion
}

[System.Serializable]
public class ChangeUIView
{
	#region Public Variables
	public bool Change;
	public UIView View;
	#endregion

	#region Change Methods
	public void ChangeScreen(UIView.Section section)
	{
		if(Change)
			UIViewController.ChangeScreen(View, section);
	}
	#endregion
}
