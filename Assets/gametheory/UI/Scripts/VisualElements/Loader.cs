using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace gametheory.UI
{
	/// <summary>
	/// A basic loader class. To perform the "load" animation
	/// it utilizes uGUI's radial fill.
	/// </summary>
	public class Loader : VisualElement
	{
	    #region Events
	    static event System.Action timedOut;
	    public event System.Action extraneousTime;
	    #endregion

	    #region Constants
	    const float MAX_FILL = 1.0f;
	    #endregion

	    #region Public Vars
	    public float _fillTime = 0.25f;
	    public float _waitTime = 5.0f;
	    public float _fillPause = 0.25f;
	    public float _extraneousTime = 60f;

	    public Image _emptyCircle;
	    public Image _fullCircle;
	    #endregion

	    #region Private Vars
	    bool _noTimeout;
	    bool _extraneousTimeUp;
		bool _initFill;

	    float _fillDelta;
	    float _timer;
	    float _pauseTimer;
	    #endregion

	    #region Unity Methods
	    void Update()
	    {
	        if(_fullCircle.fillAmount < MAX_FILL)
	        {
	            _fullCircle.fillAmount += _fillDelta * Time.deltaTime;
	        }
	        else
	        {
	            _pauseTimer += Time.deltaTime;

	            if(_pauseTimer >= _fillPause)
	            {
	                _pauseTimer = 0.0f;
	                _fullCircle.fillAmount = 0.0f;
	            }
	        }

	        _timer += Time.deltaTime;

	        if(_noTimeout)
	        {
	            if(!_extraneousTimeUp && _timer >= _extraneousTime)
	            {
	                if(extraneousTime != null)
	                    extraneousTime();

	                _extraneousTimeUp = true;
	            }

	        }
	        else
	        {
	            if (_timer >= _waitTime)
	            {
	                Done();

	                if (timedOut != null)
	                    timedOut();
	            }
	        }
	    }
	    #endregion

	    #region Methods
	    public void StartLoading(System.Action timeOutCallback)
		{
	        if(!_initFill)
	        {
	            _fillDelta = MAX_FILL / _fillTime;
	            _initFill = true;
	        }

			//_fillDelta = MAX_FILL / _fillTime;
	        _emptyCircle.enabled = true;
	        _fullCircle.enabled = true;

	        _extraneousTimeUp = false;

	        _fullCircle.fillAmount = 0f;

	        if (_waitTime <= 0.0f)
	            _noTimeout = true;
	        else
	            _noTimeout = false;

	        enabled = true;

	        timedOut = timeOutCallback;
	    }
	    public void Done()
	    {
	        _emptyCircle.enabled = false;
	        _fullCircle.enabled = false;
	        enabled = false;
	        
	        _fullCircle.fillAmount = 0f;

	        _timer = 0.0f;
	        _pauseTimer = 0.0f;
	    }
	    #endregion
	}
}