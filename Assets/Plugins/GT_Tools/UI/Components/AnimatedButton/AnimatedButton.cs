using UnityEngine;

namespace gametheory.UI
{
    public class AnimatedButton : ButtonComponent 
    {
    	#region Public Variables
    	public bool _switchOnExit;
    	/*public bool _recalculateHighlight;
    	public bool _deactivatedState;

    	public float _alpha;
    	public float _downShift;*/

        public AnimatedButtonAnimation _animation;

    	public Animator _animator;

    	public AnimatedButton _partnerButton;
    	#endregion

    	#region Private Variables
    	/*float _originalAlpha;

    	Rect _highlightRect;

    	static Color _deactivated;

    	Texture2D _highlight;*/
    	#endregion

    	#region Unity Methods
    	void Start()
    	{
            if (_animation)
                _animation.Initialize();

    		/*if(_downStateType != DownStateType.NONE)
    		{
    			_renderer = renderer as SpriteRenderer;

    			if(_renderer)
    			{
    				_originalSprite = _renderer.sprite;
    				_originalAlpha = _renderer.color.a;
    			}
    		}

    		_deactivated = new Color(1f, 1f, 1f,0.3f);*/

    		enabled = false;
    	}

    	void OnGUI()
    	{
    		//GUI.DrawTexture(_highlightRect, _highlight);
    	}
    	#endregion

    	#region Activate/Deactivate
    	protected override void OnActivate()
    	{
    		base.OnActivate();

    		/*if(_deactivatedState)
    			_renderer.color = Color.white;*/
    	}

    	protected override void OnDeactivate()
    	{
    		base.OnDeactivate();

    		/*if(_deactivatedState)
    			_renderer.color = _deactivated;*/

    		if(enabled)
    			enabled = false;

    		SetToNormal();
    	}
    	#endregion

    	#region Click Methods
    	void Click()
    	{
    		FireClickEvent();
    	}

    	protected override void SendClickEvent()
    	{
    		if(_animator && _animator.runtimeAnimatorController)
    			_animator.SetTrigger("Click");
    		else
    			Click();
    	}
    	#endregion

    	#region Animation Methods
    	public void SetToPressed()
    	{
            if (_animation)
                _animation.Animate();

    		/*switch(_downStateType)
    		{
    		case DownStateType.HIGHLIGHT:
    			transform.SetXYScale(_originalScale.x * _downScale, _originalScale.y * _downScale);

    			if(_highlight == null)
    			{
    				CalculateRect();

    				_highlight = new Texture2D(1, 1);
    				_highlight.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.2f));
    				_highlight.Apply();
    			}
    			else if(_recalculateHighlight)
    				CalculateRect();

    			enabled = true;
    			break;
    		case DownStateType.ALPHA:
    			transform.SetXYScale(_originalScale.x * _downScale, _originalScale.y * _downScale);
    			if(_renderer != null)
    				_renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, _originalAlpha * _alpha);
    			break;
    		case DownStateType.DOWN_SHIFT:
    			transform.AddYPosition(_downShift);

    			if(_pressedSprite)
    				_renderer.sprite = _pressedSprite;
    			break;
    		}*/
    	}
    	public void SetToNormal()
    	{
            if (_animation)
                _animation.Restore();

    		/*switch(_downStateType)
    		{
    		case DownStateType.HIGHLIGHT:
    			enabled = false;
    			transform.SetXYScale(_originalScale.x, _originalScale.y);
    			break;
    		case DownStateType.ALPHA:
    			transform.SetXYScale(_originalScale.x, _originalScale.y);
    			if (_renderer != null)
    				_renderer.color = new Color(_renderer.color.r,_renderer.color.g,_renderer.color.b, _originalAlpha);
    			break;
    		case DownStateType.DOWN_SHIFT:
    			transform.AddYPosition(-_downShift);

    			if(_originalSprite)
    				_renderer.sprite = _originalSprite;
    			break;
    		}*/
    	}

    	void CalculateRect()
    	{
            /*Vector2 pos = UIScreen.UICamera.WorldToScreenPoint(transform.position);
            Vector2 max = UIScreen.UICamera.WorldToScreenPoint(renderer.bounds.max);
            Vector2 min = UIScreen.UICamera.WorldToScreenPoint(renderer.bounds.min);

    		_highlightRect = new Rect(min.x, Screen.height - pos.y - (max.y - min.y) / 2f, max.x - min.x, (max.y - min.y));*/
    	}
    	#endregion

    	#region Input Methods
    	protected override void ButtonDown()
    	{
            SetToPressed();

    		if(_partnerButton)
    			_partnerButton.SetToPressed();
    	}
    	protected override void MovedOffButton()
    	{
    		SetToNormal();

    		if(_partnerButton)
    			_partnerButton.SetToNormal();
    	}
    	protected override void ButtonUp()
    	{
    		SetToNormal();

    		if(_partnerButton)
    			_partnerButton.SetToNormal();
    	}
    	#endregion

    	#region Other Methods
    	public void SetOriginalScale()
    	{
    		//_originalScale = transform.localScale;
    	}
    	#endregion
    }
}
