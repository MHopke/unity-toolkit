using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour {

	#region Enumerations
	public enum MovementState { INITIAL = 0, IN_PLACE, EXITING, EXITED }
	#endregion

	#region Constants
	protected const float CLOSE_ENOUGH = 3.0f;
	protected const float SMOOTH_FACTOR = 25.0f;
	#endregion

	#region Public Variables
	public bool _skipUIViewActivation;
	//Time in seconds to delay rendering
	public float renderDelay;
	public Vector2 position;
	#endregion

	#region Protected Variables
	new protected bool active;

	protected MovementState movementState;

	protected Vector2 startPosition;
	protected Vector2 currentPosition;

	protected UIButton _uiButton;
	protected UIFadeComponent _fadeComponent;
	protected UIFlashComponent _flashComponent;
	#endregion

	#region Private Variables
	float renderTimer;
	float moveRate;
	float speed;

	Vector2 exitPosition;
	#endregion

	#region Init, Activation, Deactivation Methods
	public virtual void Init(Vector2 offset, float speedParam)
	{
		speed = speedParam;

		position.Scale(UINavigationController.AspectRatio);

		startPosition = position + offset;

		SetStartPosition();

		enabled = false;

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
			SetStartPosition();
		else if(state == MovementState.IN_PLACE && _uiButton)
			_uiButton.Activate();

		enabled = true;
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
		if(!active || (!force && !enabled))
			return false;

		enabled = false;
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

	#region Update Methods
	protected virtual void Update()
	{
		if(renderTimer <= 0.0f)
		{
			if(movementState == MovementState.INITIAL)
			{
				moveRate = 1.0f / Vector2.Distance(currentPosition, position) * speed * Time.deltaTime * SMOOTH_FACTOR;
				SetPosition(Vector2.Lerp(currentPosition, position, moveRate));

				if((currentPosition - position).magnitude <= CLOSE_ENOUGH)
				{
					SetPosition(position);
					movementState = MovementState.IN_PLACE;

					//This is activated here because buttons shouldn't be usable during the
					//transition in.
					if(_uiButton)
						_uiButton.Activate();

					if(CanDisable())
						enabled = false;
				}

			} else if(movementState == MovementState.EXITING)
			{
				moveRate = 1.0f / Vector2.Distance(currentPosition, exitPosition) * speed * Time.deltaTime * SMOOTH_FACTOR;
				SetPosition(Vector2.Lerp(currentPosition, exitPosition, moveRate));

				//Debug.Log(name + " exiting");

				if((currentPosition - exitPosition).magnitude <= CLOSE_ENOUGH)
				{
					SetPosition(exitPosition);

					Deactivate();
				}
			}
		} else
			renderTimer -= Time.deltaTime;
	}
	protected virtual bool CanDisable(){return true;}
	#endregion

	#region Position Methods
	protected virtual void SetPosition(Vector2 position)
	{
		currentPosition = position;
	}
	void SetStartPosition()
	{
		SetPosition(startPosition);
	}
	#endregion

	#region Size Methods
	protected virtual void GetSize(){}
	#endregion

	#region Exit Methods
	public virtual void Exit(Vector2 exitPos)
	{
		if(!active)
		{
			if(_skipUIViewActivation)
				movementState = MovementState.EXITED;

			return;
		}

		if(!enabled)
			enabled = true;

		//This is deactivated here because buttons shouldn't be usable during the
		//transition out.
		if(_uiButton)
			_uiButton.Deactivate();

		movementState = MovementState.EXITING;
		exitPosition = position + exitPos;
	}
	#endregion

	#region Type Methods
	public virtual System.Type GetBaseType()
	{
		return typeof(UIBase);
	}
	#endregion

	#region Color Methods
	protected virtual Color GetColor(){return Color.white;}
	protected virtual void SetColor(Color color){}
	#endregion

	#region Accessors
	public bool InPlace
	{
		get { return movementState == MovementState.IN_PLACE; }
	}
	public bool Exited
	{
		get { return movementState == MovementState.EXITED; }
	}

	public Vector2 CurrentPosition
	{
		get { return currentPosition; }
		set { SetPosition(value); }
	}

	public virtual Rect GetBounds(){return new Rect();}

	public Color CurrentColor
	{
		get { return GetColor(); }
		set { SetColor(value); }
	}
	#endregion
}