using UnityEngine;

/// <summary>
/// The UI representation of a label. Uses OnGUI to render the text.
/// </summary>
public class UILabel : UIBase 
{
	#region Public Variables
	public string text;
	#endregion

	#region Activation, Deactivation, Init Methods
	protected override void OnInit()
	{
		base.OnInit();

		_primaryStyle.SetDefaultStyle("label");
	}
	#endregion

	#region Draw Methods
	public override void Draw()
	{
		if(_primaryStyle.custom)
			GUI.Label(_drawRect, text, _primaryStyle.style);
		else
			GUI.Label(_drawRect, text, _primaryStyle.styleName);
	}
	#endregion

	#region Style Methods
	/// <summary>
	/// Creates a custom style if the UILabel previously wasn't using one.
	/// </summary>
	public void CreateCustomStyle()
	{
		if(!_primaryStyle.custom)
		{
			_primaryStyle.custom = true;

			_primaryStyle.style = new GUIStyle(UINavigationController.Skin.FindStyle(_primaryStyle.styleName));
		}
	}
	#endregion

	#region Type Methods
	public override System.Type GetBaseType()
	{
		return typeof(UILabel);
	}
	#endregion
}