using UnityEngine;
using System.Collections;

using gametheory.UI;

public class AlphaToggle : ExtendedToggle 
{
	#region Constants
	const float OFF_SCALE = 0.75f;
	const float ON_SCALE = 1f;
	const float OFF_ALPHA = 0.5f;
	const float ON_ALPHA = 1f;
	#endregion

	#region Public Vars
	public bool Scale=true;
	public bool BaseFades=true;
	#endregion

	#region Overridden Methods
	protected override void OnActivate ()
	{
		base.OnActivate ();

		if(Toggle.isOn)
			SetOn();
		else
			SetOff();
	}
	#endregion

	#region UI Methods
	public void ToggleStatus(bool toggled)
	{
		if(toggled)
		{
			SetOn();
			ToggledOn();
		}
		else
		{
			SetOff();
			ToggledOff();
		}
	}
	#endregion

	#region Methods
	protected virtual void ToggledOn(){}
	protected virtual void ToggledOff(){}

	void SetOn()
	{
		if(Scale)
			transform.SetXYScale(ON_SCALE,ON_SCALE);

		if(Icon)
			Icon.color = UnityExtensions.SetAlpha(Icon.color,ON_ALPHA);

		if(BaseFades)
			Toggle.targetGraphic.color = UnityExtensions.SetAlpha(Toggle.targetGraphic.color,ON_ALPHA);
	}
	void SetOff()
	{
		if(Scale)
			transform.SetXYScale(OFF_SCALE,OFF_SCALE);

		if(Icon)
			Icon.color = UnityExtensions.SetAlpha(Icon.color,OFF_ALPHA);

		if(BaseFades)
			Toggle.targetGraphic.color = UnityExtensions.SetAlpha(Toggle.targetGraphic.color,OFF_ALPHA);
	}
	#endregion
}
