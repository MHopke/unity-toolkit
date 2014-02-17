using UnityEngine;

public class UIFlashingLabel : UILabel {

	#region Public Variables
	public Color FlashColor;
	public float TimeBetweenFlash;
	#endregion

	#region Private Variables
	bool flashing;
	bool fadeToFlash;

	Color originalColor;
	#endregion

	#region Update Methods
	protected override void Update()
	{
		base.Update();

		if(flashing)
		{
			if(fadeToFlash)
			{
				customStyle.style.normal.textColor = Color.Lerp(customStyle.style.normal.textColor, FlashColor, 
					Time.deltaTime * TimeBetweenFlash);
				if(customStyle.style.normal.textColor.CloseTo(FlashColor))
					fadeToFlash = false;
			} else
			{
				customStyle.style.normal.textColor = Color.Lerp(customStyle.style.normal.textColor, originalColor, 
					Time.deltaTime * TimeBetweenFlash);
				if(customStyle.style.normal.textColor.CloseTo(originalColor))
					fadeToFlash = true;
			}
		}
	}
	#endregion

	#region Activate, Deactivate, Init Methods
	public override void Init(Vector2 offset, float speed)
	{
		base.Init(offset, speed);

		originalColor = customStyle.style.normal.textColor;
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

	#region Flash Methods
	public void Flash()
	{
		flashing = true;
		enabled = true;
	}

	public void Reset()
	{
		fadeToFlash = true;
		flashing = false;

		customStyle.style.normal.textColor = originalColor;
	}
	#endregion
}
