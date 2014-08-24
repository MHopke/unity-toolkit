using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    /// <summary>
    /// Representation of a graphical UI element. Uses Unity 4.3 Sprite System.
    /// </summary>
    public class UIImage : UIBase
    {
        #region Public Vars
        public Image _image;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
    	{
    		base.OnInit();

            if (!_image)
                _image = GetComponent<Image>();
    	}
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if(_image)
                _image.enabled = display;
        }
    	#endregion

    	#region Color Methods
    	protected Color GetColor()
    	{
    		return _image.color;
    	}
    	protected void SetColor(Color color)
    	{
            _image.color = color;
    	}
    	#endregion

    	#region Accessors
    	public Sprite CurrentSprite
    	{
    		get { return _image.sprite; }
            set { _image.sprite = value; }
    	}
    	#endregion
    }
}