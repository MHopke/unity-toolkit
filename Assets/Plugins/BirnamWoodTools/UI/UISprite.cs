using UnityEngine;

/// <summary>
/// Representation of a graphical UI element. Uses Unity 4.3 Sprite System.
/// </summary>
public class UISprite : UIBase
{
	#region Public Variables
	//Indicates there is a root object so this can animate relative scale & position
	public bool _hasRoot;
	#endregion

	#region Protected Variables
	protected SpriteRenderer _spriteRenderer;

	protected Animator _spriteAnimator;
	#endregion

	#region Activation, Deactivation, Init Methods
	public override void Init(Vector2 offset, float speedParam)
	{
		_spriteAnimator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

		base.Init(offset, speedParam);
	}

	public override bool Activate(MovementState state=MovementState.INITIAL)
	{
		if(base.Activate(state))
		{
			//Debug.Log("active");
			_spriteRenderer.enabled = true;
			return true;
		} else
			return false;
	}
	public override bool Deactivate(bool force=false)
	{
		if(base.Deactivate(force))
		{
			_spriteRenderer.enabled = false;

			return true;
		} else
			return false;
	}
	#endregion

	#region Position Methods
	protected override void SetPosition(Vector2 position)
	{
		base.SetPosition(position);

		//Debug.Log(name + " " + currentPosition);

		if(_hasRoot)
			transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x,Screen.height - currentPosition.y,1f));
		else
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x,Screen.height - currentPosition.y,1f));
	}
	#endregion

	#region Animation Methods
	public void SetTrigger(string triggerName)
	{
		if(_spriteAnimator)
			_spriteAnimator.SetTrigger(triggerName);
	}
	#endregion

	#region Type Methods
	public override System.Type GetBaseType()
	{
		return typeof(UISprite);
	}
	#endregion

	#region Color Methods
	protected override Color GetColor()
	{
		return _spriteRenderer.color;
	}
	protected override void SetColor(Color color)
	{
		_spriteRenderer.color = color;
	}
	#endregion

	#region Accessors
	public Sprite CurrentSprite
	{
		get { return _spriteRenderer.sprite; }
		set { _spriteRenderer.sprite = value; }
	}

	/// <summary>
	/// Gets the bounding area of the element (in pixels). Assumes the Sprite's
	/// pivot point is Left.
	/// </summary>
	/// <returns>The bounds.</returns>
	public override Rect GetBounds()
	{
		return new Rect(currentPosition.x, currentPosition.y + (_spriteRenderer.sprite.rect.height / 2.0f) * transform.localScale.y,
			_spriteRenderer.sprite.rect.height * transform.localScale.y, _spriteRenderer.sprite.rect.width * transform.localScale.x);
	}
	#endregion
}