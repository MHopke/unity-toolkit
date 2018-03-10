//#define LOG
using UnityEngine;
using UnityEngine.UI;

using System.Reflection;
using System.Collections.Generic;

namespace gametheory.UI
{
    /// <summary>
    /// The base class for UI elements. You should never use this class, instead use an existing sub-class or 
    /// create your own.
    /// </summary>
	public class VisualElement : DataBinder
    {
    	#region Public Variables
		[Tooltip("Prevents the element from being displayed " +
			"when the parent view is Activated")]
        public bool HiddenByDefault;
        public bool StartsDisabled;
		[Tooltip("Indicates if the element is in a UIGroup, " +
			"in which case it should not be added to the parent view's Elements list")]
		public bool InSubGroup;

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

    			_initialized = true;

                if (HiddenByDefault)
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
    	public void Activate()
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

		public void Hide()
		{
			if(_active)
				OnHide();
		}
		public void Show()
		{
			if(_active)
				OnShow();
		}

		public void SetParent(Transform parent)
		{
			(transform as RectTransform).SetParent(parent,false);
		}
    	#endregion

        #region Virtual Methods
        protected virtual void OnInit()
        {
            if (!Animator)
                Animator = GetComponent<Animator>();
        }

        protected virtual void OnCleanUp()
		{
			ClearContext();	
		}

        protected virtual void OnActivate()
        {
            PresentVisuals(true);
        }
        
        protected virtual void OnDeactivate()
        {
            PresentVisuals(false);
        }

        protected virtual void Disabled(){}
        protected virtual void Enabled(){}
        
        public virtual void OnLostFocus(){}
        public virtual void OnGainedFocus(){}

		protected virtual void OnHide()
		{
			PresentVisuals(false);
		}
		protected virtual void OnShow()
		{
			PresentVisuals(true);
		}
        
        public virtual void PresentVisuals(bool display) 
        {
            #if LOG
            Debug.Log(name + " : " + display);
            #endif
        }

		protected virtual void PresentComponent(Graphic element, bool display)
		{
			if(element)
			{
                if (element.gameObject.activeSelf != display)//element.enabled = display;
                    element.gameObject.SetActive(display);
			}
		}
		protected virtual void PresentButton(Button element, bool display)
		{
			if(element)
			{
                if (element.gameObject.activeSelf != display)//element.enabled = display;
                    element.gameObject.SetActive(display);

                if (element.targetGraphic)
                {
                    if (element.targetGraphic.gameObject.activeSelf != display)
                        element.targetGraphic.gameObject.SetActive(display);
                }
			}
		}
        #endregion

		#region Properties
		public bool Active
		{
			get { return _active; }
		}
		#endregion
    }
}