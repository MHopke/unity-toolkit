using UnityEngine;
using System.Collections;

/// <summary>
/// User interface scroll view. The first element in UIElements is the container Sprite.
/// leave it null if there is no container.
/// </summary>
public class UIScrollView : UIView {

	#region Enumerations
	public enum ScrollType { HORIZONTAL = 0, VERTICAL, BOTH }
	#endregion

	#region Public Variables
	public UISprite _background;
	public ScrollType Type;
	public Rect ViewRect; //The original view position
	#endregion

	#region Overriden Method
	protected override void Initialize()
	{
		if(_background)
			_background.Init(Transition.MovementIn, Transition.Speed);

		base.Initialize();
	}

	protected override void Activation()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null && ElementInView(UIElements[i].GetBounds()))
					UIElements[i].Activate();
			}
		}

		if(_background)
			_background.Activate();

		movementState = MovementState.INITIAL;

		InputHandler.AddTouchMoving(TouchMoving);
	}
	protected override void Deactivation()
	{
		if(_background)
			_background.Deactivate();

		base.Deactivation();

		InputHandler.RemoveTouchMoving(TouchMoving);
	}

	public override void LostFocus()
	{
		InputHandler.RemoveTouchMoving(TouchMoving);

		base.LostFocus();
	}
	public override void GainedFocus()
	{
		InputHandler.AddTouchMoving(TouchMoving);

		base.GainedFocus();
	}

	public override void FlagForExit()
	{
		if(_background)
			_background.Exit(Transition.MovementOut);

		base.FlagForExit();
	}
	#endregion

	#region Methods
	bool ElementInView(Rect bounds)
	{
		return bounds.x >= ViewRect.x && bounds.xMax <= ViewRect.xMax && bounds.y >= ViewRect.y
			&& bounds.yMax <= ViewRect.yMax;
	}
	#endregion

	#region Touch Events
	void TouchMoving(Vector2 pos, Vector2 delta, int id)
	{
		if(movementState == MovementState.IN_PLACE/* && ViewRect.Contains(new Vector2(pos.x,Screen.height - pos.y))*/)
		{
			//Ensure that movement is locked if it should be
			if(Type == ScrollType.HORIZONTAL)
				delta.y = 0;
			else if(Type == ScrollType.VERTICAL)
				delta.x = 0;

			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i])
				{
					UIElements[i].CurrentPosition += delta;
					//Debug.Log(UIElements[i].CurrentPosition);

					if(ElementInView(UIElements[i].GetBounds()))
						UIElements[i].Activate(UIBase.MovementState.IN_PLACE);
					else
						UIElements[i].Deactivate(true);
				}
			}
		}
	}
	#endregion
}
