using UnityEngine;

public class AnimatedButton : ButtonComponent 
{
	#region Enumerations
	public enum DownStateType {NONE = 0, SPRITE, HIGHLIGHT, ALPHA, SCALE };
	#endregion

	#region Public Variables
	public bool _switchOnExit;
	public bool _recalculateHighlight;

	public float _downScale;
	public float _alpha;

	public DownStateType _downStateType;

	public Sprite _pressedSprite;
	public Animator _animator;
	#endregion

	#region Private Variables
	float _originalAlpha;

	Vector3 _originalScale;

	Rect _highlightRect;

	Texture2D _highlight;
	Texture2D _nonFunctional;

	Sprite _originalSprite;
	SpriteRenderer _renderer;
	#endregion

	#region Unity Methods
	void Start()
	{
		_originalScale = transform.localScale;

		if(_downStateType == DownStateType.HIGHLIGHT || _downStateType == DownStateType.ALPHA)
		{
			_renderer = renderer as SpriteRenderer;
			_originalSprite = _renderer.sprite;
			_originalAlpha = _renderer.color.a;
		}

		if(_nonFunctional == null)
		{
			_nonFunctional = new Texture2D(1, 1);
			_nonFunctional.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.3f));
			_nonFunctional.Apply();
		}

		enabled = false;
	}

	void OnGUI()
	{
		GUI.DrawTexture(_highlightRect, _highlight);
	}
	#endregion

	#region Activate/Deactivate
	protected override void OnDeactivate()
	{
		base.OnDeactivate();

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
		if(_downStateType == DownStateType.SPRITE)
			_renderer.sprite = _pressedSprite;
		else if(_downStateType == DownStateType.HIGHLIGHT || _downStateType == DownStateType.SCALE)
		{
			transform.SetScaleXY(_originalScale.x * _downScale, _originalScale.y * _downScale);

			if(_highlight == null)
			{
				Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
				Vector2 max = Camera.main.WorldToScreenPoint(renderer.bounds.max);
				Vector2 min = Camera.main.WorldToScreenPoint(renderer.bounds.min);

				_highlightRect = new Rect(min.x, Screen.height - pos.y - (max.y - min.y) / 2f, max.x - min.x, (max.y - min.y));

				_highlight = new Texture2D(1, 1);
				_highlight.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.2f));
				_highlight.Apply();
			}
			else if(_recalculateHighlight)
			{
				Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
				Vector2 max = Camera.main.WorldToScreenPoint(renderer.bounds.max);
				Vector2 min = Camera.main.WorldToScreenPoint(renderer.bounds.min);

				_highlightRect = new Rect(min.x, Screen.height - pos.y - (max.y - min.y) / 2f, max.x - min.x, (max.y - min.y));
			}

			if(_downStateType == DownStateType.HIGHLIGHT)
				enabled = true;
		}
		else if(_downStateType == DownStateType.ALPHA)
		{
			transform.SetScaleXY(_originalScale.x * _downScale, _originalScale.y * _downScale);
			if (_renderer != null)
				_renderer.color = new Color(_renderer.color.r,_renderer.color.g,_renderer.color.b, _originalAlpha * _alpha);
		}
	}
	public void SetToNormal()
	{
		if(_downStateType == DownStateType.SPRITE)
			_renderer.sprite = _originalSprite;
		else if(_downStateType == DownStateType.HIGHLIGHT || _downStateType == DownStateType.SCALE)
		{
			if(_downStateType == DownStateType.HIGHLIGHT)
				enabled = false;

			transform.SetScaleXY(_originalScale.x, _originalScale.y);
		}
		else if(_downStateType == DownStateType.ALPHA)
		{
			transform.SetScaleXY(_originalScale.x, _originalScale.y);
			if (_renderer != null)
				_renderer.color = new Color(_renderer.color.r,_renderer.color.g,_renderer.color.b, _originalAlpha);
		}
	}
	#endregion

	#region Input Methods
	protected override void ButtonDown()
	{
		SetToPressed();
	}
	protected override void MovedOffButton()
	{
		SetToNormal();
	}
	protected override void ButtonUp()
	{
		SetToNormal();
	}
	#endregion
}
