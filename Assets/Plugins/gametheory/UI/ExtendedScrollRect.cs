using UnityEngine;
using UnityEngine.UI;
using gametheory.UI;

public class ExtendedScrollRect : VisualElement 
{
    #region Public Vars
    public ScrollRect ScrollRect;
    public Image MaskImage;
    #endregion

    #region Overriden Methods
    protected override void OnInit()
    {
        base.OnInit();

        if (!ScrollRect)
            ScrollRect = GetComponent<ScrollRect>();

        if (!MaskImage)
            MaskImage = GetComponent<Image>();
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

        if (ScrollRect)
        {
            ScrollRect.enabled = display;

            if (ScrollRect.horizontalScrollbar)
                ScrollRect.horizontalScrollbar.interactable = display;

            if (ScrollRect.verticalScrollbar)
                ScrollRect.verticalScrollbar.interactable = display;
        }

        if (MaskImage)
            MaskImage.enabled = display;
    }
    #endregion
}
