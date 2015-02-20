using UnityEngine;
using UnityEngine.UI;
using gametheory.UI;

public class UIScrollbar : UIBase
{
    #region Public Vars
    public Scrollbar _slider;
    public Image _backgroundImage;
    #endregion

    #region Overriden Methods
    protected override void OnInit()
    {
        base.OnInit();

        if (!_slider)
            _slider = GetComponent<Scrollbar>();

        if (!_backgroundImage)
            _backgroundImage = GetComponent<Image>();
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

        if (_slider)
        {
            if (_backgroundImage)
                _backgroundImage.enabled = display;

            if (_slider.targetGraphic)
                _slider.targetGraphic.enabled = display;
        }
    }
    #endregion
}
