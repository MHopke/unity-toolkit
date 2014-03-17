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

	//Components (if a component is not present it will be null)
	protected UIButton _uiButton;
	protected UIFadeComponent _fadeComponent;
	protected UIFlashComponent _flashComponent;
	#endregion

	#region Private Variables
	float renderTimer;
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
	public virtual void Init()
	{
		position.Scale(UINavigationController.AspectRatio);

		startPosition = position;

		//SetStartPosition();

		_animator = GetComponent<Animator>();

		//Link & initialize components (if they exist).
		_uiButton = GetComponent<UIButton>();

		_fadeComponent = GetComponent<UIFadeComponent>();
		if(_fadeComponent)
			_fadeComponent.Init(this);

		_flashComponent = GetComponent<UIFlashComponent>();
		if(_flashComponent)
			_flashComponent.Init(this);
	}
	public virtual bool Activate(MovementState state=MovementState.INITIAL)
	{
		if(active)
			return false;

		movementState = state;

		if(state == MovementState.INITIAL)
		{
			//SetStartPosition();
            if (_animates)
                transform.parent.position = new Vector3(transform.parent.position.x * UINavigationController.AspectRatio.x, 
                    transform.parent.position.y * UINavigationController.AspectRatio.y,
                    transform.parent.position.z);
            else
                transform.position = new Vector3(transform.position.x * UINavigationController.AspectRatio.x,
                    transform.position.y * UINavigationController.AspectRatio.y,
                    transform.position.z);
            Debug.Log(name + " " +transform.parent.position);
			SetTrigger("Activate");
		}
		else if(state == MovementState.IN_PLACE && _uiButton)
			_uiButton.Activate();

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
			transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x,currentPosition.y,1f));
		else
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x,currentPosition.y,1f));
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
	#endregion

    #region Size Methods
    public void Resize(Vector2 scale, bool animate = false)
    {
        if (animate)
        {

        }
        else
        {
            transform.localScale.Scale(scale);
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

		SetTrigger("Exit");

		//This is deactivated here because buttons shouldn't be usable during the
		//transition out.
		if(_uiButton)
			_uiButton.Deactivate();

		movementState = MovementState.EXITING;
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

	public Color CurrentColor
	{
		get { return GetColor(); }
		set { SetColor(value); }
	}
	#endregion
}