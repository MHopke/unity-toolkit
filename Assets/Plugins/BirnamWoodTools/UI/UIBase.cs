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

	//The customStyle to apply to the base component of any UI element.
	//If an element uses multiple styles you will need to add more.
	public CustomStyle _primaryStyle;
	#endregion

	#region Protected Variables
	//Determines if the element is currently active
	new protected bool active;

	protected bool _disabled;

    float movementRate;

	protected MovementState movementState;

	protected Vector2 startPosition;
	protected Vector2 currentPosition;
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
	public virtual bool Init()
	{
		if(!_initialized)
		{
			UIScreen.AdjustForResolution(ref _drawRect);

			startPosition = new Vector2(_drawRect.x,_drawRect.y);

			_initialized = true;

			return true;
		} else
			return false;
	}
	public virtual bool Activate(MovementState state=MovementState.INITIAL)
	{
		if(active)
			return false;

		movementState = state;

		if(state == MovementState.INITIAL)
		{
			SetStartPosition();
			//enabled = true;

			//Debug.Log(name + " " +transform.parent.position);
		}

		active = true;

		#if LOG
		Debug.Log(name + "activated");
		#endif

		return true;
	}

	public virtual bool DelayedActivation(bool skipTransition=false)
	{
		if (active)
			return false;

		if(skipTransition)
		{
			SetToPosition();

			movementState = MovementState.IN_PLACE;
		}
		else
		{
			movementState = MovementState.INITIAL;
			SetStartPosition();

			//enabled = true;
		}

		active = true;

		return true;
	}

	public virtual bool Deactivate(bool force=false)
	{
		//Debug.Log(name + " disabled");
		if(!active /*|| (!force && !enabled)*/)
			return false;

		#if LOG
		Debug.Log(name + " deactivated");
		#endif

		active = false;

		movementState = MovementState.EXITED;

		return true;
	}

	//used for disabling interactive elements
	public void Disable()
	{
		_disabled = true;
	}

	public void Enable()
	{
		_disabled = false;
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
		currentPosition = position;
	}
	void SetStartPosition()
	{
		SetPosition(startPosition);
	}
    public void Reposition(Vector2 scale, bool animate=false)
    {
        if (animate)
        {
            _targetPosition = new Vector2(currentPosition.x * scale.x, currentPosition.y * scale.y);

            movementState = MovementState.MOVING;

            enabled = true;
        }
        else
        {
            SetPosition(new Vector2(currentPosition.x * scale.x, currentPosition.y * scale.y));
        }
	}
	public void SetToPosition()
	{
		SetPosition(new Vector2(_drawRect.x,_drawRect.y));
	}
	public void SetX(float x)
	{
		currentPosition.x = x;
		SetPosition(currentPosition);
	}
	public void SetY(float y)
	{
		currentPosition.y = y;
		SetPosition(currentPosition);
	}
	public void SetStartX(float x)
	{
		startPosition.x = x;
	}

	public void SetStartY(float y)
	{
		startPosition.y = y;
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
		if(!active)
		{
			if(_skipUIViewActivation)
				movementState = MovementState.EXITED;

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
		get { return active; }
		set { active = value; }
	}
	public bool InPlace
	{
		get { return movementState == MovementState.IN_PLACE; }
	}
	public bool HasExited
	{
		get { return movementState == MovementState.EXITED; }
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
		get { return currentPosition; }
		set { SetPosition(value); }
	}
	public Vector2 StartPosition
	{
		get { return startPosition; }
	}
	#endregion
}