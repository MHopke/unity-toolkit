//#define LOG
using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// The base class for UI elements. You should never use this class, instead use an existing sub-class or 
    /// create your own.
    /// </summary>
    public class VisualElement : MonoBehaviour 
    {
    	#region Public Variables
    	//Allows the UI element to skip activation from a UIView.
    	//Used in situations when an element should be hidden by default
        public bool SkipUIViewActivation;
        public bool StartsDisabled;

        public Animator Animator;
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

                if (SkipUIViewActivation)
                {
                    PresentVisuals(false);
                }
    		}
    	}

    	protected virtual void OnInit()
        {
            if (!Animator)
                Animator = GetComponent<Animator>();
        }
        public void CleanUp()
        {
            OnCleanUp();
        }
        protected virtual void OnCleanUp(){}

        /// <summary>
        /// Activates the UI element.
        /// </summary>
    	public void Present()
    	{
    		if(_active)
    			return;

    		_active = true;

    		OnActivate();

            if(StartsDisabled)
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
            if (Animator && Animator.runtimeAnimatorController)
            {
                //Debug.Log(triggerName);
                Animator.SetTrigger(triggerName);
                //enabled = true;
            }
        }
        public void SetBool(string name, bool value)
        {
            if (Animator && Animator.runtimeAnimatorController)
            {
                Animator.SetBool(name, value);
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