using UnityEngine;
using System.Collections;

public class UIHistoryView : UIView 
{
	#region Enumerations
	enum ViewState { BOTTOM = 0, MIDDLE, TOP }
	enum MoveState { NONE =0, DRAG, SCROLL }
	#endregion

	#region Constants
	const float DISTANCE_FOR_MOVE = 120f;
	const float CLOSE_ENOUGH = 15f;
	const float TIME_TO_SLIDE = 0.2f;
	#endregion

	#region Public Variables
	public float _topPosition;
	public float _middlePosition;
	public float _bottomPosition;

	public Rect _scrollAreaRect; //the scrollable area of the view
	public Rect _dragRegion; //The region the user can select to drag the view
	#endregion

	#region Private Variables
	float _moveAmount;
	float _targetY;
	float _yDiff;
	float _startY;

	MoveState _moveState;

	ViewState _viewState;

	Rect _realScrollArea; //the clipping box so the scroll area doesn't go into the drag area
	#endregion

	#region Unity Methods
	protected override void OnUpdate()
	{
		if(_moveState == MoveState.NONE && _viewRect.y != _targetY)
		{
			_viewRect.y += (_yDiff * Time.deltaTime);

			//Debug.Log(targetX + " " + draggableGroup.x);

			if(Mathf.Abs(_targetY - _viewRect.y) <= CLOSE_ENOUGH)
			{
				_viewRect.y = _targetY;
				_yDiff = 0.0f;
			}
		}
	}
	#endregion

	#region Overriden Method
	protected override void Initialize()
	{
		_scrollAreaRect.Scale(UIScreen.AspectRatio);

		_realScrollArea = new Rect(_viewRect.x, _dragRegion.height, _viewRect.width, _viewRect.height - _dragRegion.height);

		base.Initialize();
	}

	protected override void Activation()
	{
		InputHandler.AddTouchStart(InputStart);
		InputHandler.AddTouchMoving(TouchMoving);
		InputHandler.AddTouchEnd(InputEnd);

		base.Activation();
	}

	protected override void Deactivation()
	{
		base.Deactivation();

		InputHandler.RemoveTouchStart(InputStart);
		InputHandler.RemoveTouchMoving(TouchMoving);
		InputHandler.RemoveTouchEnd(InputEnd);
	}

	protected override void DrawContent()
	{
		GUI.Box(_dragRegion, "");
		GUI.BeginGroup(_realScrollArea);

		GUI.BeginGroup(_scrollAreaRect);
		base.DrawContent();
		GUI.EndGroup();

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
		InputHandler.RemoveTouchStart(InputStart);
		InputHandler.RemoveTouchMoving(TouchMoving);
		InputHandler.RemoveTouchEnd(InputEnd);

		base.LostFocus();
	}
	public override void GainedFocus()
	{
		InputHandler.AddTouchStart(InputStart);
		InputHandler.AddTouchMoving(TouchMoving);
		InputHandler.AddTouchEnd(InputEnd);

		base.GainedFocus();
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
		else if(_scrollAreaRect.y >= 0f)
			_scrollAreaRect.y = 0f;
	}
	#endregion

	#region Touch Events
	void InputStart(Vector2 position, int id)
	{
		if(_dragRegion.Contains(new Vector2(position.x, Screen.height - position.y- _viewRect.y)))
			_moveState = MoveState.DRAG;
		else if ((_viewState > ViewState.BOTTOM) &&_viewRect.Contains(new Vector2(position.x, Screen.height - position.y)))
			_moveState = MoveState.SCROLL;
	}

	void TouchMoving(Vector2 pos, Vector2 delta, int id)
	{
		if(movementState == MovementState.IN_PLACE /*&& _viewRect.Contains(new Vector2(pos.x,Screen.height - pos.y))*/)
		{
			if (_moveState == MoveState.DRAG) {
				_viewRect.y -= delta.y;

				_moveAmount += delta.y;

				if (_viewRect.y <= _topPosition)
					_viewRect.y = _topPosition;
				else if (_viewRect.y >= _bottomPosition)
					_viewRect.y = _bottomPosition;
			}
			else if (_moveState == MoveState.SCROLL) {
				_scrollAreaRect.y += delta.y;

				CheckVerticalBorders();
			}
		}
	}

	void InputEnd(Vector2 position, int id)
	{
		if (_viewState == ViewState.BOTTOM) {
			if (_moveAmount >= DISTANCE_FOR_MOVE) 
			{
				if (Mathf.Abs(_viewRect.y - _middlePosition) < Mathf.Abs(_viewRect.y - _topPosition)) {
					_viewState = ViewState.MIDDLE;
					_targetY = _middlePosition;
				}
				else {
					_viewState = ViewState.TOP;
					_targetY = _topPosition;
				}
			}
			else {
				_targetY = _bottomPosition;
			}
		}
		else if (_viewState == ViewState.MIDDLE) {
			if (_moveAmount >= DISTANCE_FOR_MOVE) {
				_viewState = ViewState.TOP;
				_targetY = _topPosition;
			}
			else if (_moveAmount <= -DISTANCE_FOR_MOVE) {
				_viewState = ViewState.BOTTOM;
				_targetY = _bottomPosition;
			}
			else {
				_targetY = _middlePosition;
			}
		}
		else if (_viewState == ViewState.TOP) {
			if (_moveAmount <= -DISTANCE_FOR_MOVE) {
				if (Mathf.Abs(_viewRect.y - _middlePosition) < Mathf.Abs(_viewRect.y - _bottomPosition)) {
					_viewState = ViewState.MIDDLE;
					_targetY = _middlePosition;
				}
				else {
					_viewState = ViewState.BOTTOM;
					_targetY = _bottomPosition;
				}
			}
			else {
				_targetY = _topPosition;
			}
		}

		_yDiff = (_targetY - _viewRect.y) / TIME_TO_SLIDE;

		_moveAmount = 0;
		_moveState = MoveState.NONE;
	}
	#endregion
}
