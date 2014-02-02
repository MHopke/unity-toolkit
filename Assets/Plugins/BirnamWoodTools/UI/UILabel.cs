using UnityEngine;

public class UILabel : UIBase 
{
	#region Public Variables
	public string text;

	public Vector2 size;

	public CustomStyle customStyle;
	#endregion

	#region Protected Variables
	Rect drawRect;
	#endregion

	#region Draw Methods
	public void Draw()
	{
		if(customStyle.custom)
			GUI.Label(drawRect, text, customStyle.style);
		else
			GUI.Label(drawRect, text, customStyle.styleName);
	}
	#endregion

	#region Activation, Deactivation, Init Methods
	public override void Init(Vector2 offset)
	{
		base.Init(offset);

		size.Scale(UINavigationController.AspectRatio);

		drawRect = new Rect(position.x, position.y, size.x, size.y);

		customStyle.SetDefaultStyle("label");
	}
	#endregion

	#region Position Methods
	protected override void SetPosition(Vector2 position)
	{
		base.SetPosition(position);

		drawRect.x = position.x;
		drawRect.y = position.y;
	}
	#endregion
}
