using UnityEngine;

public class UIButton : UIBase 
{
	#region Events
	public event System.Action clickEvent;
	#endregion

	#region Enumerations
	public enum ButtonType { IMAGE = 0, TEXT, BOTH }
	#endregion

	#region Public Variables
	public string _text;

	public ButtonType _type;

	public Rect _textRect;

	public Texture2D _texture;
	#endregion

	#region Init Methods
	protected override void OnInit()
	{
		base.OnInit();

		if(_type == ButtonType.BOTH)
			UIScreen.AdjustForResolution(ref _textRect);

		_primaryStyle.SetDefaultStyle("button");
	}
	#endregion

	#region Draw Methods
	public override void Draw()
	{
		if(_type == ButtonType.IMAGE)
		{
			if(GUI.Button(_drawRect, _texture,GUIStyle.none) && !_disabled)
				FireClickEvent();
		} else if(_type == ButtonType.TEXT)
		{
			if(GUI.Button(_textRect, _text,(_primaryStyle.custom) ? _primaryStyle.style : _primaryStyle.styleName) && !_disabled)
				FireClickEvent();
		} else
		{
			if((GUI.Button(_drawRect, _texture,GUIStyle.none) || GUI.Button(_textRect, _text,(_primaryStyle.custom) ? _primaryStyle.style : _primaryStyle.styleName)) && !_disabled)
				FireClickEvent();
		}
	}
	#endregion

	#region Click Methods
	void FireClickEvent()
	{
		if(clickEvent != null)
			clickEvent();

		InputHandler.MarkInput();
	}
	#endregion
}
