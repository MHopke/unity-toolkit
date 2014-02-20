using UnityEngine;
public class UIFlashComponent : UIComponent 
{
	#region Public Variables
	public Color FlashColor;
	public float TimeBetweenFlash;
	#endregion

	#region Private Variables
	bool fadeToFlash;

	Color originalColor;
	#endregion

	#region Methods
	public override void Init()
	{
		base.Init();

		if(_uiElement.GetBaseType() == typeof(UILabel))
			(_uiElement as UILabel).CreateCustomStyle();

		originalColor = _uiElement.CurrentColor;
	}
	public override void Activate()
	{
		base.Activate();

		Reset();
	}

	void Update()
	{
		if(fadeToFlash)
		{
			_uiElement.CurrentColor = Color.Lerp(_uiElement.CurrentColor, FlashColor, 
				Time.deltaTime * TimeBetweenFlash);
			if(_uiElement.CurrentColor.CloseTo(FlashColor))
				fadeToFlash = false;
		} else
		{
			_uiElement.CurrentColor = Color.Lerp(_uiElement.CurrentColor, originalColor, 
				Time.deltaTime * TimeBetweenFlash);
			if(_uiElement.CurrentColor.CloseTo(originalColor))
				fadeToFlash = true;
		}
	}
	#endregion

	#region Flash Methods
	public void Flash()
	{
		enabled = true;
	}

	public void Reset()
	{
		fadeToFlash = true;

		_uiElement.CurrentColor = originalColor;
	}
	#endregion
}
