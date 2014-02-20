using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(UISprite))]
public class UIAnimatedButton : UIButton 
{
	#region Public Variables
	public Animator _animator;
	#endregion

	#region Click Methods
	void Click()
	{
		_buttonSettings.Click();

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
}
