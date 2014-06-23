using UnityEngine;
using System.Collections;

public class UIInputField : UIEffectText 
{
	#region Events 
	public static event System.Action clearKeyboards;

	public event System.Action<string> _textChanged;
	#endregion

	#region Public Variables
	public bool _secure = false;
	public bool _autoCorrect = true;
	public bool _multiLine = false;
	public bool _autoCapitalize = true;

	public int _characterLimit = -1;

	public TouchScreenKeyboardType _type;

	public string _defaultText;
	#endregion

	#region Private Variables
	bool _filtered;

	string _previousText;
	string _secureText;

	TouchScreenKeyboard _keyBoard;
	#endregion

	#region Unity Methods
	void Update()
	{
		if (_keyBoard != null) 
		{
			if(_keyBoard.active)
			{
				//_text = _keyBoard.text;

				if(_characterLimit > 0 && _keyBoard.text.Length > _characterLimit)
					_keyBoard.text = _keyBoard.text.Remove(_characterLimit);

				if(_secure && _secureText.Length != _effect.Text.Length)
					_secureText = new string('*', _effect.Text.Length);
			}
			else if(_keyBoard.done && !_keyBoard.wasCanceled && !_filtered)
			{
				if(_characterLimit > 0 && _keyBoard.text.Length > _characterLimit)
					_keyBoard.text = _keyBoard.text.Remove(_characterLimit);

				_effect.Text = _keyBoard.text;

				if(_effect.Text == "")
					_effect.Text = _defaultText;

				if(_textChanged != null && _effect.Text != _previousText)
					_textChanged(_effect.Text);

				_previousText = _effect.Text;

				//Filter();

				//Debug.Log("filtered");
			}
		}
	}
	#endregion

	#region Overriden Methods
	protected override void OnInit()
	{
		base.OnInit();

		if(_effect.Text == "")
			_effect.Text = _defaultText;

		_previousText = _effect.Text;

		if(_secure && _secureText.Length != _effect.Text.Length)
			_secureText = new string('*', _effect.Text.Length);
	}

	protected override void OnActivate(MovementState moveState)
	{
		base.OnActivate(moveState);

		clearKeyboards += Close;

		if(_button)
			_button.clickEvent += Open;
	}
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		Close();
		clearKeyboards -= Close;

		if(_button)
			_button.clickEvent -= Open;
	}
	#endregion

	#region Methods
	public void Open()
	{
		ClearKeyboard();
		_filtered = false;
		_keyBoard = TouchScreenKeyboard.Open(_effect.Text, _type,_autoCorrect,_multiLine,_secure);

		enabled = true;
	}

	public void Close()
	{
		//Debug.Log(name + "close keyboard");

		if(_keyBoard != null)
		{
			if(_secure)
				_effect.Text = _keyBoard.text;

			Filter();

			_keyBoard.active = false;
			_keyBoard = null;

			enabled = false;
		}
	}

	public static void ClearKeyboard()
	{
		if(clearKeyboards != null)
			clearKeyboards();
	}

	void Filter()
	{
		if(_autoCapitalize)
			_effect.Text = _effect.Text[0].ToString().ToLower() + _effect.Text.Substring(1);

		_filtered = true;
	}
	#endregion

	#region Type Methods
	public override System.Type GetBaseType()
	{
		return typeof(UIInputField);
	}
	#endregion
}
