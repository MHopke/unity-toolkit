using UnityEngine;

/// <summary>
/// The UI representation of a label. Uses OnGUI to render the text.
/// </summary>
public class UILabel : UIBase 
{
	#region Public Variables
	public int depth;

	public string text;
	#endregion

	#region Protected Variables
	protected bool _animating;

	protected float _originalFontSize;

	protected Vector2 _originalDimensions;

	protected Vector3 _scale;
	protected Vector3 _originalScale;
	#endregion

	#region Activation, Deactivation, Init Methods
	public override bool Init()
	{
		if(base.Init())
		{
			customStyle.SetDefaultStyle("label");

			_originalDimensions = new Vector2(_drawRect.width,_drawRect.height);

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
		_drawRect.x = currentPosition.x;
		_drawRect.y = currentPosition.y;
		_drawRect.width = _originalDimensions.x * _scale.x;
		_drawRect.height = _originalDimensions.y * _scale.y;
	}
	#endregion

	#region Draw Methods
	public override void Draw()
	{
		useGUILayout = false;

		GUI.skin = UINavigationController.Skin;

		GUI.depth = depth;

		if(customStyle.custom)
			GUI.Label(_drawRect, text, customStyle.style);
		else
			GUI.Label(_drawRect, text, customStyle.styleName);
	}
	#endregion

	#region Position Methods
	protected override void SetPosition(Vector2 position)
	{
		base.SetPosition(position);

		_drawRect.x = position.x;
		_drawRect.y = position.y;
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
}
