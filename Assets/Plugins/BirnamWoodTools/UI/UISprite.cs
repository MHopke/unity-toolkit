﻿using UnityEngine;
using System.Collections;

public class UISprite : UIBase
{
	#region Public Variables
	//Indicates there is a root object so this can animate relative scale & position
	public bool _hasRoot;
	#endregion

	#region Activation, Deactivation, Init Methods
	public override bool Activate(MovementState state=MovementState.INITIAL)
	{
		if(base.Activate(state))
		{
			renderer.enabled = true;
			return true;
		} else
			return false;
	}
	public override bool Deactivate()
	{
		if(base.Deactivate())
		{
			renderer.enabled = false;
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
}
