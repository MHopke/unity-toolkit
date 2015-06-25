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
    public Text _loadingText;
    public Loader _loader;
    public static LoadAlert Instance = null;
    #endregion

    #region Overriden Methods
    protected override void Init()
    {
        base.Init();

        _skipStack = true;

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
        _loadingText.text = text;
        
        timedOut = timeOutCallback;

        _loader._fillTime = fillTime;
        _loader._waitTime = timeOut;
        _loader._fillPause = pauseTime;

        _loader.Start(TimedOut);
        _loader.extraneousTime += ExtranesouTime;

        base.Open();
    }
    public void Done()
    {
        if (!_loader.enabled)
            return;

        ExtraneousInfo ="";
        _loader.Done();

        timedOut = null;

        _loader.extraneousTime -= ExtranesouTime;

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
        _loadingText.text = ExtraneousInfo;
    }
    #endregion
}
