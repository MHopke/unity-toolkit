//#define LOG
using UnityEngine;

/// <summary>
/// The base class for UI elements. You should not use this class unless inheriting from it.
/// </summary>
public class UIBase : MonoBehaviour {

	#region Enumerations
	public enum MovementState { INITIAL = 0, IN_PLACE, MOVING, EXITING, EXITED }
	#endregion

	#region Constants
	protected const float CLOSE_ENOUGH = 3.0f; //Used when checking transition positions
	protected const float SMOOTH_FACTOR = 25.0f; //Applied to transition movement
	#endregion

	#region Public Variables
	//Allows the UI element to skip activation from a UIView.
	//Used in situations when an element should be hidden by default
	public bool _skipUIViewActivation;

	//Time in seconds to delay rendering
	public float renderDelay;

	//Rectangle to draw the element. (in pixels)
	public Rect _drawRect;

	public ButtonComponent _button;

	public ScreenSetting _screenSetting;

	//The customStyle to apply to the base component of any UI element.
	//If an element uses multiple styles you will need to add more.
	public CustomStyle _primaryStyle;
	#endregion

	#region Protected Variables
	//Determines if the element is currently active
	protected bool _active;

	protected bool _disabled;

	protected float _movementRate;

	protected MovementState _movementState;

	protected Vector2 _startPosition;
	protected Vector2 _currentPosition;
    protected Vector2 _targetPosition;
	#endregion

	#region Private Variables
	bool _initialized;
	#endregion

	#region Init, Activation, Deactivation Methods
    /// <summary>
	/// Initialize the element with the specified speed and offset.
	/// Moves the element to its starting position and links any components.
	/// </summary>
	/// <param name="offset">Offset.</param>
	/// <param name="speedParam">Speed parameter.</param>
	public void Init()
	{
		if(!_initialized)
		{
			OnInit();

			//_primaryStyle.style.fontSize = UIScreen.AspectRatio.x
			_initialized = true;
		}
	}

	protected virtual void OnInit()
	{
		UIScreen.AdjustForResolution(transform,_screenSetting);
		UIScreen.AdjustForResolution(ref _drawRect, _screenSetting);

		_startPosition = new Vector2(_drawRect.x,_drawRect.y);

		_currentPosition = _startPosition;

		_primaryStyle.style.contentOffset.Scale(UIScreen.AspectRatio);

		if(!_button)
			_button = GetComponent<ButtonComponent>();
	}

	public void Activate(MovementState state=MovementState.INITIAL)
	{
		if(_active)
			return;

		_active = true;

		OnActivate(state);

		#if LOG
		Debug.Log(name + "activated");
		#endif
	}

	protected virtual void OnActivate(MovementState moveState)
	{
		_movementState = moveState;

		if(moveState == MovementState.INITIAL)
			SetStartPosition();
		else if(moveState == MovementState.IN_PLACE)
			SetToPosition();

		if(renderer)
			renderer.enabled = true;

		if(_button)
			_button.Activate();
	}

	public void Deactivate()
	{
		if(!_active)
			return;

		_active = false;

		OnDeactivate();

		#if LOG
		Debug.Log(name + " deactivated");
		#endif
	}
	protected virtual void OnDeactivate()
	{
		_movementState = MovementState.EXITED;

		if(renderer)
			renderer.enabled = false;

		if(_button)
			_button.Deactivate();
	}

	//used for disabling interactive elements
	public void Disable()
	{
		if(!_active)
			return;

		_disabled = true;

		Disabled();
	}

	protected virtual void Disabled()
	{
		if(_button)
			_button.Disable();
	}

	public void Enable()
	{
		if(!_active)
			return;

		_disabled = false;

		Enabled();
	}
	protected virtual void Enabled()
	{
		if(_button)
			_button.Enable();
	}
	#endregion

	#region Draw Methods
	public virtual void Draw(){}
	#endregion

	#region Position Methods
	/// <summary>
	/// Sets the position. Designed to be overriden such as in UISprite.
	/// </summary>
	/// <param name="position">Position.</param>
	protected virtual void SetPosition(Vector2 position)
	{
		_currentPosition = position;
		_drawRect.x = position.x;
		_drawRect.y = position.y;
	}
	public void SetStartPosition()
	{
		SetPosition(_startPosition);
	}
    public void Reposition(Vector2 scale, bool animate=false)
    {
        if (animate)
        {
            _targetPosition = new Vector2(_currentPosition.x * scale.x, _currentPosition.y * scale.y);

            _movementState = MovementState.MOVING;

            enabled = true;
        }
        else
        {
            SetPosition(new Vector2(_currentPosition.x * scale.x, _currentPosition.y * scale.y));
        }
	}
	public void SetToPosition()
	{
		SetPosition(new Vector2(_drawRect.x,_drawRect.y));
	}
	public void SetX(float x)
	{
		_currentPosition.x = x;
		SetPosition(_currentPosition);
	}
	public void SetY(float y)
	{
		_currentPosition.y = y;
		SetPosition(_currentPosition);
	}
	public void SetStartX(float x)
	{
		_startPosition.x = x;
	}

	public void SetStartY(float y)
	{
		_startPosition.y = y;
	}
	#endregion

    #region Size Methods
    public void Resize(Vector2 scale, bool animate = false)
    {
        if (animate)
        {

        }
        else
        {
			transform.Scale(scale.x,scale.y,1);
        }
    }
    #endregion

	#region Exit Methods
    public virtual void Exit()
	{
		if(!_active)
		{
			if(_skipUIViewActivation)
				_movementState = MovementState.EXITED;

			return;
		}
		Deactivate();
	}
	#endregion

	#region Type Methods
	/// <summary>
	/// Determines the base type.
	/// </summary>
	/// <returns>The base type.</returns>
	public virtual System.Type GetBaseType()
	{
		return typeof(UIBase);
	}
	#endregion

	#region Accessors
	public bool Active
	{
		get { return _active; }
		set { _active = value; }
	}
	public bool InPlace
	{
		get { return _movementState == MovementState.IN_PLACE; }
	}
	public bool HasExited
	{
		get { return _movementState == MovementState.EXITED; }
	}

	/// <summary>
	/// Gets the size. UIBase doesn't actually have a size, but inherited might.
	/// </summary>
	protected virtual void GetSize(){}

	/// <summary>
	/// Gets the bounding area of the element (in pixels).
	/// </summary>
	/// <returns>The bounds.</returns>
	public virtual Rect GetBounds(){return _drawRect;}

	public Vector2 CurrentPosition
	{
		get { return _currentPosition; }
		set { SetPosition(value); }
	}
	public Vector2 StartPosition
	{
		get { return _startPosition; }
	}
		
	public ButtonComponent Button
	{
		get { return _button; }
		set { _button = value; }
	}
	#endregion
}