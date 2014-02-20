using UnityEngine;
using System.Collections;

public class UIButton : UISprite 
{
	#region Public Variables
	public ButtonSettings _buttonSettings;
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
			//Debug.Log(name + "add stuff");
			fingerID = InputHandler.INVALID_FINGER;

			InputHandler.AddTouchStart(TouchStart);
			InputHandler.AddTouchEnd(TouchEnd);
			InputHandler.AddTouchMoving(TouchMoving);

			return true;
		} else
			return false;
	}
	public override bool Deactivate(bool force=false)
	{
		if(base.Deactivate(force))
		{
			//Debug.Log(name + "remove stuff");
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
		if(!active || !disabled)
			return;

		InputHandler.AddTouchStart(TouchStart);
		InputHandler.AddTouchEnd(TouchEnd);
		InputHandler.AddTouchMoving(TouchMoving);

		disabled = false;
	}
	public void Disable()
	{
		if(!active || disabled)
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
		_buttonSettings.Click();
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
			if(_spriteAnimator.runtimeAnimatorController)
				SetTrigger("Click");
			else
				Click();
		}
	}
	void TouchMoving(Vector2 pos, Vector2 delta, int id)
	{
		if(!collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(pos)))
			fingerID = InputHandler.INVALID_FINGER;
	}
	#endregion
}