using UnityEngine;

public class UIToggle : UIBase 
{
	#region Events
	public event System.Action<bool> toggledEvent;
	#endregion

	#region Enumerations
	public enum ToggleType { IMAGE = 0, TEXT, BOTH }
	#endregion

	#region Public Variables
	public bool _registerChanges;

	public string _textOn;
	public string _textOff;

	public ToggleType _type;

	public Rect _textRect;

	public Texture2D _textureOn;
	public Texture2D _textureOff;

	public CustomStyle _textStyle;
	#endregion

	#region Private Variables
	bool _toggled = true;
	bool _previousState;
	#endregion

	#region Control Methods
	protected override void OnInit()
	{
		base.OnInit();

		_textStyle.SetDefaultStyle("label");
	}
	protected override void OnActivate(MovementState state)
	{
		base.OnActivate(state);

		if(_registerChanges)
			enabled = true;
	}
	protected override void OnDeactivate()
	{
		base.OnDeactivate();

		if(_registerChanges)
			enabled = false;
	}
	#endregion

	#region Update Methods
	void Update()
	{
		if(_registerChanges)
		{
			if(_previousState != _toggled && toggledEvent != null)
				toggledEvent(_toggled);

			_previousState = _toggled;
		}
	}
	#endregion

	#region Overriden Methods
	public override void Draw()
	{
		if(_type == ToggleType.IMAGE)
		{
			_toggled = GUI.Toggle(_drawRect, _toggled, (_toggled) ? _textureOn : _textureOff,_primaryStyle.style);
		} else if(_type == ToggleType.TEXT)
		{
			_toggled = GUI.Toggle(_drawRect, _toggled, (_toggled) ? _textOn : _textOff, _textStyle.style);
		} else
		{
			_toggled = GUI.Toggle(_drawRect, _toggled, (_toggled) ? _textureOn : _textureOff);
			_toggled = GUI.Toggle(_drawRect, _toggled, (_toggled) ? _textOn : _textOff, _textStyle.style );
		}
	}
	#endregion

	#region Accessors
	public bool Toggled
	{
		get { return _toggled; }
		set { _toggled = value; }
	}
	#endregion
}
