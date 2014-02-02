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
	#endregion

	#region Private Variables
	float renderTimer;
	float moveRate;

	Vector2 currentPosition;
	Vector2 exitPosition;
	#endregion

	#region Activation, Deactivation, Init Methods
	public virtual void Init(Vector2 offset=new Vector2())
	{
		position.Scale(UINavigationController.AspectRatio);

		startPosition = position + offset;

		SetStartPosition();
	}
	public virtual void Activate(MovementState state=MovementState.INITIAL)
	{
		movementState = state;

		SetStartPosition();
	}
	public virtual void Deactivate(){}
	#endregion

	#region Update Methods
	public virtual void UpdateUI(float deltaTime, float speed)
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
				}

			} else if(movementState == MovementState.EXITING)
			{
				moveRate = 1.0f / Vector2.Distance(currentPosition, exitPosition) * speed * Time.deltaTime * SMOOTH_FACTOR;
				SetPosition(Vector2.Lerp(currentPosition, exitPosition, moveRate));

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
		movementState = MovementState.EXITING;
		exitPosition = position + exitPos;
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