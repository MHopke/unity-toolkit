using UnityEngine;
using System;

public class UIFadingSprite : UISprite 
{
	#region Events
	public event Action fadedOutEvent;
	public event Action fadedInEvent;
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

	#region Update
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
				_spriteRenderer.color = Color.Lerp(_spriteRenderer.color, original, 
					Time.deltaTime * _fadeInSettings.Speed);

				if(_spriteRenderer.color.CloseTo(original))
				{
					fading = false;

					fadeFromTrigger = _fadeOutSettings.Triggered;

					//Debug.Log("faded in");

					_spriteRenderer.color = original;

					if(fadeFromTrigger)
					{
						_fadeTriggered = false;
						enabled = false;
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
				_spriteRenderer.color = Color.Lerp(_spriteRenderer.color, _fadeOutSettings.TargetColor, 
					Time.deltaTime * _fadeOutSettings.Speed);

				if(_spriteRenderer.color.CloseTo(_fadeOutSettings.TargetColor))
				{
					fading = false;

					//Debug.Log(name + "faded out");

					_spriteRenderer.color = _fadeOutSettings.TargetColor;

					enabled = false;

					_fadeTriggered = false;

					if(fadedOutEvent != null)
						fadedOutEvent();

					//Deactivate();
				}
			} else
			{
				if(delayTimer >= _fadeOutSettings.Delay)
				{
					fading = true;
					delayTimer = 0.0f;
				}
				else
					delayTimer += Time.deltaTime;
			}
		}
	}
	protected override bool CanDisable()
	{
		return !_fadeTriggered;
	}
	#endregion

	#region Activate, Deactivate, Init Methods
	public override void Init(Vector2 offset, float speedParam)
	{
		base.Init(offset, speedParam);

		original = _spriteRenderer.color;

		if(_fadeInSettings.Speed > 0.0f)
			_spriteRenderer.color = _fadeInSettings.TargetColor;
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

	#region Fade Methods
	public void FadeIn()
	{
		if(fading || fadeIn)
			return;

		fadeIn = true;

		if(_fadeInSettings.Delay <= 0.0f)
			fading = true;

		fadeFromTrigger = false;
		_fadeTriggered = true;

		enabled = true;
	}

	public void FadeOut()
	{
		if(fading || !fadeIn)
			return;

		fadeIn = false;

		if(_fadeOutSettings.Delay <= 0.0f)
			fading = true;
			
		fadeFromTrigger = false;
		_fadeTriggered = true;

		enabled = true;
	}

	public void Reset()
	{
		fadeFromTrigger = _fadeInSettings.Triggered;

		if(_fadeInSettings.Speed > 0.0f)
			_spriteRenderer.color = _fadeInSettings.TargetColor;

		if(!fadeFromTrigger)
		{
			enabled = true;
			fadeIn = true;
			_fadeTriggered = true;
		} else
		{fadeIn = false;
			fading = false;
			_fadeTriggered = false;
		}
	}
	#endregion
}
