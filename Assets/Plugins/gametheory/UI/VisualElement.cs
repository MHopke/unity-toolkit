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

    	#region Methods
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

    	
        public void CleanUp()
        {
            OnCleanUp();
        }

        /// <summary>
        /// Activates the UI element.
        /// </summary>
    	public void Present()
    	{
    		if(_active)
    			return;

    		_active = true;

    		OnPresent();

            if(StartsDisabled)
                Disable();

    		#if LOG
    		Debug.Log(name + "activated");
    		#endif
    	}

    	public void Remove()
    	{
    		if(!_active)
    			return;

    		_active = false;

    		OnRemove();

    		#if LOG
    		Debug.Log(name + " deactivated");
    		#endif
    	}

    	//used for disabling interactive elements
        public void Disable(bool force=false)
    	{
            if (_active || force)
                Disabled();
    	}

    	public void Enable()
    	{
    		if(!_active)
    			return;

    		Enabled();
    	}
    	
        public void LostFocus()
        {
            if(!_active)
                return;

            OnLostFocus();
        }
        public void GainedFocus()
        {
            if(!_active)
                return;

            OnGainedFocus();
        }

        public void SetTrigger(string triggerName)
        {
            if (Animator)
            {
                //Debug.Log(triggerName);
                Animator.SetTrigger(triggerName);
                //enabled = true;
            }
        }
        public void SetBool(string name, bool value)
        {
            if (Animator)
            {
                Animator.SetBool(name, value);
                //enabled = true;
            }
        }
    	#endregion

        #region Virtual Methods
        protected virtual void OnInit()
        {
            if (!Animator)
                Animator = GetComponent<Animator>();
        }

        protected virtual void OnCleanUp(){}

        protected virtual void OnPresent()
        {
            PresentVisuals(true);
        }
        
        protected virtual void OnRemove()
        {
            PresentVisuals(false);
        }

        protected virtual void Disabled(){}
        protected virtual void Enabled(){}
        
        public virtual void OnLostFocus(){}
        public virtual void OnGainedFocus(){}
        
        public virtual void PresentVisuals(bool display) 
        {
            #if LOG
            Debug.Log(name + " : " + display);
            #endif
        }
        #endregion
    }
}