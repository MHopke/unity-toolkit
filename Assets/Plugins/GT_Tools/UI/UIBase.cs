//#define LOG
using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// The base class for UI elements. You should never use this class, instead use an existing sub-class or 
    /// create your own.
    /// </summary>
    public class UIBase : MonoBehaviour 
    {
        #region Enumerations
        public enum MovementState { INITIAL = 0, IN_PLACE, MOVING, EXITING, EXITED }
        #endregion

    	#region Public Variables
    	//Allows the UI element to skip activation from a UIView.
    	//Used in situations when an element should be hidden by default
    	public bool _skipUIViewActivation;

    	//Time in seconds to delay rendering
    	public float renderDelay;

    	public ButtonComponent _button;

    	public ScreenSetting _screenSetting;
    	#endregion

    	#region Protected Variables
    	//Determines if the element is currently active
    	protected bool _active;

    	protected bool _disabled;

    	protected MovementState _movementState;
    	#endregion

    	#region Private Variables
    	bool _initialized;
    	#endregion

    	#region Init, Activation, Deactivation Methods
    	public void Init()
    	{
    		if(!_initialized)
    		{
    			OnInit();

    			//_primaryStyle.style.fontSize = UIScreen.AspectRatio.x
    			_initialized = true;
    		}
    	}

    	protected virtual void OnInit()
    	{
    		UIScreen.AdjustTransform(transform,_screenSetting);

            if (!_button)
                _button = GetComponent<ButtonComponent>();
    	}

    	public void Activate(MovementState state=MovementState.INITIAL)
    	{
    		if(_active)
    			return;

    		_active = true;

    		OnActivate(state);

    		#if LOG
    		Debug.Log(name + "activated");
    		#endif
    	}

    	protected virtual void OnActivate(MovementState moveState)
    	{
    		_movementState = moveState;

    		if(renderer)
    			renderer.enabled = true;

    		if(_button)
    			_button.Activate();
    	}

    	public void Deactivate()
    	{
    		if(!_active)
    			return;

    		_active = false;

    		OnDeactivate();

    		#if LOG
    		Debug.Log(name + " deactivated");
    		#endif
    	}
    	protected virtual void OnDeactivate()
    	{
    		_movementState = MovementState.EXITED;

    		if(renderer)
    			renderer.enabled = false;

    		if(_button)
    			_button.Deactivate();
    	}

    	//used for disabling interactive elements
    	public void Disable()
    	{
    		if(!_active)
    			return;

    		_disabled = true;

    		Disabled();
    	}

    	protected virtual void Disabled()
    	{
    		if(_button)
    			_button.Disable();
    	}

    	public void Enable()
    	{
    		if(!_active)
    			return;

    		_disabled = false;

    		Enabled();
    	}
    	protected virtual void Enabled()
    	{
    		if(_button)
    			_button.Enable();
    	}
    	#endregion

    	#region Exit Methods
        public virtual void Exit()
    	{
    		if(!_active)
    		{
    			if(_skipUIViewActivation)
    				_movementState = MovementState.EXITED;

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
    		get { return _active; }
    		set { _active = value; }
    	}
    	public bool InPlace
    	{
    		get { return _movementState == MovementState.IN_PLACE; }
    	}
    	public bool HasExited
    	{
    		get { return _movementState == MovementState.EXITED; }
    	}
    		
    	public ButtonComponent Button
    	{
    		get { return _button; }
    		set { _button = value; }
    	}
    	#endregion
    }
}