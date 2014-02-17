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
	public Vector2 position;

	//Time in seconds to delay rendering
	public float renderDelay;
	#endregion

	#region Protected Variables
	protected MovementState movementState;
	protected Vector2 startPosition;
	protected Vector2 currentPosition;
	#endregion

	#region Private Variables
	float renderTimer;
	float moveRate;
	float speed;

	Vector2 exitPosition;
	#endregion

	#region Activation, Deactivation, Init Methods
	public virtual void Init(Vector2 offset, float speedParam)
	{
		speed = speedParam;

		position.Scale(UINavigationController.AspectRatio);

		startPosition = position + offset;

		SetStartPosition();

		enabled = false;
	}
	public virtual bool Activate(MovementState state=MovementState.INITIAL)
	{
		if(enabled)
			return false;

		movementState = state;

		if(state == MovementState.INITIAL)
			SetStartPosition();

		enabled = true;

		return true;
	}
	public virtual bool Deactivate()
	{
		if(!enabled)
			return false;

		enabled = false;

		return true;
	}
	#endregion

	#region Update Methods
	protected virtual void Update()
	{
		//Debug.Log(name);
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
					movementState = MovementState.EXITED;

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

	#region Exit Methods
	public virtual void Exit(Vector2 exitPos)
	{
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
	#endregion
}