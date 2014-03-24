using UnityEngine;

public class UIToggle : UITexture 
{
	#region Events
	public event System.Action<bool> toggledEvent;
	#endregion

	#region Public Variables
	public GUIContent _content;
	#endregion

	#region Private Variables
	bool _toggled;

	Sprite _spriteOff;
	#endregion

	#region Overriden Methods
	public override void Draw()
	{
		_toggled = GUI.Toggle(_drawRect, _toggled, _content);
	}
	#endregion
}
