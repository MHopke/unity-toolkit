using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// Representation of a graphical UI element. Uses Unity 4.3 Sprite System.
    /// </summary>
    public class GUITexture : UIBase
    {
    	#region Public Variables
    	public Texture2D _texture;
    	#endregion

    	#region Draw Method
    	public override void Draw()
    	{
    		GUI.DrawTexture(_drawRect, _texture);
    		base.Draw();
    	}
    	#endregion

    	#region Type Methods
    	public override System.Type GetBaseType()
    	{
    		return typeof(GUITexture);
    	}
    	#endregion
    }
}