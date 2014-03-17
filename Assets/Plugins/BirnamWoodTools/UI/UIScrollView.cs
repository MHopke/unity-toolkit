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
	public UISprite _background;

	public ScrollType _type;

	public Rect _viewRect; //The original view position
	#endregion

	#region Overriden Method
	protected override void Initialize()
	{
		if(_background)
			_background.Init();

		base.Initialize();
	}
		
	protected override void Activation()
	{
		if(_elements != null)
		{
			//Only UI Elements within the _viewRect should be activated
			for(int i = 0; i < _elements.Count; i++)
			{
				if(_elements[i] != null && ElementInView(_elements[i].GetBounds()))
					_elements[i].Activate();
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
			_background.Exit();

		base.FlagForExit();
	}
	#endregion

	#region Methods
	bool ElementInView(Rect bounds)
	{
		return bounds.x >= _viewRect.x && bounds.xMax <= _viewRect.xMax && bounds.y >= _viewRect.y
			&& bounds.yMax <= _viewRect.yMax;
	}
	#endregion

	#region Touch Events
	void TouchMoving(Vector2 pos, Vector2 delta, int id)
	{
		if(movementState == MovementState.IN_PLACE/* && ViewRect.Contains(new Vector2(pos.x,Screen.height - pos.y))*/)
		{
			//Ensure that movement is locked if it should be
			if(_type == ScrollType.HORIZONTAL)
				delta.y = 0;
			else if(_type == ScrollType.VERTICAL)
				delta.x = 0;

			for(int i = 0; i < _elements.Count; i++)
			{
				if(_elements[i])
				{
					_elements[i].CurrentPosition += delta;
					//Debug.Log(UIElements[i].CurrentPosition);

					if(ElementInView(_elements[i].GetBounds()))
						_elements[i].Activate(UIBase.MovementState.IN_PLACE);
					else
						_elements[i].Deactivate(true);
				}
			}
		}
	}
	#endregion
}
