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
    public static LoadAlert Instance = null;
    #endregion

    #region Overriden Methods
    protected override void Init()
    {
        base.Init();

        SkipStack = true;

        Instance = this;
    }
    protected override void CleanUp()
    {
        base.CleanUp();

        Instance = null;
    }
    #endregion

    #region Methods
    public void StartLoad(string text, System.Action timeOutCallback=null, float timeOut=10.0f, float fillTime=1.0f, float pauseTime=0.15f)
    {
        LoadingText.text = text;
        
        timedOut = timeOutCallback;

        Loader._fillTime = fillTime;
        Loader._waitTime = timeOut;
        Loader._fillPause = pauseTime;

        Loader.Start(TimedOut);
        Loader.extraneousTime += ExtranesouTime;

        base.Open();
    }
    public void Done()
    {
        if (!Loader.enabled)
            return;

        ExtraneousInfo ="";
        Loader.Done();

        timedOut = null;

        Loader.extraneousTime -= ExtranesouTime;

		Close();
    }

    void TimedOut()
    {
        if(timedOut != null)
            timedOut();

        Close();
    }
    void ExtranesouTime()
    {
        LoadingText.text = ExtraneousInfo;
    }
    #endregion
}
