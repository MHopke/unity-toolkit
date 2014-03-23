using UnityEngine;

/// <summary>
/// Representation of a graphical UI element. Uses Unity 4.3 Sprite System.
/// </summary>
public class UISprite : UIBase
{
	#region Protected Variables
	protected SpriteRenderer _spriteRenderer;

	protected Animator _spriteAnimator;
	#endregion

	/*void Update()
	{
		if(!_spriteRenderer.enabled)
		{
			Debug.Log("re-enabled");
			_spriteRenderer.enabled = true;
			enabled = false;
		}
	}*/

	#region Activation, Deactivation, Init Methods
	public override bool Init()
	{
		if(base.Init())
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();

			_spriteRenderer.enabled = false;
			return true;
		} else
			return false;
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
	public override bool DelayedActivation(bool skipTransition=false)
	{
		if (base.DelayedActivation(skipTransition))
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

	#region Animation Methods
	protected override void SetTrigger(string triggerName)
	{
		if(_animator && _animator.runtimeAnimatorController)
		{
			//Debug.Log(triggerName);
			_animator.SetTrigger(triggerName);
			//enabled = true;
		} else
			_spriteRenderer.enabled = true;
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
		return new Rect(currentPosition.x, currentPosition.y, _spriteRenderer.sprite.rect.width * transform.localScale.x,
			_spriteRenderer.sprite.rect.height * transform.localScale.y);
	}
	#endregion
}