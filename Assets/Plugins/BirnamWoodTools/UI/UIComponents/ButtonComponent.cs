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
	bool _active;

	int _fingerId;
	#endregion

	#region Methods
	public virtual bool Activate()
	{
		if(_active)
			return false;

		_active = true;

		Enable();

		AddListeners();

		return true;
	}
	public virtual bool Deactivate()
	{
		if(!_active)
			return false;

		_active = false;

		Disable();

		RemoveListeners();

		return true;
	}

	public virtual void Enable()
	{
		collider2D.enabled = true;
	}
	public virtual void Disable()
	{
		collider2D.enabled = false;
	}

	public void AddListeners()
	{
		if(!_active || _eventsAdded)
			return;

		_fingerId = InputHandler.INVALID_FINGER;

		InputHandler.AddTouchStart(InputStart);
		InputHandler.AddTouchEnd(InputEnd);
		InputHandler.AddTouchMoving(InputMoving);

		_eventsAdded = true;

		//Debug.Log(name + "listeners added");
	}
	public void RemoveListeners()
	{
		if(!_active || !_eventsAdded)
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
		if(audio)
			audio.Play();
		else
			UINavigationController.PlayGenericClick();

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
