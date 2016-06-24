using UnityEngine;
using UnityEngine.UI;
using gametheory.UI;
using System.Collections;

public class LoadAlert : UIAlert 
{
    #region Events
    System.Action timedOut;
    #endregion

    #region Public Vars
    public string ExtraneousInfo;
    public Text LoadingText;
    public Loader Loader;
    #endregion

    #region Overriden Methods
    protected override void OnShow()
    {
        base.OnShow();

        //indicates loading timed out or is done
        if(!Loader.enabled)
            Deactivate();
    }
    #endregion

    #region Methods
    public void StartLoad(string text, System.Action timeOutCallback=null, 
                          float timeOut=10.0f, float fillTime=1.0f, float pauseTime=0.15f)
    {
        UIAlertController.Instance.PresentAlert(this);//base.Open();

        LoadingText.text = text;
        
        timedOut = timeOutCallback;

        Loader._fillTime = fillTime;
        Loader._waitTime = timeOut;
        Loader._fillPause = pauseTime;

        Loader.StartLoading(TimedOut);
        Loader.extraneousTime += ExtranesouTime;
    }
    public void SetText(string text)
    {
        LoadingText.text = text;
    }
    public void Done()
    {
        if (!Loader.enabled)
            return;

        ExtraneousInfo ="";
        Loader.Done();

        timedOut = null;

        Loader.extraneousTime -= ExtranesouTime;

        Deactivate();//Close();
    }

    void TimedOut()
    {
        if(timedOut != null)
            timedOut();

        Deactivate();//Close();
    }
    void ExtranesouTime()
    {
        LoadingText.text = ExtraneousInfo;
    }
    #endregion

	#region Properties
	public static LoadAlert Instance
	{
		get { return UIView.Load("Alerts/LoadAlert") as LoadAlert; }
	}
	#endregion
}
