using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is a collection of UI elements. Each UIView
/// has a position and size. Each element's position & size
/// are relative to the UIView's position & size.
/// </summary>
public class UIView : MonoBehaviour {

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

	#region Enums
	public enum Section { NONE = 0, HEADER, CONTENT, FOOTER }
	protected enum MovementState { INITIAL = 0, IN_PLACE, EXITING }
	#endregion

	#region Public Variables
    public Vector2 _position;
    public Vector2 _size;

	public List<UIBase> _elements;
	#endregion

	#region Protected Variables
	protected MovementState movementState;
	#endregion

	#region Unity Methods
	// Use this for initialization
	protected void Awake () 
	{
		Initialize();

		enabled = false;
	}
	
	protected void Update()
	{
		if(movementState == MovementState.EXITING)
		{
			if(HasUIExited())
				Deactivate();

		} else if(IsUIInPlace())
		{
			enabled = false;

			movementState = MovementState.IN_PLACE;

			if(transitionInEvent != null)
				transitionInEvent();
		}
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

		enabled = true;

		if(activatedEvent != null)
			activatedEvent();

		//Debug.Log(name + " activate");
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

		movementState = MovementState.INITIAL;
	}

	public void Deactivate() 
	{
		if(!enabled)
			return;

		//Debug.Log(name + " deactivate");

		Deactivation();

		enabled = false;

		if(deactivatedEvent != null)
			deactivatedEvent();
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
	}
	#endregion

	#region Interaction Methods
	public virtual void LostFocus()
	{
		UIButton.DisableUIButtons();
	}

	public virtual void GainedFocus()
	{
		UIButton.EnableUIButtons();
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
	#endregion

	#region Position Methods
    /// <summary>
    /// Reposition the UI elements according to the newPosition.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <param name="animate">Determines if the UIView animates.</param>
    public void Reposition(Vector2 newPosition,bool animate=false)
    {
        Vector2 scale = new Vector2(newPosition.x / _position.x,newPosition.y / _position.y);

        if (_elements != null)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i])
                    _elements[i].Reposition(scale);
            }
        }
    }

	public bool IsUIInPlace()
	{
		if(_elements != null)
		{
			for(int i = 0; i < _elements.Count; i++)
			{
				if(_elements[i] && !_elements[i].InPlace)
					return false;
			}
		}

		return true;
	}
	#endregion

	#region Exit Methods
	public virtual void FlagForExit()
	{
		if(_elements != null)
		{
			for(int i = 0; i < _elements.Count; i++)
			{
				if(_elements[i])
					_elements[i].Exit();
			}
		}

		enabled = true;

		movementState = MovementState.EXITING;

		if(transitionOutEvent != null)
			transitionOutEvent();
	}

	bool HasUIExited()
	{
		if(_elements != null)
		{
			for(int i = 0; i < _elements.Count; i++)
			{
				if(_elements[i] && !_elements[i].HasExited)
					return false;
			}
		}

		return true;
	}
	#endregion
}
