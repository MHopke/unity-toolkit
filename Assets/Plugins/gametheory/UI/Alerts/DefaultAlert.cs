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
    #endregion

    #region Private Vars
    string CONFIRM_TEXT = "CONFIRM";
    static Queue<AlertContent> _alertQueue;
	static DefaultAlert _instance = null;
    #endregion

    #region Overriden Methods
    protected override void OnInit()
    {
        base.OnInit();

		IsOpen = false;

        _alertQueue = new Queue<AlertContent>();
    }
    protected override void OnDeactivate()
    {
		base.OnDeactivate();
		_instance = null;
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
    public static void Present(string title, string message, Action confirmCallback=null, Action cancelCallback=null, bool showClose = false, string confirmText="")
    {
		GetInstance();

        if (confirmText == "")
            confirmText = _instance.CONFIRM_TEXT;

        if (_alertQueue.Count == 0)
        {
            confirm = confirmCallback;
            cancel = cancelCallback;
        
            _instance.TitleText.text = title;
            _instance.MessageText.text = message;
            _instance.ConfirmText.text = confirmText;

			UIAlertController.Instance.PresentAlert(_instance);

			if (showClose)
				_instance.CloseButton.Present();

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
			CloseButton.Present();
        else
			CloseButton.Remove();
        
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

	static void GetInstance()
	{
		if(_instance == null)
		{
			_instance = Load("Alerts/DefaultAlert",
				UIAlertController.Instance.CanvasRect) as DefaultAlert;
		}
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
