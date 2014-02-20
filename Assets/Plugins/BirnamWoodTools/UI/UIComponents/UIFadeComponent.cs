using UnityEngine;

public class UIFadeComponent : UIComponent 
{
	#region Events
	public event System.Action fadedOutEvent;
	public event System.Action fadedInEvent;
	#endregion

	#region Public Variables
	public FadeSettings _fadeInSettings;
	public FadeSettings _fadeOutSettings;
	#endregion

	#region Private Variables
	bool fadeIn;
	bool fading;
	bool _fadeTriggered;

	bool fadeFromTrigger;

	float delayTimer;

	Color original;
	#endregion

	#region Methods
	public override void Init()
	{
		base.Init();

		if(_uiElement.GetBaseType() == typeof(UILabel))
			(_uiElement as UILabel).CreateCustomStyle();

		original = _uiElement.CurrentColor;

		if(_fadeInSettings.Speed > 0.0f)
			_uiElement.CurrentColor = _fadeInSettings.TargetColor;
	}
	public override void Activate()
	{
		base.Activate();

		Reset();
	}

	void Update()
	{
		if(!_fadeTriggered)
			return;

		if(fadeIn)
		{
			if(!fading)
			{
				delayTimer += Time.deltaTime;

				if(delayTimer >= _fadeInSettings.Delay)
				{
					fading = true;

					delayTimer = 0.0f;
				}
			} else
			{
				//Debug.Log( name + " fading in");
				_uiElement.CurrentColor = Color.Lerp(_uiElement.CurrentColor, original, 
					Time.deltaTime * _fadeInSettings.Speed);

				if(_uiElement.CurrentColor.CloseTo(original))
				{
					fading = false;

					fadeFromTrigger = _fadeOutSettings.Triggered;

					//Debug.Log("faded in");

					_uiElement.CurrentColor = original;

					if(fadeFromTrigger)
					{
						enabled = false;
						_fadeTriggered = false;
					}
					else
						fadeIn = false;

					if(fadedInEvent != null)
						fadedInEvent();
				}
			}
		} else
		{
			if(fading)
			{
				_uiElement.CurrentColor = Color.Lerp(_uiElement.CurrentColor, _fadeOutSettings.TargetColor, 
					Time.deltaTime * _fadeOutSettings.Speed);

				if(_uiElement.CurrentColor.CloseTo(_fadeOutSettings.TargetColor))
				{
					//Debug.Log(name + "faded out");

					fading = false;

					_uiElement.CurrentColor = _fadeOutSettings.TargetColor;

					enabled = false;
					_fadeTriggered = false;

					if(fadedOutEvent != null)
						fadedOutEvent();
				}
			} else
			{
				if(delayTimer >= _fadeOutSettings.Delay)
				{
					fading = true;
					delayTimer = 0.0f;
				} else
					delayTimer += Time.deltaTime;
			}
		}
	}
	#endregion

	#region Fade Methods
	public void FadeIn()
	{
		if(fading || fadeIn)
			return;

		fadeIn = true;

		if(_fadeInSettings.Delay <= 0.0f)
			fading = true;

		_fadeTriggered = true;
		fadeFromTrigger = false;
		enabled = true;
	}

	public void FadeOut()
	{
		if(fading || !fadeIn)
			return;

		fadeIn = false;

		if(_fadeOutSettings.Delay <= 0.0f)
			fading = true;

		_fadeTriggered = true;
		fadeFromTrigger = false;
		enabled = true;
	}

	public void Reset()
	{
		fadeFromTrigger = _fadeInSettings.Triggered;

		if(_fadeInSettings.Speed > 0.0f)
			_uiElement.CurrentColor = _fadeInSettings.TargetColor;

		if(!fadeFromTrigger)
			FadeIn();
		else
		{
			fadeIn = false;
			fading = false;
			_fadeTriggered = false;
		}
	}
	#endregion
}
