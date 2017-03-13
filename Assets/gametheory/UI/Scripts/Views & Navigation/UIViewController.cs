//#define LOG
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace gametheory.UI
{
    /// <summary>
    /// This class represents a collection of UIViews. Only one
    /// UIViewController can be active at a time.
    /// </summary>
    public class UIViewController : MonoBehaviour 
    {
    	#region Public Variables
        public List<UIView> Views;
        public List<UIViewTransition> Transitions;

        public static UIViewController Instance = null;
    	#endregion

		#region Unity Methods
		void Awake()
		{
			OnAwake();	
		}
		void Start()
		{
            enabled = false;
			Activate();
		}
        void OnDestroy()
        {
            CleanUp();
        }
		#endregion

    	#region Methods
        public void Activate()
        {
            Instance = this;
            
            OnActivate();
            
            #if LOG
            Debug.Log(name + " activated.");
            #endif
        }
        
        public void Deactivate()
        {
            for(int i = 0; i < Views.Count; i++)
            {
                if(Views[i])
                    Views[i].Deactivate();
            }
            
            OnDeactivate();
            
            #if LOG
            Debug.Log(name + " deactivated.");
            #endif
        }
        
        public void CleanUp()
        {
            OnCleanUp();
        }

    	//Present view methods
        public static void ActivateUIView(UIView view,string animation="")
    	{
            view.Activate(animation);
			//view.Activated();

            Instance.OnPresent(view);

            //Debug.Log(instance._viewStack.Count);
    	}
    	public static void ActivateUIView(string view)
    	{
    		UIView temp = Instance.GetUIView(view);

            if (temp)
                ActivateUIView(temp);
    	}

    	//Remove view methods
        public static void DeactivateUIView(UIView view,string animation="")
    	{
            view.Deactivate(animation);

            Instance.OnRemove(view);
    	}
    	public static void DeactivateUIView(string view)
    	{
    		UIView temp = Instance.GetUIView(view);

            if (temp)
                DeactivateUIView(temp);
    	}

    	//Other methods
    	public static VisualElement GetElementFromView(string element, string view)
    	{
    		for(int i = 0; i < Instance.Views.Count; i++)
    		{
    			if(Instance.Views[i] && view == Instance.Views[i].name)
    				return Instance.Views[i].RetrieveUIElement(element);
    		}

    		return null;
    	}

    	public UIView GetUIView(string name)
    	{
    		for (int i = 0; i < Views.Count; i++)
    		{
    			if (Views[i] && Views[i].name == name)
    				return Views[i];
    		}

    		return null;
    	}

    	bool HasUIView(string name)
    	{
    		for (int i = 0; i < Views.Count; i++)
    			if (Views[i] && Views[i].name == name)
    				return true;

    		return false;
    	}

    	/// <summary>
    	/// Use this method to make the current UIViewController regain focus enabling
    	/// user interaction.
    	/// </summary>
    	public static  void GainFocus()
    	{
    		for(int i = 0; i < Instance.Views.Count; i++)
    		{
                if(Instance.Views[i])
    				Instance.Views[i].GainedFocus();
    		}
    	}

    	/// <summary>
    	/// Use this method to make the current UIViewController lose focus disabling 
    	/// user interaction.
    	/// </summary>
    	public static void LoseFocus()
    	{
    		for(int i = 0; i < Instance.Views.Count; i++)
    		{
                if(Instance.Views[i])
    				Instance.Views[i].LostFocus();
    		}
    	}

		public void AddViewToList(UIView view)
		{
			if(!Views.Contains(view))
				Views.Add(view);
		}
		public void RemoveViewFromList(UIView view)
		{
			Views.Remove(view);
		}

		public void SetupViewList()
		{
			UIView[] views = GetComponentsInChildren<UIView>();

			Views.Clear();

			for(int index = 0; index< views.Length; index++)
			{
				Views.Add(views[index]);
			}
		}
    	#endregion

        #region Virtual Methods
		protected virtual void OnAwake()
		{
			UIView view=null;
			for (int i = 0; i < Views.Count; i++)
			{
				view = Views[i];
				if(view != null)
					view.Initialize();
			}
		}
        protected virtual void OnActivate()
        {
			UIView view = null;
            for (int i = 0; i < Views.Count; i++)
            {
				view = Views[i];
				if (view != null && !view.HiddenByDefault)
				{
					view.Activate();
					//view.Activated();
				}
            }
        }
        protected virtual void OnDeactivate(){}
        protected virtual void OnCleanUp()
        {
            UIView view = null;
            for (int i = 0; i < Views.Count; i++)
            {
                view = Views[i];
                if (view != null && !view.gameObject.activeSelf)
                {
                    view.CleanUp();
                }
            }
        }

        
        protected virtual void OnPresent(UIView view){}
        protected virtual void OnRemove(UIView view){}
        #endregion
    }
}