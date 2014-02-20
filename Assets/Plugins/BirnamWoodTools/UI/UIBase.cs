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

	protected UIComponentManager _componentManager;
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

		_componentManager = new UIComponentManager(GetComponents<UIComponent>());
	}
	public virtual bool Activate(MovementState state=MovementState.INITIAL)
	{
		if(active)
			return false;

		movementState = state;

		if(state == MovementState.INITIAL)
			SetStartPosition();

		enabled = true;
		active = true;

		_componentManager.ActivateComponents();

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

		_componentManager.DeactivateComponents();

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

	#region Component Methods
	public void InvokeComponentMethod(string method)
	{
		_componentManager.InvokeMethod(method);
	}
	public UIComponent GetComponent(string component)
	{
		return _componentManager.GetComponent(component);
	}
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