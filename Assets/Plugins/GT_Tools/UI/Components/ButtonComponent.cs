using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// Add to any GameObject with a collider2D to make it act as a button.
    /// </summary>
    public class ButtonComponent : MonoBehaviour 
    {
    	#region Events
        /// <summary>
        /// Fires when the user releases the button, and they are within its bounds.
        /// </summary>
    	public event System.Action clickEvent;
        /// <summary>
        /// Fires when the user releases the button, they are within its bounds, and the
        /// button is indexed.
        /// </summary>
    	public event System.Action<int> indexedClickEvent;
    	#endregion

    	#region Public Vars
    	public bool _indexedButton;

    	public int _index;
    	#endregion

    	#region Private Variables
    	protected bool _eventsAdded;
    	protected bool _active;

    	protected int _fingerId;
    	#endregion

    	#region Unity Methods
    	void OnDestroy()
    	{
    		RemoveListeners();
    	}
    	#endregion

    	#region Methods
    	public void Activate()
    	{
    		if(_active)
    			return;
    			
    		_active = true;

    		OnActivate();
    	}
    	protected virtual void OnActivate()
    	{
    		Enable();

    		AddListeners();
    	}
    	public void Deactivate()
    	{
    		if(!_active)
    			return;

    		OnDeactivate();

    		_active = false;
    	}
    	protected virtual void OnDeactivate()
    	{
    		Disable();

    		RemoveListeners();
    	}

    	public void Enable()
    	{
    		collider2D.enabled = true;
    	}
    	public void Disable()
    	{
    		collider2D.enabled = false;
    	}

    	public void AddListeners()
    	{
    		if(!_active || _eventsAdded)
    			return;

    		_fingerId = InputHandler.INVALID_FINGER;

    		InputHandler.AddInputStart(InputStart);
    		InputHandler.AddInputEnd(InputEnd);
    		//InputHandler.AddInputMoving(InputMoving);

    		_eventsAdded = true;

    		//Debug.Log(name + "listeners added");
    	}
    	public void RemoveListeners()
    	{
    		if(!_active || !_eventsAdded)
    			return;

    		InputHandler.RemoveInputStart(InputStart);
    		InputHandler.RemoveInputEnd(InputEnd);
    		//InputHandler.RemoveInputMoving(InputMoving);

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
    		if(_indexedButton && indexedClickEvent != null)
    			indexedClickEvent(_index);

    		if(clickEvent != null)
    			clickEvent();
    	}
    	#endregion

    	#region Input Event Listeners
    	//Start
    	void InputStart(Vector2 touch, int id)
    	{
    		if(CheckStart(touch))
    		{
    			_fingerId = id;
    			ButtonDown();
    		}
    	}
    	protected virtual bool CheckStart(Vector2 touch)
    	{
            return  collider2D.OverlapPoint(UIScreen.UICamera.ScreenToWorldPoint(touch));
    	}
    	protected virtual void ButtonDown(){}

    	//End
    	void InputEnd(Vector2 touch, int id)
    	{
    		if(_fingerId != InputHandler.INVALID_FINGER)
    		{
    			if(CheckEnd(touch, id))
    				SendClickEvent();

    			_fingerId = InputHandler.INVALID_FINGER;

    			ButtonUp();
    		}
    	}
    	protected virtual bool CheckEnd(Vector2 touch, int id)
    	{
            return (collider2D.OverlapPoint(UIScreen.UICamera.ScreenToWorldPoint(touch)) && id == _fingerId);
    	}
    	protected virtual void ButtonUp(){}

    	//Moving
    	void InputMoving(Vector2 pos, Vector2 delta, int id)
    	{
    		if(_fingerId != InputHandler.INVALID_FINGER && !CheckMoving(pos))
    		{
    			_fingerId = InputHandler.INVALID_FINGER;

    			MovedOffButton();
    		}
    	}
    	protected virtual bool CheckMoving(Vector2 pos)
    	{
            return collider2D.OverlapPoint(UIScreen.UICamera.ScreenToWorldPoint(pos));
    	}
    	protected virtual void MovedOffButton(){}
    	#endregion

    	#region Accessors
    	public bool Active
    	{
    		get { return _active; }
    	}
    	#endregion
    }
}