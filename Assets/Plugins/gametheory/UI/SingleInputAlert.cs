using UnityEngine.UI;
using gametheory.UI;
using System.Collections;

public class SingleInputAlert : UIAlert
{
    #region Events
    static event System.Action<string> confirm;
    static event System.Action cancel;
    #endregion
    
    #region Public Vars
    public Text _titleText;
    public Text _messageText;
    public UIInputField _inputField;
    public UIButton _closeButton;

    public static SingleInputAlert Instance = null;
    #endregion

    #region Overriden Methods
    protected override void Init()
    {
        base.Init();
        
        if(Instance == null)
            Instance = this;
    }
    #endregion
    
    #region Methods
    public void Present(string title, string message, string inputPlaceholder, System.Action<string> confirmCallback, System.Action cancelCallback, bool showClose=false)
    {
        confirm = confirmCallback;
        cancel = cancelCallback;

        _inputField.PlaceholderText = inputPlaceholder;
        _inputField.Text = "";

        if(showClose)
            _closeButton.Activate();
        
        _titleText.text = title;
        _messageText.text = message;
        
        Open();
    }
    
    public void Confirm()
    {
        if (string.IsNullOrEmpty(_inputField.Text) || Helper.CheckIfSpaces(_inputField.Text))
        {
            DefaultAlert.Present("Sorry!", "Please input some text.", null, null);
            return;
        }

        Close();

        if (confirm != null)
            confirm(_inputField.Text);
        
        confirm = null;
        cancel = null;
    }
    public void Cancel()
    {
        Close();
        
        if (cancel != null)
            cancel();
        
        confirm = null;
        cancel = null;
    }
    #endregion
}
