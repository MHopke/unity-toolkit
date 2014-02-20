using UnityEngine;

/// <summary>
/// Button component. Put this on any gameobject to make it a button which
/// responds to the InputHandler.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ButtonComponent : MonoBehaviour {

	#region Events
	public event System.Action clickEvent;
	#endregion

	#region Private Variables
	bool _eventsAdded;

	int _fingerId;
	#endregion

	#region Methods
	public virtual void Activate()
	{
		if(enabled)
			return;

		enabled = true;

		AddListeners();
	}
	public virtual void Deactivate()
	{
		if(!enabled)
			return;

		enabled = false;

		RemoveListeners();
	}

	public void AddListeners()
	{
		if(!enabled || _eventsAdded)
			return;

		_fingerId = InputHandler.INVALID_FINGER;

		InputHandler.AddTouchStart(InputStart);
		InputHandler.AddTouchEnd(InputEnd);
		InputHandler.AddTouchMoving(InputMoving);

		_eventsAdded = true;
	}
	public void RemoveListeners()
	{
		if(!enabled || !_eventsAdded)
			return;

		InputHandler.RemoveTouchStart(InputStart);
		InputHandler.RemoveTouchEnd(InputEnd);
		InputHandler.RemoveTouchMoving(InputMoving);

		_eventsAdded = false;
	}

	/// <summary>
	/// Calls the FireClickEvent Method. Can be overriden for custom 
	/// behavoir.
	/// </summary>
	protected virtual void SendClickEvent()
	{
		FireClickEvent();
	}

	/// <summary>
	/// Actually fires the clickEvent.
	/// </summary>
	protected void FireClickEvent()
	{
		if(clickEvent != null)
			clickEvent();
	}
	#endregion

	#region Input Event Listeners
	//Start
	void InputStart(Vector2 touch, int id)
	{
		if(CheckStart(touch))
			_fingerId = id;
	}
	protected virtual bool CheckStart(Vector2 touch)
	{
		return collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch));
	}

	//End
	void InputEnd(Vector2 touch, int id)
	{
		if(CheckEnd(touch,id))
			SendClickEvent();
	}
	protected virtual bool CheckEnd(Vector2 touch, int id)
	{
		return (collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch)) && id == _fingerId);
	}

	//Moving
	void InputMoving(Vector2 pos, Vector2 delta, int id)
	{
		if(!CheckMoving(pos))
			_fingerId = InputHandler.INVALID_FINGER;
	}
	protected virtual bool CheckMoving(Vector2 pos)
	{
		return collider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(pos));
	}
	#endregion
}
