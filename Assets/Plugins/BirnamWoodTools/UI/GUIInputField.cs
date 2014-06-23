using UnityEngine;

public class GUIInputField : UIBase 
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
	public TouchScreenKeyboardType _type;

	public string _text;
	public string _defaultText;
	#endregion

	#region Private Variables
	bool _filtered;

	string _previousText;
	string _secureText;

	TouchScreenKeyboard _keyBoard;
	#endregion

	#region Overriden Methods
	protected override void OnInit()
	{
		base.OnInit();

		_primaryStyle.SetDefaultStyle("textfield");

		if(_text == "")
			_text = _defaultText;

		_previousText = _text;

		if(_secure && _secureText.Length != _text.Length)
			_secureText = new string('*', _text.Length);
	}

	protected override void OnActivate(MovementState state)
	{
		base.Activate(state);

		clearKeyboards += Close;
	}
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		Close();
		clearKeyboards -= Close;
	}
	#endregion

	#region Methods
	public void Close()
	{
		//Debug.Log(name + "close keyboard");

		if(_keyBoard != null)
		{
			if(_secure)
				_text = _keyBoard.text;

			Filter();

			_keyBoard.active = false;
			_keyBoard = null;
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
			_text = _text[0].ToString().ToLower() + _text.Substring(1);

		_filtered = true;
	}

	public override void Draw()
	{
		#if UNITY_EDITOR
		GUI.SetNextControlName(GetHashCode().ToString());

		if(_primaryStyle.custom)
			_text = GUI.TextField(_drawRect, _text, _primaryStyle.style);
		else
			_text = GUI.TextField(_drawRect, _text, _primaryStyle.styleName);

		if(GUI.GetNameOfFocusedControl() != name && _text == "")
			_text = _defaultText;
			
		if(_textChanged != null && GUI.GetNameOfFocusedControl() != GetHashCode().ToString())
		{
			if(_text != _previousText)
				_textChanged(_text);

			_previousText = _text;
		}
		#else
		if(GUI.Button(_drawRect, (_secure) ? _secureText : _text, (_primaryStyle.custom) ? _primaryStyle.style : _primaryStyle.styleName))
		{
			ClearKeyboard();
			_filtered = false;
			_keyBoard = TouchScreenKeyboard.Open(_text, _type,_autoCorrect,_multiLine,_secure);
		}

		if (_keyBoard != null) 
		{
			if(_keyBoard.active)
			{
				//_text = _keyBoard.text;

				if(_secure && _secureText.Length != _text.Length)
					_secureText = new string('*', _text.Length);
			}
			else if(_keyBoard.done && !_keyBoard.wasCanceled && !_filtered)
			{
				_text = _keyBoard.text;

				if(_text == "")
					_text = _defaultText;

				if(_textChanged != null && _text != _previousText)
					_textChanged(_text);

				_previousText = _text;

				Filter();

				//Debug.Log("filtered");
			}
		}
		#endif
	} 
	#endregion

	#region Type Methods
	public override System.Type GetBaseType()
	{
		return typeof(GUIInputField);
	}
	#endregion
}
