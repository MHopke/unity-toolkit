using UnityEngine.UI;
using gametheory.UI;
using System.Collections;

using gametheory.Utilities;

public class SingleInputAlert : UIAlert
{
    #region Events
    static event System.Action<string> confirm;
    static event System.Action cancel;
    #endregion
    
    #region Public Vars
    public Text TitleText;
    public Text MessageText;
    public ExtendedInputField InputField;
    public ExtendedButton CloseButton;

    public static SingleInputAlert Instance = null;
    #endregion

    #region Overriden Methods
    protected override void OnInit()
    {
        base.OnInit();
        
        if(Instance == null)
            Instance = this;
    }
    #endregion
    
    #region Methods
    public void Present(string title, string message, string inputPlaceholder, System.Action<string> confirmCallback, System.Action cancelCallback, bool showClose=false)
    {
        confirm = confirmCallback;
        cancel = cancelCallback;

        InputField.PlaceholderText = inputPlaceholder;
        InputField.Text = "";

        if(showClose)
            CloseButton.Activate();
        
        TitleText.text = title;
        MessageText.text = message;
        
        UIAlertController.Instance.PresentAlert(this);//Open();
    }
    
    public void Confirm()
    {
        if (string.IsNullOrEmpty(InputField.Text) || Helper.CheckIfSpaces(InputField.Text))
        {
            DefaultAlert.Present("Sorry!", "Please input some text.", null, null);
            return;
        }

        Deactivate();//Close();

        if (confirm != null)
            confirm(InputField.Text);
        
        confirm = null;
        cancel = null;
    }
    public void Cancel()
    {
        Deactivate();//Close();
        
        if (cancel != null)
            cancel();
        
        confirm = null;
        cancel = null;
    }
    #endregion
}
