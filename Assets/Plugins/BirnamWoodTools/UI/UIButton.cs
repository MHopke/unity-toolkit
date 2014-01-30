﻿using UnityEngine;
using System.Collections;

public class UIButton : UISprite 
{
	#region Events
	public event System.Action clickEvent;
	#endregion

	#region Public Variables
	public ChangeUIScreen Header;
	public ChangeUIScreen Content;
	public ChangeUIScreen Footer;
	#endregion

	#region Private Variables
	bool disabled;

	int fingerID;

	Animator animator;
	#endregion

	#region Activation, Deactivation, Init Methods
	public override void Init(Vector2 offset)
	{
		animator = GetComponent<Animator>();
		base.Init(offset);
	}
	public override void Activate()
	{
		enabled = true;

		renderer.enabled = true;

		fingerID = InputHandler.INVALID_FINGER;

		InputHandler.touchStart += TouchStart;
		InputHandler.touchEnd += TouchEnd;
		InputHandler.touchMoving += TouchMoving;

		base.Activate();
	}
	public override void Deactivate()
	{
		enabled = false;

		renderer.enabled = false;

		InputHandler.touchStart -= TouchStart;
		InputHandler.touchEnd -= TouchEnd;
		InputHandler.touchMoving -= TouchMoving;

		base.Deactivate();
	}
	#endregion

	#region Enable Methods
	public void Enable()
	{
		if(!disabled)
			return;

		InputHandler.touchStart -= TouchStart;
		InputHandler.touchEnd -= TouchEnd;
		InputHandler.touchMoving -= TouchMoving;

		disabled = false;
	}
	public void Disable()
	{
		if(disabled)
			return;

		InputHandler.touchStart += TouchStart;
		InputHandler.touchEnd += TouchEnd;
		InputHandler.touchMoving += TouchMoving;

		disabled = true;
	}
	#endregion

	#region Click Methods
	void Click()
	{
		//Change UIScreens
		Header.ChangeScreen(UIView.Section.HEADER);
		Content.ChangeScreen(UIView.Section.CONTENT);
		Footer.ChangeScreen(UIView.Section.FOOTER);

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
			if(animator)
				animator.SetTrigger("Click");
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
public class ChangeUIScreen
{
	#region Public Variables
	public bool Change;
	public string Screen;
	#endregion

	#region Change Methods
	public void ChangeScreen(UIView.Section section)
	{
		if(Change)
		{
			UIViewController.ChangeScreen(Screen, section);
		}
	}
	#endregion
}