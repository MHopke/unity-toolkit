using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class AnimatedButton : ButtonComponent 
{
	#region Public Variables
	public bool _switchOnExit;
	public Sprite _pressedSprite;
	public Animator _animator;
	#endregion

	#region Private Variables
	Sprite _originalSprite;
	SpriteRenderer _renderer;
	#endregion

	#region Unity Methods
	void Start()
	{
		_renderer = renderer as SpriteRenderer;
		_originalSprite = _renderer.sprite;
	}
	#endregion

	#region Activate/Deactivate
	public override bool Deactivate()
	{
		if(base.Deactivate())
		{
			SetToNormal();
			return true;
		} else
			return false;
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
		_renderer.sprite = _pressedSprite;
	}
	public void SetToNormal()
	{
		_renderer.sprite = _originalSprite;
	}
	#endregion

	#region Input Methods
	protected override bool CheckStart(Vector2 touch)
	{
		if(base.CheckStart(touch))
		{
			_renderer.sprite = _pressedSprite;
			return true;
		}
		else
			return false;
	}
	protected override bool CheckMoving(Vector2 pos)
	{
		if(base.CheckMoving(pos))
			return true;
		else
		{
			_renderer.sprite = _originalSprite;
			return false;
		}
	}
	protected override bool CheckEnd(Vector2 touch, int id)
	{
		_renderer.sprite = _originalSprite;

		return base.CheckEnd(touch, id);
	}
	#endregion
}
