using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using gametheory.UI;

/// <summary>
/// This class is provided as a starting point for any "native app" styled
/// project that we build.
/// </summary>
public class AppNavigationController : UIViewController 
{
    #region Events
    /// <summary>
    /// Fires when the back button is pressed. Passes the current view's name
    /// </summary>
    public static event System.Action<string> back;

    /// <summary>
    /// Fires when the user has confirmed a back button request.
    /// </summary>
    static event System.Action confirmedBack;

    /// <summary>
    /// Fires when the UI view stack is cleared.
    /// </summary>
    public static event System.Action viewStackCleared;
    #endregion

    #region Enums
    //enum NavigationState { NONE =0, MENU, TEAM, WORKOUT, GAMES }
    #endregion

    #region Public Vars
    public ExtendedButton BackButton;

    public UIView CurrentView;
    public UIView NavView;
    public UIView TopView;
    #endregion

    #region Private Vars
    bool _viewChangeRequiresConfirm, 
    _backRequiresConfirm,
    _clearStackHistoryWithConfirm;

    string _backConfirmTitle;
    string _backConfirmInfo;

    UIView _viewForConfirm;
    protected UIView _homeView;

    //NavigationState _navState;

    protected Stack<UIView> _viewStack;
    #endregion

    #region Overriden Methods
    protected override void OnActivate()
    {
        base.OnActivate();

        /*if(_backButton)
            _defaultBack = _backButton._button.image.sprite;*/

        _viewStack = new Stack<UIView>();

		if(CurrentView != null)
		{
        	_homeView = CurrentView;
        	_viewStack.Push(_homeView);
		}
    }
    protected override void OnDeactivate()
    {
        base.OnDeactivate();

        _viewStack.Clear();
    }
    protected override void OnPresent(UIView view)
    {
        if (CurrentView && CurrentView.name == view.name)
            return;

        base.OnPresent(view);

        if ((_viewStack != null && BackButton != null))
        {
            if(_viewStack.Count == 1)
            {
                BackButton.Activate();
            }
        }

		view.SetInNavigation(true);

        _viewStack.Push(view);

        CurrentView = view;
    }
    #endregion

    #region Methods
    void ActivateSingleView(UIView view, bool clearStackHistory=false)
    {
        if (_viewChangeRequiresConfirm)
        {
            _viewForConfirm = view;
            _clearStackHistoryWithConfirm = clearStackHistory;
            //AlertView.Present(_backConfirmTitle, _backConfirmInfo, Confirm, Cancel,true);
            return;
        }

        if (CurrentView.name == view.name)
            return;

        DeactivateUIView(CurrentView);
        ActivateUIView(view);

        CheckNavigationState();

        if(clearStackHistory)
            RestoreStackToFlowStart();
    }
    public void PushViewOnToStack(UIView view)
    {
        if (_viewChangeRequiresConfirm)
        {
            _viewForConfirm = view;
            //_clearStackHistoryWithConfirm = clearStackHistory;
            //AlertView.Present(_backConfirmTitle, _backConfirmInfo, Confirm, Cancel,true);
            return;
        }

        if (CurrentView.name == view.name)
            return;

        DeactivateUIView(CurrentView);
        ActivateUIView(view);
    }
    public void PopView()
    {
        Back();
    }
    public void PopStackToHome()
    {
        ClearStack();

        DeactivateUIView(CurrentView);
        ActivateUIView(_homeView);
    }
    public void ViewChangeRequiresConfirm(string title, string message, System.Action callback)
    {
        _viewChangeRequiresConfirm = true;
        _backConfirmTitle = title;
        _backConfirmInfo = message;

        confirmedBack = callback;
    }
    public void ClearViewConfirm()
    {
        _viewChangeRequiresConfirm = false;
        _viewForConfirm = null;
        
        confirmedBack = null;
    }
    public void BackRequiresConfirm(string title, string message, System.Action callback,bool includeNavBar=true)
    {
        _backRequiresConfirm = true;
        _backConfirmTitle = title;
        _backConfirmInfo = message;
        //controller._includeNavBar = includeNavBar;
        
        confirmedBack = callback;
    }
    public void ClearBackConfirm()
    {
        _backRequiresConfirm = false;
        _viewForConfirm = null;

        confirmedBack = null;
    }

	public void BringBackbuttonToTop()
	{
		if(BackButton != null)
			BackButton.transform.SetAsLastSibling();
	}

    void RestoreStackToFlowStart()
    {
        PopStackToHome();
        _viewStack.Push(CurrentView);
        
        /*if(_backButton._button.image.sprite != _homeSprite)
            _backButton._button.image.sprite = _homeSprite;*/
        
        if (viewStackCleared != null)
            viewStackCleared();
    }
    
    void ClearStack()
    {
		UIView view = null;
		while(_viewStack.Count > 0)
		{
			view = _viewStack.Pop();
			view.SetInNavigation(false);
			RemoveViewFromList(view);
		}
        _viewStack.Clear(); 
    }
    void CheckNavigationState()
    {
    }
    #endregion

    #region UI Methods
    public void Back()
    {
        if (_viewChangeRequiresConfirm || _backRequiresConfirm)
        {
            _viewForConfirm = null;
            //AlertView.Present(_backConfirmTitle, _backConfirmInfo, Confirm, Cancel,true);
            return;
        }

		UIView current = _viewStack.Peek();

		current.SetInNavigation(false);
		current.Deactivate();

        if (back != null)
			back(current.name);
        
		_viewStack.Pop();

        //Debug.Log(_viewStack.Count);

        _viewStack.Peek().Activate();
        CurrentView = _viewStack.Peek();

        CheckNavigationState();

        if (_viewStack.Count == 1 && BackButton)
            BackButton.Deactivate();
        /*else if(_viewStack.Count == 2)
            _backButton._button.image.sprite = _homeSprite;*/
    }
    #endregion

    #region AlertView Callbacks
    void Confirm()
    {
        //_includeNavBar = false;
        _viewChangeRequiresConfirm = false;
        _backRequiresConfirm = false;
        
        if(_viewForConfirm == null)
            Back();
        else
        {
            ActivateSingleView(_viewForConfirm,_clearStackHistoryWithConfirm);
        }

        if (confirmedBack != null)
            confirmedBack();

        confirmedBack = null;
    }
    void Cancel()
    {
        //confirmedBack = null;
    }
    void NavConfirm()
    {
        //_includeNavBar = false;
        _viewChangeRequiresConfirm = false;
        _backRequiresConfirm = false;

        ActivateSingleView(_viewForConfirm,_clearStackHistoryWithConfirm);

        if (confirmedBack != null)
            confirmedBack();

        confirmedBack = null;
    }
    #endregion

	#region Properties
	public AppNavigationController Navigation
	{
		get { return Instance as AppNavigationController; }
	}
	#endregion
}
