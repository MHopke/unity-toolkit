using UnityEngine;

public class UIToggle : UISprite 
{
	#region Events
	public event System.Action<bool> toggledEvent;
	#endregion

	#region Public Variables
	public Sprite _spriteOn;
	#endregion

	#region Private Variables
	bool _toggled;

	Sprite _spriteOff;
	#endregion

	#region Overriden Methods
	public override bool Init()
	{
		if(base.Init())
		{

			if(_uiButton)
				_uiButton.clickEvent += ToggleImage;

			_spriteOff = _spriteRenderer.sprite;

			return true;
		} else
			return false;
	}
	#endregion

	#region Methods
	void ToggleImage()
	{
		if(_toggled)
		{
			_spriteRenderer.sprite = _spriteOff;
			_toggled = false;
		} else
		{
			_toggled = true;
			_spriteRenderer.sprite = _spriteOn;
		}

		if(toggledEvent != null)
			toggledEvent(_toggled);
	}

	public void Toggle(bool toggle)
	{
		if(toggle)
		{
			_spriteRenderer.sprite = _spriteOn;
			_toggled = true;
		} else
		{
			_toggled = false;
			_spriteRenderer.sprite = _spriteOff;
		}
	}
	#endregion
}
