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
        public List<UIView> _views;
        public static UIViewController Instance = null;
    	#endregion

    	#region Activate, Deactivate Methods
        public void Initialize(bool currentViewController)
        {
            for (int i = 0; i < _views.Count; i++)
                _views[i].Initialize(currentViewController);

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
            for (int i = 0; i < _views.Count; i++)
            {
                if (!_views[i]._skipActivation)
                    _views[i].Activate();
            }
        }

    	public void Deactivate()
    	{
    		for(int i = 0; i < _views.Count; i++)
    		{
    			if(_views[i])
                    _views[i].Deactivate();
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
    	public static void PresentUIView(UIView view)
    	{
            view.Activate();

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
    	public static void RemoveUIView(UIView view)
    	{
            view.Deactivate();

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
    	public static UIBase GetElementFromView(string element, string view)
    	{
    		for(int i = 0; i < Instance._views.Count; i++)
    		{
    			if(Instance._views[i] && view == Instance._views[i].name)
    				return Instance._views[i].RetrieveUIElement(element);
    		}

    		return null;
    	}

    	public UIView GetUIView(string name)
    	{
    		for (int i = 0; i < _views.Count; i++)
    		{
    			if (_views[i] && _views[i].name == name)
    				return _views[i];
    		}

    		return null;
    	}

    	bool HasUIView(string name)
    	{
    		for (int i = 0; i < _views.Count; i++)
    			if (_views[i] && _views[i].name == name)
    				return true;

    		return false;
    	}

    	/// <summary>
    	/// Use this method to make the current UIViewController regain focus enabling
    	/// user interaction.
    	/// </summary>
    	public static  void GainFocus()
    	{
    		for(int i = 0; i < Instance._views.Count; i++)
    		{
                if(Instance._views[i] && Instance._views[i].Active)
    				Instance._views[i].GainedFocus();
    		}
    	}

    	/// <summary>
    	/// Use this method to make the current UIViewController lose focus disabling 
    	/// user interaction.
    	/// </summary>
    	public static void LoseFocus()
    	{
    		for(int i = 0; i < Instance._views.Count; i++)
    		{
                if(Instance._views[i] && Instance._views[i].Active)
    				Instance._views[i].LostFocus();
    		}
    	}
    	#endregion
    }
}