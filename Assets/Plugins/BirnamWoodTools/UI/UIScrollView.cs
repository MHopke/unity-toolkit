using UnityEngine;
using System.Collections;

/// <summary>
/// User interface scroll view. The first element in UIElements is the container Sprite.
/// leave it null if there is no container.
/// </summary>
public class UIScrollView : UIView {

	#region Enumerations
	public enum ScrollType { HORIZONTAL = 0, VERTICAL, BOTH }
	#endregion

	#region Public Variables
	public ScrollType Type;
	public Rect ViewRect; //The original view position
	#endregion

	#region Activation, Deactivation
	protected override void Activation()
	{
		if(UIElements != null)
		{
			for(int i = 0; i < UIElements.Count; i++)
			{
				if(UIElements[i] != null && ViewRect.Contains(UIElements[i].CurrentPosition))
					UIElements[i].Activate();
			}
		}

		movementState = MovementState.INITIAL;

		InputHandler.touchMovingEvent += TouchMoving;
	}
	protected override void Deactivation()
	{
		base.Deactivation();

		InputHandler.touchMovingEvent -= TouchMoving;
	}
	#endregion

	#region Touch Events
	void TouchMoving(Vector2 pos, Vector2 delta, int id)
	{
		if(movementState == MovementState.IN_PLACE && ViewRect.Contains(new Vector2(pos.x,Screen.height - pos.y)))
		{
			//Ensure that movement is locked if it should be
			if(Type == ScrollType.HORIZONTAL)
				delta.y = 0;
			else if(Type == ScrollType.VERTICAL)
				delta.x = 0;

			for(int i = 1; i < UIElements.Count; i++)
			{
				if(UIElements[i])
				{
					UIElements[i].CurrentPosition += delta;

					if(ViewRect.Contains(UIElements[i].CurrentPosition))
						UIElements[i].Activate(UIBase.MovementState.IN_PLACE);
					else
						UIElements[i].Deactivate();
				}
			}
		}
	}
	#endregion
}
