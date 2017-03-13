using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    /// <summary>
    /// Representation of a graphical UI element. Uses Unity 4.3 Sprite System.
    /// </summary>
	[RequireComponent(typeof(Image))]
    public class ExtendedImage : VisualElement
    {
        #region Public Vars
        public Image Image;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
    	{
    		base.OnInit();

            if (!Image)
                Image = GetComponent<Image>();
    	}
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if(Image)
                Image.enabled = display;
        }
    	#endregion

    	#region Color Methods
    	protected Color GetColor()
    	{
    		return Image.color;
    	}
    	protected void SetColor(Color color)
    	{
            Image.color = color;
    	}
    	#endregion

    	#region Accessors
    	public Sprite CurrentSprite
    	{
    		get { return Image.sprite; }
            set { Image.sprite = value; }
    	}
        public Image CurrentImage
        {
            get { return Image; }
            set { Image = value; }
        }
    	#endregion
    }
}