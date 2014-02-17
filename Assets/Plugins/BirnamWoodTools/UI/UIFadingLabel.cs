using UnityEngine;

public class UIFadingLabel : UILabel 
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

	#region Activate, Deactivate, Init Methods
	public override void Init(Vector2 offset, float speed)
	{
		base.Init(offset, speed);

		if(!customStyle.custom)
		{
			customStyle.custom = true;
			customStyle.style = new GUIStyle(UINavigationController.Skin.FindStyle(customStyle.styleName));
		}

		original = customStyle.style.normal.textColor;

		if(_fadeInSettings.Speed > 0.0f)
			customStyle.style.normal.textColor = _fadeInSettings.TargetColor;
	}
	public override bool Activate(MovementState state = MovementState.INITIAL)
	{
		if(base.Activate(state))
		{
			Reset();
			return true;
		} else
			return false;
	}
	#endregion

	#region Update Methods
	protected override void Update()
	{
		base.Update();

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
				customStyle.style.normal.textColor = Color.Lerp(customStyle.style.normal.textColor, original, 
					Time.deltaTime * _fadeInSettings.Speed);

				if(customStyle.style.normal.textColor.CloseTo(original))
				{
					fading = false;

					fadeFromTrigger = _fadeOutSettings.Triggered;

					//Debug.Log("faded in");

					customStyle.style.normal.textColor = original;

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
				customStyle.style.normal.textColor = Color.Lerp(customStyle.style.normal.textColor, _fadeOutSettings.TargetColor, 
					Time.deltaTime * _fadeOutSettings.Speed);

				if(customStyle.style.normal.textColor.CloseTo(_fadeOutSettings.TargetColor))
				{
					//Debug.Log(name + "faded out");

					fading = false;

					customStyle.style.normal.textColor = _fadeOutSettings.TargetColor;

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
	protected override bool CanDisable()
	{
		return !_fadeTriggered;
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
			customStyle.style.normal.textColor = _fadeInSettings.TargetColor;

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
