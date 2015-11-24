using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
	[RequireComponent(typeof(ScrollRect))]
	public class ExtendedScrollRect : VisualElement 
	{
	    #region Public Vars
	    public ScrollRect ScrollRect;
	    #endregion

	    #region Overriden Methods
	    protected override void OnInit()
	    {
	        base.OnInit();

	        if (!ScrollRect)
	            ScrollRect = GetComponent<ScrollRect>();
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
	    }
	    #endregion
	}
}
