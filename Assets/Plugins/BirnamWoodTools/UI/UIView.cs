//#define LOG
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
	protected enum MovementState { INITIAL = 0, IN_PLACE, EXITING, EXITED }
	#endregion

	#region Constants
	const float CLOSE_ENOUGH = 30.0f;
	const float SPEED_MOD = 100.0f;
	#endregion

	#region Public Variables
	public bool _skipActivation;

	//used to determine if useGUILayout should be used
	//views that need to utilize GUI.depth should have this turned on
	public bool _useGUILayout;

	//lower numbers are drawn on top of higher numbers
	public int _depth;

	public Rect _viewRect;

	public List<UIBase> _elements;
	#endregion

	#region Protected Variables
	protected bool _transitioning;

	protected float movementRate;

	protected MovementState movementState;

	protected Vector2 _currentPosition;
	protected Vector2 _startPosition;

	protected Transition _currentTransition;
	#endregion

	#region Unity Methods
	// Use this for initialization
	protected void Awake () 
	{
		Initialize();

		enabled = false;

		if(!_useGUILayout)
			useGUILayout = false;
	}
	
	protected void Update()
	{
		if(movementState == MovementState.INITIAL)
		{
			movementRate = 1.0f / (_currentPosition - _currentTransition._targetPosition).magnitude;

			SetPosition(Vector2.Lerp(_currentPosition, _currentTransition._targetPosition, movementRate * _currentTransition._speed * SPEED_MOD * Time.deltaTime));

			if(Mathf.Abs((_currentPosition - _currentTransition._targetPosition).magnitude) <= CLOSE_ENOUGH)
			{
				SetPosition(_currentTransition._targetPosition);

				movementState = MovementState.IN_PLACE;

				if(transitionInEvent != null)
					transitionInEvent();
			}
		} else if(movementState == MovementState.EXITING)
		{
			movementRate = 1.0f / (_currentPosition - _currentTransition._targetPosition).magnitude;

			SetPosition(Vector2.Lerp(_currentPosition, _currentTransition._targetPosition, movementRate * _currentTransition._speed * SPEED_MOD * Time.deltaTime));

			if(Mathf.Abs((_currentPosition - _currentTransition._targetPosition).magnitude) <= CLOSE_ENOUGH)
			{
				SetPosition(_currentTransition._targetPosition);

				movementState = MovementState.EXITED;

				if(transitionOutEvent != null)
					transitionOutEvent();

				Deactivate();
			}
		}

		OnUpdate();
	}

	protected virtual void OnUpdate(){}
		
	void OnGUI()
	{
		GUI.depth = _depth;

		if(UINavigationController.Skin)
			GUI.skin = UINavigationController.Skin;

		GUI.BeginGroup(_viewRect);
		DrawContent();
		GUI.EndGroup();
	}

	/// <summary>
	/// Draws the content with in the view.
	/// </summary>
	protected virtual void DrawContent()
	{
		for(int i = 0; i < _elements.Count; i++)
		{
			if(_elements[i] && _elements[i].Active)
				_elements[i].Draw();
		}
	}

	#endregion

	#region Activation, Deactivation Methods
	protected virtual void Initialize()
	{
		UIScreen.AdjustForResolution(ref _viewRect);

		_startPosition.x = _viewRect.x;
		_startPosition.y = _viewRect.y;

		_currentPosition = _startPosition;

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

		_currentTransition = new Transition(_currentPosition, 1f);

		Activation();
	}

	public void Activate(Transition transition)
	{
		if(enabled)	return;

		_currentTransition = transition;
		_currentTransition._targetPosition.Scale(UIScreen.AspectRatio);

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

		if(activatedEvent != null)
			activatedEvent();

		SetPosition(_startPosition);

		movementState = MovementState.INITIAL;

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

		#if LOG
		Debug.Log(name + " deactivated.");
		#endif
	}
	#endregion

	#region Interaction Methods
	public virtual void LostFocus()
	{
		for(int i = 0; i < _elements.Count; i++)
		{
			if(_elements[i])
				_elements[i].Disable();
		}
	}

	public virtual void GainedFocus()
	{
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
	#endregion

	#region Position Methods
    /// <summary>
    /// Reposition the UI elements according to the newPosition.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <param name="animate">Determines if the UIView animates.</param>
    public void Reposition(Vector2 newPosition,bool animate=false)
    {
		Vector2 scale = new Vector2(newPosition.x / _viewRect.x,newPosition.y / _viewRect.y);

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
	protected virtual void InPlace()
	{
		enabled = false;

		movementState = MovementState.IN_PLACE;

		if(transitionInEvent != null)
			transitionInEvent();
	}

	void SetPosition(Vector2 position)
	{
		_currentPosition.x = position.x;
		_currentPosition.y = position.y;

		_viewRect.x = position.x;
		_viewRect.y = position.y;
	}
	#endregion

	#region Exit Methods
	public void FlagForExit()
	{
		_currentTransition = new Transition(_currentPosition, 1f);

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

		movementState = MovementState.EXITING;

		if(transitionOutEvent != null)
			transitionOutEvent();
	}

	public void FlagForExit(Transition transition)
	{
		_currentTransition = transition;

		Exit();
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
}