using UnityEngine;
using System.Collections.Generic;

public class UIView : MonoBehaviour {

	#region Events
	//Fired when the screen is activated;
	public event System.Action activatedEvent;
	//Fired when the screen is deactivated;
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

	protected List<UILabel> labels;
	#endregion

	#region Unity Methods
	// Use this for initialization
	protected void Start () 
	{
		labels = new List<UILabel>();

		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null)
				{
					UIElements[i].Init(Transition.MovementIn);

					//If its not a label check to see if it has one
					if(UIElements[i].GetType() != typeof(UILabel))
					{
						UILabel[] labelComponents = UIElements[i].GetComponents<UILabel>();

						for(int j = 0; j < labelComponents.Length; j++)
						{
							labelComponents[j].Init(Transition.MovementIn);
							labels.Add(labelComponents[j]);
							UIElements.Add(labelComponents[j]);
						}
					}
					else
						labels.Add(UIElements[i] as UILabel);
				}
			}
		}

		if(Transition.Speed == 0)
			Transition.Speed = 1;

		enabled = false;
	}
	
	protected void Update()
	{
		if(movementState == MovementState.IN_PLACE)
			return;

		for(int i = 0; i < UIElements.Count; i++)
			if(UIElements[i] != null)
				UIElements[i].UpdateUI(Time.deltaTime, Transition.Speed);

		if(movementState == MovementState.EXITING)
		{
			if(HasUIExited())
				Deactivate();

		} else if(IsUIInPlace())
		{
			//If there are no labels you can disable the component
			//so that it doesn't update / draw unneccessarily
			if(HasNoLabels())
				enabled = false;
			movementState = MovementState.IN_PLACE;
		}
	}

	//Used to draw text
	void OnGUI()
	{
		useGUILayout = false;

		GUI.skin = UIViewController.Skin;

		for(int i = 0; i < labels.Count; i++)
			labels[i].Draw();
	}
	#endregion

	#region Activation, Deactivation Methods
	public void Activate()
	{
		if(enabled) return;

		//background = (Texture2D)Resources.Load(BackgroundName);

		Activation();

		enabled = true;

		if(activatedEvent != null)
			activatedEvent();
	}

	protected virtual void Activation()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null)
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

	protected virtual void Deactivation()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null)
				{
					UIElements[i].Deactivate();
				}
			}
		}
	}
	#endregion

	#region Interaction Methods
	public void DisableButtons()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null && UIElements[i].GetType() == typeof(UIButton))
				{
					UIButton button = UIElements[i] as UIButton;
					button.Disable();
				}
			}
		}
	}

	public void EnableButtons()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null && UIElements[i].GetType() == typeof(UIButton))
				{
					UIButton button = UIElements[i] as UIButton;
					button.Enable();
				}
			}
		}
	}
	#endregion

	#region Movement Methods
	public bool IsUIInPlace()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null && !UIElements[i].InPlace)
					return false;
			}
		}

		return true;
	}

	bool HasNoLabels()
	{
		return (labels.Count == 0);
	}
	#endregion

	#region Exit Methods
	public void FlagForExit()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null)
					UIElements[i].Exit(Transition.MovementOut);
			}
		}

		//If there are no labels make sure you enable the component
		//so that elements will move upon exit
		if(HasNoLabels())
			enabled = true;

		movementState = MovementState.EXITING;
	}

	bool HasUIExited()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null && !UIElements[i].Exited)
					return false;
			}
		}

		return true;
	}
	#endregion
}
