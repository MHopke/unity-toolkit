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
        //These four events will always fire, and come coupled (even if there are no animations).
        //transitionEvent will always follow activatedEvent
        //transitionOutEvent will always follow deactivatedEvent

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
		[Tooltip("Determines if the view tranverses it's " +
			"heirachry to add Elements upon Init. You almost " +
			"always want this set to true")]
		public bool UseHierarchy = true;
        public bool Animates;
		[Tooltip("Determines if the view is " +
			"displayed when the parent ViewController is Activated")]
        public bool HiddenByDefault;
		[Tooltip("Used to keep the view's gameobject active " +
			"until a long running process has finished")]
        public bool DelayedDeactivation;
		public bool LoadedFromResources;

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
		protected bool _inNavigation;
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

				VisualElement element = null;
                for (int i = 0; i < ui.Length; i++)
				{
					element = ui[i];

					if(element != null && !element.InSubGroup)
                    	Elements.Add(ui[i]);
				}
            }

            OnInit();

            _active = false;
            _init = true;

            if(!DelayedDeactivation)
                gameObject.SetActive(false);
        }

        public void ActivateWithoutAnimation()
        {
            if(_active) return;

            gameObject.SetActive(true);

            _active = true;

            if(CanvasGroup)
                CanvasGroup.blocksRaycasts = true;

            OnActivate();

            OnAnimateIn();

            ActivateEvent();
        }

        public void Activate(string animation="")
    	{
            if(_active) return;

            gameObject.SetActive(true);

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

            OnActivate();

            ActivateEvent();

            if(!Animates)
                TransitionInEvent();
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
                OnDeactivate();

            DeactivateEvent();
    		//Resources.UnloadUnusedAssets();
    	}
    	
        public void CleanUp()
        {
            OnCleanUp();
        }

		public void SetToFront()
		{
			transform.SetAsLastSibling();
		}
    	#endregion

    	#region Interaction Methods
    	public void LostFocus()
        {
            OnLostFocus();
        }
        public void GainedFocus()
        {
            OnGainedFocus();
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

            OnDeactivate();
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
		public static UIView Load(string path)
		{
			return Load(path,UIAlertController.Instance.CanvasRect);
		}
		public static UIView Load(string path, Transform parent)
		{
			UIView obj = (UIView)GameObject.Instantiate(Resources.Load<UIView>(path),Vector3.zero,Quaternion.identity);
			(obj.transform as RectTransform).SetParent(parent,false);

			obj.Initialize();

			return obj;
		}
        public void AddUIElement(VisualElement element, bool activate)
        {
            Elements.Add(element);
            element.Init();

            if (activate)
                element.Activate();
        }
        public void RemoveUIElement(VisualElement element)
        {
            if(!Elements.Contains(element))
                return;

            Elements.Remove(element);
        }
		public void SetInNavigation(bool status)
		{
			_inNavigation = status;
		}
        #endregion

        #region Virtual Methods
        protected virtual void OnInit()
        {
            if (HiddenByDefault && CanvasGroup)
                CanvasGroup.blocksRaycasts = false;
            
            if(Elements != null)
            {
                for(int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i])
                    {
                        //Debug.Log(_elements[i].name);
                        
                        Elements[i].Init();
                        
                        //Disables all renders, etc so that you don't have to manually do it
                        if (HiddenByDefault)
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

        /// <summary>
        /// Overloadable method which handles the actual activation of UI elements
        /// </summary>
        protected virtual void OnActivate()
        {
            if(Elements != null)
            {
                for(int i = 0; i < Elements.Count; i++)
                {
                    if(Elements[i] && !Elements[i].HiddenByDefault)
                        Elements[i].Activate();
                }
            }
            
			if(LoadedFromResources)
				UIViewController.Instance.AddViewToList(this);

            #if LOG
            Debug.Log(name + " activated.");
            #endif
        }

        /// <summary>
        /// Overloadable method which handles the actual deactivation of UI elements
        /// </summary>
        protected virtual void OnDeactivate()
        {
            if(Elements != null)
            {
                for(int i = 0; i < Elements.Count; i++)
                {
                    if(Elements[i])
                        Elements[i].Deactivate();
                }
            }
            
            gameObject.SetActive(false);

            TransitionOutEvent();

			if(LoadedFromResources && !_inNavigation)
			{
				UIViewController.Instance.RemoveViewFromList(this);
				Destroy(gameObject);
			}

            #if LOG
            Debug.Log(name + " deactivated.");
            #endif
        }

        protected virtual void OnCleanUp()
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
        protected virtual void OnLostFocus()
        {
            if(!_active)
                return;
            
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
        
        protected virtual void OnGainedFocus()
        {
            if(!_active)
                return;
            
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
        #endregion
    }
}