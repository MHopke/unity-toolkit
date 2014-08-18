using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// Used to make any Sprite act as toggle.
    /// </summary>
    public class ToggleComponent : ButtonComponent
    {
        #region Events
        public event System.Action<bool> toggled;
        #endregion

        #region Public Variables
        public Sprite _spriteOn;
    	#endregion

    	#region Private Variables
    	bool _toggled;

    	Sprite _spriteOff;

    	SpriteRenderer _renderer;
    	#endregion

    	#region Unity Methods
    	void Start()
    	{
    		_renderer = GetComponent<SpriteRenderer>();

    		_spriteOff = _renderer.sprite;
    	}
    	#endregion

    	#region Methods
    	protected override void SendClickEvent()
    	{
    		ToggleImage();

            if (toggled != null)
                toggled(_toggled);

    		//base.SendClickEvent();
    	}

    	void ToggleImage()
    	{
    		if(_toggled)
    		{
    			_renderer.sprite = _spriteOff;
    			_toggled = false;
    		} else
    		{
    			_toggled = true;
    			_renderer.sprite = _spriteOn;
    		}
    	}

    	public void Toggle(bool toggle)
    	{
    		if(toggle)
    		{
    			_renderer.sprite = _spriteOn;
    			_toggled = true;
    		} else
    		{
    			_toggled = false;
    			_renderer.sprite = _spriteOff;
    		}
    	}
    	#endregion
    }
}