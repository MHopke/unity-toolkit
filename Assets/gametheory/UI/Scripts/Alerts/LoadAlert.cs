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
        Instance = this;
    }
    protected override void OnCleanUp()
    {
        Instance = null;
        base.OnCleanUp();
    }
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
                          float timeOut=-1f, float fillTime=1.0f, float pauseTime=0.15f)
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
    public static void SetText(string text)
    {
		if(Instance == null)
			return;
		
        Instance.LoadingText.text = text;
    }
	public static void Done()
    {
		if(Instance == null)
			return;

        if (!Instance.Loader.enabled)
            return;

        Instance.ExtraneousInfo ="";
        Instance.Loader.Done();

        Instance.timedOut = null;

        Instance.Loader.extraneousTime -= Instance.ExtranesouTime;

        Instance.Deactivate();//Close();
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
}
