using UnityEngine;

/// <summary>
/// The UI representation of a label. Uses OnGUI to render the text.
/// </summary>
public class UILabel : UIBase 
{
	#region Public Variables
	public int depth;

	public string text;

	public Vector2 size;

	public CustomStyle customStyle;
	#endregion

	#region Protected Variables
	protected bool _animating;

	protected float _originalFontSize;

	protected Vector2 _originalDimensions;

	protected Vector3 _scale;
	protected Vector3 _originalScale;

	protected Rect drawRect;
	#endregion

	#region Activation, Deactivation, Init Methods
	public override bool Init()
	{
		if(base.Init())
		{
			customStyle.SetDefaultStyle("label");

			size.Scale(UIScreen.AspectRatio);

			_originalDimensions = size;

			if(customStyle.custom)
				_originalFontSize = customStyle.style.fontSize;
			else
			{
				if(UINavigationController.Skin)
					_originalFontSize = UINavigationController.Skin.FindStyle(customStyle.styleName).fontSize;
				else
					_originalFontSize = 1;
			}

			_originalScale = transform.localScale;

			drawRect = new Rect(position.x, position.y, size.x, size.y);
			return true;
		} else
			return false;
	}
	public override bool Activate(MovementState state = MovementState.INITIAL)
	{
		if(base.Activate(state))
		{
			enabled = true;
			return true;
		}
		else
			return false;
	}
	public override bool DelayedActivation(bool skipTransition = false)
	{
		if(base.DelayedActivation(skipTransition))
		{
			enabled = true;
			return true;
		} else
			return false;
	}
	public override bool Deactivate(bool force =false)
	{
		if(base.Deactivate(force))
		{
			enabled = false;
			return true;
		} else
			return false;
	}
	#endregion

	#region Update
	void Update()
	{
		if(!_animating)
			return;

		//determine the scale
		_scale.x = transform.localScale.x / _originalScale.x;
		_scale.y = transform.localScale.y / _originalScale.y;
		_scale.z = transform.localScale.z / _originalScale.z;

		//scale font size
		customStyle.style.fontSize = (int)(_originalFontSize * _scale.x);

		//Get the new position
		currentPosition = Camera.main.WorldToScreenPoint(transform.position);

		//Set the draw rect
		drawRect.x = currentPosition.x;
		drawRect.y = currentPosition.y;
		drawRect.width = _originalDimensions.x * _scale.x;
		drawRect.height = _originalDimensions.y * _scale.y;
	}
	#endregion

	#region Draw Methods
	protected virtual void OnGUI()
	{
		useGUILayout = false;

		GUI.skin = UINavigationController.Skin;

		GUI.depth = depth;

		if(customStyle.custom)
			GUI.Label(drawRect, text, customStyle.style);
		else
			GUI.Label(drawRect, text, customStyle.styleName);
	}
	#endregion

	#region Position Methods
	protected override void SetPosition(Vector2 position)
	{
		base.SetPosition(position);

		drawRect.x = position.x;
		drawRect.y = position.y;
	}
	#endregion

	#region Style Methods
	/// <summary>
	/// Creates a custom style if the UILabel previously wasn't using one.
	/// </summary>
	public void CreateCustomStyle()
	{
		if(!customStyle.custom)
		{
			customStyle.custom = true;

			customStyle.style = new GUIStyle(UINavigationController.Skin.FindStyle(customStyle.styleName));

			_originalFontSize = customStyle.style.fontSize;
		}
	}
	#endregion

	#region Type Methods
	public override System.Type GetBaseType()
	{
		return typeof(UILabel);
	}
	#endregion

	#region Animation Methods
	protected override void SetTrigger(string triggerName)
	{
		if(_animator && _animator.runtimeAnimatorController)
		{
			CreateCustomStyle();
			_animator.SetTrigger(triggerName);
			AnimationStarted();
		}
	}
	protected override void Activated()
	{
		base.Activated();

		AnimationEnded();
	}
	public override void Exited()
	{
		base.Exited();

		AnimationEnded();
	}

	void AnimationStarted()
	{
		_animating = true;
	}
	void AnimationEnded()
	{
		_animating = false;
	}
	#endregion

	#region Color Methods
	protected override Color GetColor()
	{
		return customStyle.style.normal.textColor;
	}
	protected override void SetColor(Color color)
	{
		customStyle.style.normal.textColor = color;
	}
	#endregion

	#region Accessors
	public override Rect GetBounds()
	{
		return new Rect(currentPosition.x, currentPosition.y, size.x, size.y);
	}
	#endregion
}
