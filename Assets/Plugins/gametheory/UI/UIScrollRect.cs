using UnityEngine;
using UnityEngine.UI;
using gametheory.UI;

public class UIScrollRect : UIBase 
{
    #region Public Vars
    public ScrollRect _scrollRect;
    public Image _maskImage;
    #endregion

    #region Overriden Methods
    protected override void OnInit()
    {
        base.OnInit();

        if (!_scrollRect)
            _scrollRect = GetComponent<ScrollRect>();

        if (!_maskImage)
            _maskImage = GetComponent<Image>();
    }
    /*protected override void Enabled()
    {
        base.Enabled();

        if (_scrollRect)
        {
            _scrollRect.enabled = true;

            if (_scrollRect.horizontalScrollbar)
                _scrollRect.horizontalScrollbar.interactable = true;

            if (_scrollRect.verticalScrollbar)
                _scrollRect.verticalScrollbar.interactable = true;
        }
    }
    protected override void Disabled()
    {
        base.Disabled();

        if (_scrollRect)
        {
            _scrollRect.enabled = false;

            if (_scrollRect.horizontalScrollbar)
                _scrollRect.horizontalScrollbar.interactable = false;

            if (_scrollRect.verticalScrollbar)
                _scrollRect.verticalScrollbar.interactable = false;
        }
    }*/
    public override void PresentVisuals(bool display)
    {
        base.PresentVisuals(display);

        if (_scrollRect)
        {
            _scrollRect.enabled = display;

            if (_scrollRect.horizontalScrollbar)
                _scrollRect.horizontalScrollbar.interactable = display;

            if (_scrollRect.verticalScrollbar)
                _scrollRect.verticalScrollbar.interactable = display;
        }

        if (_maskImage)
            _maskImage.enabled = display;
    }
    #endregion
}
