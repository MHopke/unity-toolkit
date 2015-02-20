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
    	public bool _useHierarchy;
        public bool _animates;
    	public bool _skipActivation;

        public string _animationInKey;
        public string _animationOutKey;
        public string _animationBackKey;
        public string _animationCurrentBackKey;

        public Animator _animator;

        public CanvasGroup _canvasGroup;

    	public List<UIBase> _elements;
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
        public void Initialize(bool inCurrentViewController)
        {
            if (_init)
                return;

            if (_useHierarchy)
            {
                UIBase[] ui = GetComponentsInChildren<UIBase>();

                for (int i = 0; i < ui.Length; i++)
                    _elements.Add(ui[i]);
            }

            OnInit(inCurrentViewController);

            _active = false;
            _init = true;
        }

        protected virtual void OnInit(bool inCurrentViewController)
    	{
    		if(_elements != null)
    		{
                if (_skipActivation || !inCurrentViewController)
                {
                    if (_canvasGroup)
                        _canvasGroup.blocksRaycasts = false;
                }

    			for(int i = 0; i < _elements.Count; i++)
    			{
                    if (_elements[i])
                    {
                        //Debug.Log(_elements[i].name);

                        _elements[i].Init();

                        //Disables all renders, etc so that you don't have to manually do it
                        if (_skipActivation || !inCurrentViewController)
                        {
                            _elements[i].PresentVisuals(false);
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

            if(_canvasGroup)
                _canvasGroup.blocksRaycasts = true;

            Activation();

            OnAnimateIn();

            ActivateEvent();
        }

        public void Activate(bool backButton=false)
    	{
            if(_active) return;

            _active = true;

            if (_canvasGroup)
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }

            if (_animates)
            {
                if (backButton)
                    _animator.SetTrigger(_animationBackKey);
                else
                    _animator.SetTrigger(_animationInKey);
            }

            Activation();

            ActivateEvent();
    	}

    	/// <summary>
    	/// Overloadable method which handles the actual activation of UI elements
    	/// </summary>
    	protected virtual void Activation()
    	{
    		if(_elements != null)
    		{
    			for(int i = 0; i < _elements.Count; i++)
    			{
    				if(_elements[i] && !_elements[i]._skipUIViewActivation)
    					_elements[i].Activate();
    			}
    		}

    		#if LOG
    		Debug.Log(name + " activated.");
    		#endif
    	}

        public void Deactivate(bool backButton=false) 
    	{
            if(!_active)
    			return;

    		//Debug.Log(name + " deactivate");

            _active = false;

            if (_canvasGroup)
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }

            if (_animates)
            {
                if (backButton)
                    _animator.SetTrigger(_animationCurrentBackKey);
                else
                    _animator.SetTrigger(_animationOutKey);
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
    		if(_elements != null)
    		{
    			for(int i = 0; i < _elements.Count; i++)
    			{
    				if(_elements[i])
    					_elements[i].Deactivate();
    			}
    		}

    		#if LOG
    		Debug.Log(name + " deactivated.");
    		#endif
    	}
        protected virtual void CleanUp()
        {
            if(_elements != null)
            {
                for(int i = 0; i < _elements.Count; i++)
                {
                    if(_elements[i])
                        _elements[i].CleanUp();
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

            if (_canvasGroup)
                _canvasGroup.interactable = false;

    		for(int i = 0; i < _elements.Count; i++)
    		{
                if (_elements[i])
                {
                    _elements[i].LostFocus();
                }
    		}
    	}

    	public virtual void GainedFocus()
    	{
    		#if LOG
    		Debug.Log(name + " gained focus");
    		#endif

            if (_canvasGroup)
                _canvasGroup.interactable = true;

    		for(int i = 0; i < _elements.Count; i++)
    		{
    			if(_elements[i])
                    _elements[i].GainedFocus();
    		}
    	}

    	public UIBase RetrieveUIElement(string name)
    	{
    		for(int i = 0; i < _elements.Count; i++)
    		{
    			if(name == _elements[i].name)
    				return _elements[i];
    		}

    		return null;
    	}

    	public bool HasUIElement(string element)
    	{
    		for(int i = 0; i < _elements.Count; i++)
    		{
    			if(element == _elements[i].name)
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
    		if(_elements != null)
    		{
    			for(int i = 0; i < _elements.Count; i++)
    			{
    				if(_elements[i])
    					_elements[i].Exit();
    			}
    		}

            TransitionOutEvent();
    	}
    	#endregion

        #region Animation Methods
        void AnimationInDone()
        {
            TransitionInEvent();
            OnAnimateIn();
        }
        protected virtual void OnAnimateIn(){}

        void AnimationOutDone()
        {
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
        public void AddUIElement(UIBase element, bool activate)
        {
            _elements.Add(element);
            element.Init();

            if (activate)
                element.Activate();
        }
        public void RemoveUIElement(UIBase element)
        {
            if(!_elements.Contains(element))
                return;

            _elements.Remove(element);
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