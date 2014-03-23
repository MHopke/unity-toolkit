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
	public bool _animates;

	//Time in seconds to delay rendering
	public float renderDelay;

	//Position in pixels. Top Left is (0,0)
	public Vector2 position;

	//Components (if a component is not present it will be null)
	public ButtonComponent _uiButton;
	public UIFadeComponent _fadeComponent;
	public UIFlashComponent _flashComponent;
	#endregion

	#region Protected Variables
	//Determines if the element is currently active
	new protected bool active;

    float movementRate;

	protected MovementState movementState;

	protected Vector2 startPosition;
	protected Vector2 currentPosition;
    protected Vector2 _targetPosition;

	public Animator _animator;
	#endregion

	#region Private Variables
	bool _initialized;
	#endregion

    #region Unity Methods
    void Update()
    {
        movementRate = 1.0f / (currentPosition - _targetPosition).magnitude;

        SetPosition(Vector2.Lerp(currentPosition, _targetPosition, movementRate * Time.deltaTime));

        if (Mathf.Abs((currentPosition - _targetPosition).magnitude) <= CLOSE_ENOUGH)
        {
            SetPosition(_targetPosition);

            if (CanDisable())
            {
                enabled = false;
            }
        }
    }

    protected virtual bool CanDisable()
    {
        return true;
    }
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
			position.Scale(UIScreen.AspectRatio);

			startPosition = position;

			transform.Scale(UIScreen.AspectRatio.x, UIScreen.AspectRatio.y, 1);

			_animator = GetComponent<Animator>();

			//Initialize components (if they exist).
			if(_fadeComponent)
				_fadeComponent.Init(this);
			if(_flashComponent)
				_flashComponent.Init(this);

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

			if(_animates)
				SetTrigger("Activate");
			else
				Activated();

			//Debug.Log(name + " " +transform.parent.position);
		}
		else if(state == MovementState.IN_PLACE && _uiButton)
			_uiButton.Activate();

		active = true;

		//Activate components
		if(_fadeComponent)
			_fadeComponent.Activate();

		if(_flashComponent)
			_flashComponent.Activate();

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
			SetPosition(position);

			movementState = MovementState.IN_PLACE;

			if(_uiButton)
				_uiButton.Activate();
		}
		else
		{
			movementState = MovementState.INITIAL;
			SetStartPosition();

			enabled = true;

			if(_animates)
				SetTrigger("Activate");
			else
				Activated();
		}

		active = true;

		//Activate components
		if(_fadeComponent)
			_fadeComponent.Activate();

		if(_flashComponent)
			_flashComponent.Activate();

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

		if(_uiButton)
			_uiButton.Deactivate();

		//Deactivate components
		if(_fadeComponent)
			_fadeComponent.Deactivate();

		if(_flashComponent)
			_flashComponent.Deactivate();

		return true;
	}
	#endregion

	#region Position Methods
	/// <summary>
	/// Sets the position. Designed to be overriden such as in UISprite.
	/// </summary>
	/// <param name="position">Position.</param>
	protected virtual void SetPosition(Vector2 position)
	{
		currentPosition = position;

		if(_animates)
			transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x,Screen.height - currentPosition.y,3f));
		else
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x,Screen.height - currentPosition.y,3f));
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
		SetPosition(position);
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

		if(_animates)
		{
			movementState = MovementState.EXITING;
			SetTrigger("Exit");
		}
		else
			Exited();

		//This is deactivated here because buttons shouldn't be usable during the
		//transition out.
		if(_uiButton)
			_uiButton.Deactivate();
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

	#region Color Methods
	protected virtual Color GetColor(){return Color.white;}
	protected virtual void SetColor(Color color){}
	#endregion

	#region Animation Methods
	protected virtual void SetTrigger(string triggerName)
	{
		if(_animator && _animator.runtimeAnimatorController)
			_animator.SetTrigger(triggerName);
	}
	protected virtual void Activated()
	{
		if(_uiButton)
			_uiButton.Activate();
	}
	public virtual void Exited()
	{
		Deactivate();
	}
	#endregion

	#region Accessors
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
	public virtual Rect GetBounds(){return new Rect();}

	public Vector2 CurrentPosition
	{
		get { return currentPosition; }
		set { SetPosition(value); }
	}
	public Vector2 StartPosition
	{
		get { return startPosition; }
	}

	public Color CurrentColor
	{
		get { return GetColor(); }
		set { SetColor(value); }
	}
	#endregion
}