using UnityEngine;
using UnityEngine.UI;
using gametheory.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DefaultAlert : UIAlert
{
    #region Events
    static event System.Action confirm;
    static event System.Action cancel;
    #endregion

    #region Public Vars
    public Text TitleText;
    public Text MessageText;
    public Text ConfirmText;

    public ExtendedButton CloseButton;

	public static bool IsOpen;
    public static DefaultAlert Instance = null;
    #endregion

    #region Private Vars
    string CONFIRM_TEXT = "CONFIRM";
    static Queue<AlertContent> _alertQueue;
    #endregion

    #region Unity Methods
    void Awake()
    {
        Initialize();
    }
    #endregion

    #region Overriden Methods
    protected override void OnInit()
    {
        base.OnInit();

		IsOpen = false;

        _alertQueue = new Queue<AlertContent>();

        Instance = this;
    }
    protected override void OnCleanUp()
    {
        Instance = null;
        base.OnCleanUp();
    }
    #endregion

    #region UI Methods
    public void Confirm()
	{
		if(IsQueueEmpty())
		{
			Deactivate();
			IsOpen = false;
		}

		if (confirm != null)
			confirm();

		confirm = null;
		cancel = null;
    }
	public void Cancel()
	{
		if(IsQueueEmpty())
		{
			Deactivate();
			IsOpen = false;
		}

		if (cancel != null)
			cancel();

		confirm = null;
		cancel = null;
    }
    #endregion

    #region Methods
    public static void Present(string title, string message, bool showClose = false, string confirmText = "")
    {
        Present(title, message, null, null, showClose, confirmText);
    }
    public static void Present(string title, string message, Action confirmCallback = null, bool showClose = false, string confirmText = "")
    {
        Present(title, message, confirmCallback, null, showClose, confirmText);
    }
    public static void Present(string title, string message, Action confirmCallback=null, Action cancelCallback=null, bool showClose = false, string confirmText="")
    {
        if (confirmText == "")
            confirmText = Instance.CONFIRM_TEXT;

        if (_alertQueue.Count == 0)
        {
            confirm = confirmCallback;
            cancel = cancelCallback;
        
            Instance.TitleText.text = title;
            Instance.MessageText.text = message;
            Instance.ConfirmText.text = confirmText;

			UIAlertController.Instance.PresentAlert(Instance);

			if (showClose)
				Instance.CloseButton.Activate();

            IsOpen = true;
        }

        _alertQueue.Enqueue(new AlertContent(title,message,confirmText, confirmCallback,cancelCallback,showClose));
    }

    public void OpenNext()
    {
        AlertContent content = _alertQueue.Peek();

        confirm = content.Confirm;
        cancel = content.Cancel;
        
        if(content.ShowClose)
			CloseButton.Activate();
        else
			CloseButton.Deactivate();
        
        TitleText.text = content.Title;
        MessageText.text = content.Message;
        ConfirmText.text = content.ConfirmText;

		transform.SetAsLastSibling();

		CanvasGroup.interactable = true;
		CanvasGroup.blocksRaycasts = true;
    }

	bool IsQueueEmpty()
	{
		_alertQueue.Dequeue();

		if(_alertQueue.Count > 0)
		{
			OpenNext();
			return false;
		}
		else
			return true;
	}
    #endregion

    #region LocalizationComponent Event Listeners
    void LanguageChanged()
    {
        ConfirmText.text = CONFIRM_TEXT;
    }
    #endregion
}

public class AlertContent
{
    #region Public Vars
    public bool ShowClose;
    public string Title;
    public string Message;
    public string ConfirmText;
    public System.Action Confirm;
    public System.Action Cancel;
    #endregion

    #region Constructors
    public AlertContent(string title, string message,string confirmText, Action confirm, Action cancel, bool showClose)
    {
        Title = title;
        Message = message;
        ConfirmText = confirmText;
        Confirm = confirm;
        Cancel = cancel;
        ShowClose = showClose;
    }
    #endregion
}
