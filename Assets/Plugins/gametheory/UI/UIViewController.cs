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
			Initialize();
			Activate();
		}
		#endregion

    	#region Activate, Deactivate Methods
        public void Initialize()
        {
            for (int i = 0; i < Views.Count; i++)
                Views[i].Initialize();

            OnInit();
        }
        protected virtual void OnInit(){}

        public void Activate()
    	{
            Instance = this;

            OnActivate();

    		#if LOG
    		Debug.Log(name + " activated.");
    		#endif
    	}

        protected virtual void OnActivate()
        {
            for (int i = 0; i < Views.Count; i++)
            {
                if (!Views[i].SkipActivation)
                    Views[i].Activate();
            }
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

        protected virtual void OnDeactivate(){}

        public void CleanUp()
        {
            OnCleanUp();
        }

        protected virtual void OnCleanUp(){}
    	#endregion

    	#region Methods
    	//Present view methods
        public static void PresentUIView(UIView view,string animation="")
    	{
            view.Activate(animation);

            Instance.OnPresent(view);

            //Debug.Log(instance._viewStack.Count);
    	}
    	public static void PresentUIView(string view)
    	{
    		UIView temp = Instance.GetUIView(view);

            if (temp)
                PresentUIView(temp);
    	}
        protected virtual void OnPresent(UIView view){}

    	//Remove view methods
        public static void RemoveUIView(UIView view,string animation="")
    	{
            view.Deactivate(animation);

            Instance.OnRemove(view);
    	}
    	public static void RemoveUIView(string view)
    	{
    		UIView temp = Instance.GetUIView(view);

            if (temp)
                RemoveUIView(temp);
    	}
        protected virtual void OnRemove(UIView view){}

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
                if(Instance.Views[i] && Instance.Views[i].Active)
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
                if(Instance.Views[i] && Instance.Views[i].Active)
    				Instance.Views[i].LostFocus();
    		}
    	}
    	#endregion
    }
}