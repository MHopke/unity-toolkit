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

	public Vector2 _scrollBounds;

	public Rect _viewRect; //The original view position
	#endregion

	#region Private Variables
	Vector2 _scrollPosition; //used to determine if the scroll has reached its bounds

	Rect _startViewRect;
	Rect _currentViewRect;
	#endregion

	#region Overriden Method
	protected override void Initialize()
	{
		if(_background)
			_background.Init();

		_viewRect.Scale(UIScreen.AspectRatio);

		_startViewRect = _viewRect;

		_currentViewRect = _startViewRect;

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

		_scrollPosition = Vector2.zero;

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

	protected override void InPlace()
	{
		_currentViewRect = _viewRect;

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
		return bounds.xMax >= _currentViewRect.x && bounds.x <= _currentViewRect.xMax && bounds.yMax >= _currentViewRect.y
			&& bounds.y <= _currentViewRect.yMax;
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

			_scrollPosition += delta;

			if(_scrollPosition.y <= -_scrollBounds.y || _scrollPosition.y > 0)
				delta.y = 0;
			if(_scrollPosition.x <= -_scrollBounds.x || _scrollPosition.x > 0)
				delta.x = 0;

			if(delta == Vector2.zero)
				return;

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
