using UnityEngine;
using System.Collections;

public class UIEffectText : UIBase 
{
	#region Public Methods
	public EffectManager _effect;
	public float _zDepth;
	#endregion

	#region Overridden Methods
	protected override void OnInit()
	{
		base.OnInit();

		if(!_effect)
			_effect = GetComponent<EffectManager>();

		transform.SetZPosition(_zDepth);
	}
	protected override void OnActivate(MovementState moveState)
	{
		base.OnActivate(moveState);
	}
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
	}
	#endregion

	#region Methods
	public void PlayEffect()
	{
		if(_effect)
			_effect.PlayAnimation();
	}

	protected override void SetPosition(Vector2 position)
	{
		base.SetPosition(position);

		transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_currentPosition.x,Screen.height - _currentPosition.y,_zDepth));
	}
	#endregion

	#region Accessors
	public string Text
	{
		get { return _effect.Text; }
		set { _effect.SetText(value); }
	}
	#endregion
}
