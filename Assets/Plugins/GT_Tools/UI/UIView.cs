//#define LOG
using UnityEngine;
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

    	public bool _skipActivation;

    	public ScreenSetting _screenSetting;

    	public List<UIBase> _elements;
    	#endregion

    	#region Unity Methods
    	// Use this for initialization
    	protected void Awake () 
    	{
    		if(_useHierarchy)
    		{
    			UIBase[] ui = GetComponentsInChildren<UIBase>();

    			for(int i = 0; i < ui.Length; i++)
    				_elements.Add(ui[i]);
    		}

    		Initialize();

    		enabled = false;
    	}
    	#endregion

    	#region Activation, Deactivation Methods
    	protected virtual void Initialize()
    	{
    		if(_elements != null)
    		{
    			for(int i = 0; i < _elements.Count; i++)
    			{
    				if(_elements[i])
    					_elements[i].Init();
    			}
    		}
    	}

    	public void Activate()
    	{
    		if(enabled) return;

    		//background = (Texture2D)Resources.Load(BackgroundName);

    		Activation();
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

    		enabled = true;

            ActivateEvent();

    		#if LOG
    		Debug.Log(name + " activated.");
    		#endif
    	}

    	public void Deactivate() 
    	{
    		if(!enabled)
    			return;

    		//Debug.Log(name + " deactivate");

    		Deactivation();

    		enabled = false;

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
    	#endregion

    	#region Interaction Methods
    	public virtual void LostFocus()
    	{
    		#if LOG
    		Debug.Log(name + " lost focus");
    		#endif

    		for(int i = 0; i < _elements.Count; i++)
    		{
    			if(_elements[i])
    				_elements[i].Disable();
    		}
    	}

    	public virtual void GainedFocus()
    	{
    		#if LOG
    		Debug.Log(name + " gained focus");
    		#endif

    		for(int i = 0; i < _elements.Count; i++)
    		{
    			if(_elements[i])
    				_elements[i].Enable();
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

    	bool HasUIExited()
    	{
    		if(_elements != null)
    		{
    			for(int i = 0; i < _elements.Count; i++)
    			{
    				if(_elements[i] && !_elements[i].HasExited)
    					#if LOG
    					{
    						Debug.Log(_elements[i].name);
    						return false;
    					}
    					#else
    					return false;
    					#endif
    			}
    		}

    		return true;
    	}
    	#endregion

        #region Event Call Methods
        protected void ActivateEvent()
        {
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
    }
}