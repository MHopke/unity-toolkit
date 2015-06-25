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
    	#region Public Variables
    	//Allows the UI element to skip activation from a UIView.
    	//Used in situations when an element should be hidden by default
        public bool _skipUIViewActivation;
        public bool _startsDisabled;

        public Animator _animator;
    	#endregion

    	#region Protected Variables
    	//Determines if the element is currently active
    	protected bool _active;
        protected bool _previousEnabledState;
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

                if (_skipUIViewActivation)
                {
                    PresentVisuals(false);
                }
    		}
    	}

    	protected virtual void OnInit()
        {
            if (!_animator)
                _animator = GetComponent<Animator>();
        }
        public void CleanUp()
        {
            OnCleanUp();
        }
        protected virtual void OnCleanUp(){}

        /// <summary>
        /// Activates the UI element.
        /// </summary>
    	public void Activate()
    	{
    		if(_active)
    			return;

    		_active = true;

    		OnActivate();

            if(_startsDisabled)
                Disable();

    		#if LOG
    		Debug.Log(name + "activated");
    		#endif
    	}

    	protected virtual void OnActivate()
        {
            PresentVisuals(true);
        }

    	public void Remove()
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
            PresentVisuals(false);
        }

    	//used for disabling interactive elements
        public void Disable(bool force=false)
    	{
            if (_active || force)
                Disabled();
    	}

    	protected virtual void Disabled(){}

    	public void Enable()
    	{
    		if(!_active)
    			return;

    		Enabled();
    	}
    	protected virtual void Enabled(){}

        public virtual void LostFocus(){}
        public virtual void GainedFocus(){}

        public virtual void PresentVisuals(bool display) 
        {
            #if LOG
            Debug.Log(name + " : " + display);
            #endif

            if (display)
                Enabled();
            else
                Disabled();
        }
    	#endregion

    	#region Exit Methods
        public virtual void Exit()
    	{
    		if(!_active)
    			return;

    		Remove();
    	}
    	#endregion

        #region Animation Methods
        public void SetTrigger(string triggerName)
        {
            if (_animator && _animator.runtimeAnimatorController)
            {
                //Debug.Log(triggerName);
                _animator.SetTrigger(triggerName);
                //enabled = true;
            }
        }
        public void SetBool(string name, bool value)
        {
            if (_animator && _animator.runtimeAnimatorController)
            {
                _animator.SetBool(name, value);
                //enabled = true;
            }
        }
        #endregion

    	#region Accessors
    	public bool Active
    	{
    		get { return _active; }
    		set { _active = value; }
    	}
    	#endregion
    }
}