using UnityEngine;
using System.Collections;

/// <summary>
/// Representation of a scroll view.
/// </summary>
public class UIScrollView : UIView {

	#region Enumerations
	public enum ScrollType { HORIZONTAL = 0, VERTICAL, BOTH }
	#endregion

	#region Public Variables
	public UITexture _background;

	public ScrollType _type;

	public Rect _scrollAreaRect; //The original view position
	#endregion

	#region Overriden Method
	protected override void Initialize()
	{
		if(_background)
			_background.Init();

		_scrollAreaRect.Scale(UIScreen.AspectRatio);

		base.Initialize();
	}
		
	protected override void Activation()
	{
		if(_background)
			_background.Activate();

		InputHandler.AddTouchMoving(TouchMoving);

		base.Activation();
	}

	protected override void Deactivation()
	{
		if(_background)
			_background.Deactivate();

		base.Deactivation();

		InputHandler.RemoveTouchMoving(TouchMoving);
	}

	protected override void DrawContent()
	{
		if(_background)
			_background.Draw();

		GUI.BeginGroup(_scrollAreaRect);
		base.DrawContent();
		GUI.EndGroup();
	}

	protected override void InPlace()
	{
		for(int i = 0; i < _elements.Count; i++)
		{
			if(_elements[i] != null)
				_elements[i].SetToPosition();
		}

		base.InPlace();
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

	public override void Exit()
	{
		base.Exit();

		if(_background)
			_background.Exit();
	}
	#endregion

	#region Methods
	void CheckHorizontalBorders()
	{
		if(_scrollAreaRect.x <= -_scrollAreaRect.width)
			_scrollAreaRect.x = -_scrollAreaRect.width;
		else if(_scrollAreaRect.x >= 0.0f)
			_scrollAreaRect.x = 0.0f;
	}
	void CheckVerticalBorders()
	{
		if(_scrollAreaRect.y <= -_scrollAreaRect.height)
			_scrollAreaRect.y = -_scrollAreaRect.height;
		else if(_scrollAreaRect.y >= 0.0f)
			_scrollAreaRect.y = 0.0f;
	}
	#endregion

	#region Touch Events
	void TouchMoving(Vector2 pos, Vector2 delta, int id)
	{
		if(movementState == MovementState.IN_PLACE/* && ViewRect.Contains(new Vector2(pos.x,Screen.height - pos.y))*/)
		{
			//Adjust movement for each type
			if(_type == ScrollType.HORIZONTAL)
			{
				_scrollAreaRect.x += delta.x;

				CheckHorizontalBorders();
			} else if(_type == ScrollType.VERTICAL)
			{
				_scrollAreaRect.y += delta.y;

				CheckVerticalBorders();

			} else
			{
				_scrollAreaRect.x += delta.x;
				_scrollAreaRect.y += delta.y;

				CheckHorizontalBorders();
				CheckVerticalBorders();
			}
		}
	}
	#endregion
}
