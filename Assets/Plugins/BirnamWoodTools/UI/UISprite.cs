using UnityEngine;
using System.Collections;

public class UISprite : UIBase
{
	#region Activation, Deactivation, Init Methods
	public override void Activate()
	{
		enabled = true;

		renderer.enabled = true;

		base.Activate();
	}
	public override void Deactivate()
	{
		enabled = false;

		renderer.enabled = false;

		base.Deactivate();
	}
	#endregion

	#region Movement Methods
	protected override void SetPosition(Vector2 position)
	{
		base.SetPosition(position);
		transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x,Screen.height - position.y,1f));
	}
	#endregion
}
