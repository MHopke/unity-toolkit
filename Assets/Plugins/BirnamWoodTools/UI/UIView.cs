using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Representation of a collection of UI elements.
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
	public Section section;

	public TransitionSettings Transition;

	public List<UIBase> UIElements;
	#endregion

	#region Protected Variables
	protected MovementState movementState;
	#endregion

	#region Unity Methods
	// Use this for initialization
	protected void Awake () 
	{
		if(Transition.Speed == 0)
			Transition.Speed = 1;

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
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i])
					UIElements[i].Init(Transition.MovementIn,Transition.Speed);
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
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] && !UIElements[i]._skipUIViewActivation)
					UIElements[i].Activate();
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
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i])
					UIElements[i].Deactivate();
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
		for(int i = 0; i < UIElements.Count; i++)
		{
			if(name == UIElements[i].name)
				return UIElements[i];
		}

		return null;
	}
	#endregion

	#region Movement Methods
	public bool IsUIInPlace()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] && !UIElements[i].InPlace)
					return false;
			}
		}

		return true;
	}
	#endregion

	#region Exit Methods
	public virtual void FlagForExit()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i])
					UIElements[i].Exit(Transition.MovementOut);
			}
		}

		enabled = true;

		movementState = MovementState.EXITING;

		if(transitionOutEvent != null)
			transitionOutEvent();
	}

	bool HasUIExited()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] && !UIElements[i].Exited)
					return false;
			}
		}

		return true;
	}
	#endregion
}
