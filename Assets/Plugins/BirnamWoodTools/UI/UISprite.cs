using UnityEngine;
using System.Collections;

public class UISprite : UIBase
{
	#region Activation, Deactivation, Init Methods
	public override void Activate(MovementState state=MovementState.INITIAL)
	{
		enabled = true;

		renderer.enabled = true;

		base.Activate(state);
	}
	public override void Deactivate()
	{
		enabled = false;

		renderer.enabled = false;

		base.Deactivate();
	}
	#endregion

	#region Position Methods
	protected override void SetPosition(Vector2 position)
	{
		base.SetPosition(position);

		//Debug.Log(name + " " + currentPosition);

		transform.position = Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x,Screen.height - currentPosition.y,1f));
	}
	#endregion
}
