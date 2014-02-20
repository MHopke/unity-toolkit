//#define DRAWLABELS
using UnityEngine;

public class UILabel : UIBase 
{
	#region Public Variables
	public int depth;

	public string text;

	public Vector2 size;

	public CustomStyle customStyle;
	#endregion

	#region Protected Variables
	protected Rect drawRect;
	#endregion

	#region Update
	protected override bool CanDisable()
	{
		return false;
	}
	#endregion

	#region Draw Methods
	#if DRAWLABELS
	public void Draw()
	{
	#else
	protected virtual void OnGUI()
	{
		useGUILayout = false;

		GUI.skin = UINavigationController.Skin;

		GUI.depth = depth;

	#endif
		if(customStyle.custom)
			GUI.Label(drawRect, text, customStyle.style);
		else
			GUI.Label(drawRect, text, customStyle.styleName);
	}
	#endregion

	#region Activation, Deactivation, Init Methods
	public override void Init(Vector2 offset, float speed)
	{
		customStyle.SetDefaultStyle("label");

		base.Init(offset,speed);

		size.Scale(UINavigationController.AspectRatio);

		drawRect = new Rect(position.x, position.y, size.x, size.y);
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

	#region Style Methods
	public void CreateCustomStyle()
	{
		if(!customStyle.custom)
		{
			customStyle.custom = true;
			customStyle.style = new GUIStyle(UINavigationController.Skin.FindStyle(customStyle.styleName));
		}
	}
	#endregion

	#region Type Methods
	public override System.Type GetBaseType()
	{
		return typeof(UILabel);
	}
	#endregion

	#region Color Methods
	protected override Color GetColor()
	{
		return customStyle.style.normal.textColor;
	}
	protected override void SetColor(Color color)
	{
		customStyle.style.normal.textColor = color;
	}
	#endregion

	#region Accessors
	public override Rect GetBounds()
	{
		return new Rect(currentPosition.x, currentPosition.y, size.x, size.y);
	}
	#endregion
}
