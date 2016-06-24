using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using gametheory.UI;

public class IntroView : UIView 
{
    #region Events
    public static event System.Action complete;
    #endregion

    #region Public Vars
    public bool UsesResources;
    public IntroPage[] Pages;
    public IntroTab[] Tabs;
    #endregion

    #region Protected Vars
    protected float _scrollAmount;
    protected int _pageIndex;
    #endregion

    #region Overidden Methods
    protected override void OnActivate()
    {
        base.OnActivate();

        if(UsesResources)
        {
            for(int index=0; index< Pages.Length; index++)
                Pages[index].LoadImage();
        }

        _scrollAmount = 1.0f / Pages.Length;
        //_scrollAmount += (_scrollAmount * 0.5f);

        _pageIndex = 0;
        Tabs[_pageIndex].SwitchStatus(true);
    }
    protected override void OnDeactivate()
    {
        base.OnDeactivate();

        if(UsesResources)
        {
            for(int index=0; index< Pages.Length; index++)
                Pages[index].UnloadImage();
        }
    }
    #endregion

    #region UI Methods
    public void IncrementPage()
    {
        OnIncrement();
        IncrementTabs();
    }
    public void DecrementPage()
    {
        OnDecrement();
        DecrementTabs();
    }
    public void CompleteIntro()
    {
        if(complete != null)
            complete();
    }
    #endregion

    #region Methods
    void IncrementTabs()
    {
        if(_pageIndex + 1 == Pages.Length)
            return;

        Tabs[_pageIndex].SwitchStatus(false);

        _pageIndex++;

        Tabs[_pageIndex].SwitchStatus(true);
    }
    void DecrementTabs()
    {
        if(_pageIndex - 1 < 0)
            return;

        Tabs[_pageIndex].SwitchStatus(false);
        
        _pageIndex--;
        
        Tabs[_pageIndex].SwitchStatus(true);
    }
    #endregion

    #region Virtual Methods
    protected virtual void OnIncrement(){}
    protected virtual void OnDecrement(){}
    #endregion
}