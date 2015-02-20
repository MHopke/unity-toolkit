using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using gametheory.UI;

/// <summary>
/// This class is provided as a starting point for any "native app" styled
/// project that we build.
/// </summary>
public class MainViewController : UIViewController 
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
    public UIButton _backButton;

    public UIView _currentView;
    public UIView _navView;
    public UIView _topView;

    public UIView _mainView;
    public UIView _loginView;
    public UIView _usersView;
    #endregion

    #region Private Vars
    bool _viewChangeRequiresConfirm, 
    _backRequiresConfirm,
    _clearStackHistoryWithConfirm;

    string _backConfirmTitle;
    string _backConfirmInfo;

    UIView _viewForConfirm;

    //NavigationState _navState;

    Stack<UIView> _viewStack;
    #endregion

    #region Overriden Methods
    protected override void OnInit()
    {
        base.OnInit();

        /*if(_backButton)
            _defaultBack = _backButton._button.image.sprite;*/

        _viewStack = new Stack<UIView>();
    }
    protected override void OnDeactivate()
    {
        base.OnDeactivate();

        _viewStack.Clear();
    }
    protected override void OnPresent(UIView view)
    {
        if (_currentView.name == view.name)
            return;

        base.OnPresent(view);

        if ((_viewStack != null && _backButton != null))
        {
            if(_viewStack.Count == 1)
            {
                _backButton.Activate();
            }
        }

        _viewStack.Push(view);

        _currentView = view;
    }
    #endregion

    #region Methods
    void PresentSingleView(UIView view, bool clearStackHistory=false)
    {
        if (_viewChangeRequiresConfirm)
        {
            _viewForConfirm = view;
            _clearStackHistoryWithConfirm = clearStackHistory;
            //AlertView.Present(_backConfirmTitle, _backConfirmInfo, Confirm, Cancel,true);
            return;
        }

        if (_currentView.name == view.name)
            return;

        RemoveUIView(_currentView);
        PresentUIView(view);

        CheckNavigationState();

        if(clearStackHistory)
            RestoreStackToFlowStart();
    }
    public static void ViewChangeRequiresConfirm(string title, string message, System.Action callback)
    {
        MainViewController controller = Instance as MainViewController;

        controller._viewChangeRequiresConfirm = true;
        controller._backConfirmTitle = title;
        controller._backConfirmInfo = message;

        confirmedBack = callback;
    }
    public static void ClearViewConfirm()
    {
        MainViewController controller = Instance as MainViewController;
        
        controller._viewChangeRequiresConfirm = false;
        controller._viewForConfirm = null;
        
        confirmedBack = null;
    }
    public static void BackRequiresConfirm(string title, string message, System.Action callback,bool includeNavBar=true)
    {
        MainViewController controller = Instance as MainViewController;

        controller._backRequiresConfirm = true;
        controller._backConfirmTitle = title;
        controller._backConfirmInfo = message;
        //controller._includeNavBar = includeNavBar;
        
        confirmedBack = callback;
    }
    public static void ClearBackConfirm()
    {
        MainViewController controller = Instance as MainViewController;

        controller._backRequiresConfirm = false;
        controller._viewForConfirm = null;

        confirmedBack = null;
    }
    public static void GoBack()
    {
        MainViewController controller = Instance as MainViewController;
        controller.Back();
    }

    void RestoreStackToFlowStart()
    {
        SetViewStackToStart();
        _viewStack.Push(_currentView);
        
        /*if(_backButton._button.image.sprite != _homeSprite)
            _backButton._button.image.sprite = _homeSprite;*/
        
        if (viewStackCleared != null)
            viewStackCleared();
    }

    void SetViewStackToStart()
    {
        ClearStack();
        _viewStack.Push(_mainView);
    }
    
    void ClearStack()
    {
        _viewStack.Clear(); 
    }
    void CheckNavigationState()
    {
        /*switch (_navState)
        {
            case NavigationState.WORKOUT:
                _workoutButton._text.color = _workoutButton._button.image.color = _workoutButton._button.colors.normalColor;
                break;
            case NavigationState.GAMES:
                _gamesButton._text.color = _gamesButton._button.image.color = _gamesButton._button.colors.normalColor;
                break;
            case NavigationState.MENU:
                _menuButton._text.color = _menuButton._button.image.color = _menuButton._button.colors.normalColor;
                break;
            case NavigationState.TEAM:
                _teamButton._text.color = _teamButton._button.image.color = _teamButton._button.colors.normalColor;
                break;
        }

        _navState = NavigationState.NONE;

        if (_currentView == _programView)
        {
            _workoutButton._text.color = _workoutButton._button.colors.pressedColor;
            _workoutButton._button.image.color = _workoutButton._button.colors.pressedColor;
            _navState = NavigationState.WORKOUT;
        }
        else if (_currentView == _teamView)
        {
            _teamButton._text.color = _teamButton._button.colors.pressedColor;
            _teamButton._button.image.color = _teamButton._button.colors.pressedColor;
            _navState = NavigationState.TEAM;
        }
        else if (_currentView == _gamesListView)
        {
            _gamesButton._text.color = _gamesButton._button.colors.pressedColor;
            _gamesButton._button.image.color = _gamesButton._button.colors.pressedColor;
            _navState = NavigationState.GAMES;
        }
        else if (_currentView == _menuView)
        {
            _menuButton._text.color = _menuButton._button.colors.pressedColor;
            _menuButton._button.image.color = _menuButton._button.colors.pressedColor;
            _navState = NavigationState.MENU;
        }*/
    }
    void LoadMain()
    {
        if(_currentView != null)
            _currentView.Deactivate();

        _currentView = _mainView;
        _mainView.Activate();
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

        _viewStack.Peek().Deactivate(true);

        if (back != null)
            back(_viewStack.Peek().name);

        _viewStack.Pop();

        //Debug.Log(_viewStack.Count);

        _viewStack.Peek().Activate(true);
        _currentView = _viewStack.Peek();

        CheckNavigationState();

        if (_viewStack.Count == 1)
            _backButton.Deactivate();
        /*else if(_viewStack.Count == 2)
            _backButton._button.image.sprite = _homeSprite;*/
    }
    public void GoToUsersView()
    {
        PresentSingleView(_usersView,true);
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
            PresentSingleView(_viewForConfirm,_clearStackHistoryWithConfirm);
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

        PresentSingleView(_viewForConfirm,_clearStackHistoryWithConfirm);

        if (confirmedBack != null)
            confirmedBack();

        confirmedBack = null;
    }
    #endregion
}
