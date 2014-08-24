using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    public class UICustomKeyboard : UIEffectText 
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

        public ScreenSetting _screenSetting;
        public ButtonComponent _button;

    #if UNITY_IOS
    	public TouchScreenKeyboardType _type;
    #endif

    	public string _defaultText;
    	#endregion

    	#region Private Variables
    	bool _filtered;

    	string _previousText;
    	string _secureText;
    #if UNITY_IOS
    	TouchScreenKeyboard _keyBoard;
    #endif
    	#endregion

    	#region Unity Methods

        #if UNITY_IOS
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
        #endif
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

    		#if !UNITY_IOS || UNITY_EDITOR
            UIScreen.AdjustRect(ref _rect,_screenSetting);

    		_rect.x = Screen.width / 2f - _rect.width / 2f;

            _guiStyle.SetDefaultStyle("textfield");
    		#endif
    	}

    	protected override void OnActivate()
    	{
    		base.OnActivate();

    		clearKeyboards += Close;

    		if(_button)
    			_button.clickEvent += Open;

    		#if !UNITY_IOS || UNITY_EDITOR
    		renderer.enabled = false;
    		#endif
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
    #if UNITY_IOS
    		_keyBoard = TouchScreenKeyboard.Open(_effect.Text, _type,_autoCorrect,_multiLine,_secure);
    #endif

    		enabled = true;
    	}

    	public void Close()
    	{
    		//Debug.Log(name + "close keyboard");

    		#if UNITY_IOS
    		if(_keyBoard != null)
    		{
    			if(_secure)
    				_effect.Text = _keyBoard.text;

    			Filter();

    			_keyBoard.active = false;
    			_keyBoard = null;

    			enabled = false;
    		}
    #endif
    	}

    	public void ResetText()
    	{
    		_effect.Text = _defaultText;
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
    		return typeof(UICustomKeyboard);
    	}
    	#endregion

    	#if !UNITY_IOS || UNITY_EDITOR
    	string _text="";

        public Rect _rect;

        public CustomStyle _guiStyle;

    	void OnGUI()
    	{
    		GUI.SetNextControlName(GetHashCode().ToString());
    		
            if(_guiStyle.custom)
                _effect.Text = GUI.TextField(_rect, _effect.Text, _guiStyle.style);
    		else
                _effect.Text = GUI.TextField(_rect, _effect.Text, _guiStyle.styleName);
    		
    		if(GUI.GetNameOfFocusedControl() != name && _text == "")
    			_text = _defaultText;
    		
    		if(_textChanged != null && GUI.GetNameOfFocusedControl() != GetHashCode().ToString())
    		{
    			if(_text != _previousText)
    				_textChanged(_text);
    			
    			_previousText = _text;
    		}
    	}
    	#endif
    }
}
