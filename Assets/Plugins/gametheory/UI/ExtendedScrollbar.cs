using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
	[RequireComponent(typeof(Scrollbar))]
	public class ExtendedScrollbar : VisualElement
	{
	    #region Public Vars
	    public Scrollbar Slider;
	    public Image BackgroundImage;
	    #endregion

	    #region Private Vars
	    bool _hide;
	    #endregion

	    #region Overriden Methods
	    protected override void OnInit()
	    {
	        base.OnInit();

	        if (!Slider)
	            Slider = GetComponent<Scrollbar>();

	        if (!BackgroundImage)
	            BackgroundImage = GetComponent<Image>();
	    }
	    /*protected override void Disabled()
	    {
	        base.Disabled();

	        if (_slider)
	            _slider.interactable = false;
	    }
	    protected override void Enabled()
	    {
	        base.Enabled();

	        if (_slider)
	            _slider.interactable = true;
	    }*/
	    public override void PresentVisuals(bool display)
	    {
	        base.PresentVisuals(display);

	        if (Slider)
	        {
	            if (BackgroundImage)
	                BackgroundImage.enabled = display;

	            if (Slider.targetGraphic)
	                Slider.targetGraphic.enabled = display;
	        }
	    }
	    #endregion

	    #region Methods
	    public void Hide()
	    {
	        if (_hide)
	            return;

	        _hide = true;
	        
	        PresentVisuals(false);
	        Slider.interactable = false;
	    }
	    public void Show()
	    {
	        if (!_hide)
	            return;
	        
	        _hide = false;

	        PresentVisuals(true);
	        Slider.interactable = true;
	    }
	    #endregion

	    #region Accessors
	    public float Size
	    {
	        get { return Slider.size; }
	    }
	    #endregion
	}
}
