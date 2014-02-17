using UnityEngine;
using System.Collections;

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
		base.Init(offset, speedParam);

		_spriteAnimator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public override bool Activate(MovementState state=MovementState.INITIAL)
	{
		//Debug.Log(name + " " +enabled);
		if(base.Activate(state))
		{
			//Debug.Log("active");
			_spriteRenderer.enabled = true;
			return true;
		} else
			return false;
	}
	public override bool Deactivate()
	{
		if(base.Deactivate())
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
}