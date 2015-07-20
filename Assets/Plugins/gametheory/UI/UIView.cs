//#define LOG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gametheory.UI
{
    /// <summary>
    /// This class is a collection of UI elements. Each UIView
    /// has a position and size. Each GUI element's position & size
    /// are relative to the UIView's position & size.
    /// </summary>
    public class UIView : MonoBehaviour 
    {
    	#region Events
    	//Fired when the screen is activated
    	public event System.Action activatedEvent;
    	//Fired when the screen has completed its transition in
    	public event System.Action transitionInEvent;
    	//Fired when the screen is told to transition out
    	public event System.Action transitionOutEvent;
    	//Fired when the screen is deactivated
    	public event System.Action deactivatedEvent;
    	#endregion

    	#region Public Variables
    	public bool UseHierarchy;
        public bool Animates;
    	public bool SkipActivation;

        public string AnimationInKey;
        public string AnimationOutKey;
        public string AnimationBackKey;
        public string AnimationCurrentBackKey;

        public Animator Animator;

        public CanvasGroup CanvasGroup;

    	public List<VisualElement> Elements;
    	#endregion

        #region Protected Vars
		protected bool _init;
        protected bool _active;
        #endregion

        #region Unity Methods
        void OnDestroy()
        {
            CleanUp();
        }
        #endregion

    	#region Activation, Deactivation Methods
        // Use this for initialization
        public void Initialize()
        {
            if (_init)
                return;

            if (UseHierarchy)
            {
                VisualElement[] ui = GetComponentsInChildren<VisualElement>();

                for (int i = 0; i < ui.Length; i++)
                    Elements.Add(ui[i]);
            }

            OnInit();

            _active = false;
            _init = true;
        }

        protected virtual void OnInit()
    	{
    		if(Elements != null)
    		{
                if (SkipActivation)
                {
                    if (CanvasGroup)
                        CanvasGroup.blocksRaycasts = false;
                }

    			for(int i = 0; i < Elements.Count; i++)
    			{
                    if (Elements[i])
                    {
                        //Debug.Log(_elements[i].name);

                        Elements[i].Init();

                        //Disables all renders, etc so that you don't have to manually do it
                        if (SkipActivation)
                        {
                            Elements[i].PresentVisuals(false);
                        }
                    }
    			}
    		}

            #if LOG
            Debug.Log(name + " : initialized");
            #endif
    	}

        public void ActivateWithoutAnimation()
        {
            if(_active) return;

            _active = true;

            if(CanvasGroup)
                CanvasGroup.blocksRaycasts = true;

            Activation();

            OnAnimateIn();

            ActivateEvent();
        }

        public void Activate(string animation="")
    	{
            if(_active) return;

            _active = true;

            if (CanvasGroup)
            {
                CanvasGroup.interactable = true;
                CanvasGroup.blocksRaycasts = true;
            }

            if (Animates)
            {
                if (!Animator.enabled)
                    Animator.enabled = true;

                if (animation == "")
                    Animator.SetTrigger(AnimationInKey);
                else
                    Animator.SetTrigger(animation);

                /*if (backButton)
                    _animator.SetTrigger(_animationBackKey);
                else
                    _animator.SetTrigger(_animationInKey);*/
            }

            Activation();

            ActivateEvent();
    	}

    	/// <summary>
    	/// Overloadable method which handles the actual activation of UI elements
    	/// </summary>
    	protected virtual void Activation()
    	{
    		if(Elements != null)
    		{
    			for(int i = 0; i < Elements.Count; i++)
    			{
    				if(Elements[i] && !Elements[i].SkipUIViewActivation)
    					Elements[i].Present();
    			}
    		}

    		#if LOG
    		Debug.Log(name + " activated.");
    		#endif
    	}

        public void Deactivate(string animation="") 
    	{
            if(!_active)
    			return;

    		//Debug.Log(name + " deactivate");

            _active = false;

            if (CanvasGroup)
            {
                CanvasGroup.interactable = false;
                CanvasGroup.blocksRaycasts = false;
            }

            if (Animates)
            {
                if (Animator)
                    Animator.enabled = true;

                if (animation == "")
                    Animator.SetTrigger(AnimationOutKey);
                else
                    Animator.SetTrigger(animation);
            }
            else
                Deactivation();

            DeactivateEvent();
    		//Resources.UnloadUnusedAssets();
    	}

    	/// <summary>
    	/// Overloadable method which handles the actual deactivation of UI elements
    	/// </summary>
    	protected virtual void Deactivation()
    	{
    		if(Elements != null)
    		{
    			for(int i = 0; i < Elements.Count; i++)
    			{
    				if(Elements[i])
    					Elements[i].Remove();
    			}
    		}

    		#if LOG
    		Debug.Log(name + " deactivated.");
    		#endif
    	}
        protected virtual void CleanUp()
        {
            if(Elements != null)
            {
                for(int i = 0; i < Elements.Count; i++)
                {
                    if(Elements[i])
                        Elements[i].CleanUp();
                }
            }
        }
    	#endregion

    	#region Interaction Methods
    	public virtual void LostFocus()
    	{
    		#if LOG
    		Debug.Log(name + " lost focus");
    		#endif

            if (CanvasGroup)
                CanvasGroup.interactable = false;

    		for(int i = 0; i < Elements.Count; i++)
    		{
                if (Elements[i])
                {
                    Elements[i].LostFocus();
                }
    		}
    	}

    	public virtual void GainedFocus()
    	{
    		#if LOG
    		Debug.Log(name + " gained focus");
    		#endif

            if (CanvasGroup)
                CanvasGroup.interactable = true;

    		for(int i = 0; i < Elements.Count; i++)
    		{
    			if(Elements[i])
                    Elements[i].GainedFocus();
    		}
    	}

    	public VisualElement RetrieveUIElement(string name)
    	{
    		for(int i = 0; i < Elements.Count; i++)
    		{
    			if(name == Elements[i].name)
    				return Elements[i];
    		}

    		return null;
    	}

    	public bool HasUIElement(string element)
    	{
    		for(int i = 0; i < Elements.Count; i++)
    		{
    			if(element == Elements[i].name)
    				return true;
    		}

    		return false;
    	}
    	#endregion

    	#region Exit Methods
    	public void FlagForExit()
    	{
    		Exit();
    	}

    	public virtual void Exit()
    	{
    		if(Elements != null)
    		{
    			for(int i = 0; i < Elements.Count; i++)
    			{
    				if(Elements[i])
    					Elements[i].Exit();
    			}
    		}

            TransitionOutEvent();
    	}
    	#endregion

        #region Animation Methods
        void AnimationInDone()
        {
            if (Animator)
                Animator.enabled = false;
            TransitionInEvent();
            OnAnimateIn();
        }
        protected virtual void OnAnimateIn(){}

        void AnimationOutDone()
        {
            if (Animator)
                Animator.enabled = false;

            Deactivation();
            TransitionOutEvent();
            OnAnimateOut();
        }
        protected virtual void OnAnimateOut(){}
        #endregion

        #region Event Call Methods
        protected void ActivateEvent()
        {
			//GoogleAnalytics.Client.SendScreenHit(name);

            if (activatedEvent != null)
                activatedEvent();
        }
        protected void TransitionOutEvent()
        {
            if(transitionOutEvent != null)
                transitionOutEvent();
        }
        protected void TransitionInEvent()
        {
            if (transitionInEvent != null)
                transitionInEvent();
        }
        protected void DeactivateEvent()
        {
            if (deactivatedEvent != null)
                deactivatedEvent();
        }
        #endregion

        #region Other Methods
        public void AddUIElement(VisualElement element, bool activate)
        {
            Elements.Add(element);
            element.Init();

            if (activate)
                element.Present();
        }
        public void RemoveUIElement(VisualElement element)
        {
            if(!Elements.Contains(element))
                return;

            Elements.Remove(element);
        }
        #endregion

        #region Accessors
        public bool Active
        {
            get { return _active; }
        }
        #endregion
    }
}