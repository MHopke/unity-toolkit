using UnityEngine;

public class UIInputField : UIBase 
{
	#region Events 
	public event System.Action<string> _textChanged;
	#endregion

	#region Public Variables
	public string _text;
	#endregion

	public override bool Init()
	{
		if(base.Init())
		{
			_primaryStyle.SetDefaultStyle("textfield");

			return true;
		} else
			return false;
	}

	#region Draw Methods
	public override void Draw()
	{
		if(_primaryStyle.custom)
			_text = GUI.TextField(_drawRect, _text, _primaryStyle.style);
		else
			_text = GUI.TextField(_drawRect, _text, _primaryStyle.styleName);
	} 
	#endregion
}
