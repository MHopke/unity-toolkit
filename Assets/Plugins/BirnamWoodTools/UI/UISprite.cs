//#define DRAW
using UnityEngine;

/// <summary>
/// Representation of a graphical UI element. Uses Unity 4.3 Sprite System.
/// </summary>
public class UISprite : UIBase
{
	#region Public Variables
	public bool _hasRoot;

	public float _zDepth = 3;
	#endregion

	#region Protected Variables
	protected SpriteRenderer _spriteRenderer;

	protected Animator _spriteAnimator;
	#endregion

	#region Activation, Deactivation, Init Methods
	protected override void OnInit()
	{
		base.OnInit();

		_spriteRenderer = GetComponent<SpriteRenderer>();

		_spriteAnimator = GetComponent<Animator>();

		_spriteRenderer.enabled = false;
	}
	#endregion

	#region Type Methods
	public override System.Type GetBaseType()
	{
		return typeof(UISprite);
	}
	#endregion

	#if DRAW
	public override void Draw()
	{
		GUI.Box(_drawRect, "");
		//base.Draw();
	}
	#endif

	#region Position Method
	protected override void SetPosition(Vector2 position)
	{
		base.SetPosition(position);

		if(_hasRoot)
			transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(_currentPosition.x,Screen.height - _currentPosition.y,_zDepth));
		else
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_currentPosition.x,Screen.height - _currentPosition.y,_zDepth));
	}
	#endregion

	#region Color Methods
	protected Color GetColor()
	{
		return _spriteRenderer.color;
	}
	protected void SetColor(Color color)
	{
		_spriteRenderer.color = color;
	}
	#endregion

	#region Animation Methods
	public void SetTrigger(string triggerName)
	{
		if(_spriteAnimator && _spriteAnimator.runtimeAnimatorController)
		{
			//Debug.Log(triggerName);
			_spriteAnimator.SetTrigger(triggerName);
			//enabled = true;
		} else
			_spriteRenderer.enabled = true;
	}
	public void SetBool(string name, bool value)
	{
		if(_spriteAnimator && _spriteAnimator.runtimeAnimatorController)
		{
			_spriteAnimator.SetBool(name,value);
			//enabled = true;
		}
	}
	#endregion

	#region Accessors
	public Sprite CurrentSprite
	{
		get { return _spriteRenderer.sprite; }
		set { _spriteRenderer.sprite = value; }
	}
	public SpriteRenderer Renderer
	{
		get { return _spriteRenderer; }
		set { _spriteRenderer = value; }
	}

	/// <summary>
	/// Gets the bounding area of the element (in pixels). Assumes the Sprite's
	/// pivot point is Left.
	/// </summary>
	/// <returns>The bounds.</returns>
	public override Rect GetBounds()
	{
		Vector2 max = Camera.main.WorldToScreenPoint(_spriteRenderer.sprite.bounds.max);
		Vector2 min = Camera.main.WorldToScreenPoint(_spriteRenderer.sprite.bounds.min);

		float width = max.x - min.x;
		float height = max.y - min.y;

		return new Rect(_currentPosition.x - width / 2f, _currentPosition.y - height / 2f, max.x - min.x, (max.y - min.y));
	}
	#endregion
}