using UnityEngine;

/// <summary>
/// Representation of a percent bar.
/// </summary>
public class UIPercentBar : GUITexture 
{
	#region Constants
	float SCALE_CLOSE = 0.1f;
	#endregion

	#region Public Variables
	public bool _canAnimate = true;
	public bool _scaleX;
	public bool _scaleY;
	public bool _scaleZ;

	public Vector3 _maximumScale;
	public Vector3 _scaleToAdd;
	#endregion

	#region Private Variable
	Vector3 _scalePerPercent;
	Vector3 _newScale;
	#endregion

	#region Init
	protected override void OnInit()
	{
		base.OnInit();

		Vector3 scale = _maximumScale - new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		scale.Scale(new Vector3(0.01f, 0.01f, 0.01f));

		_scalePerPercent = scale;
	}
	#endregion

	#region Update
	void Update()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, _newScale, Time.deltaTime * 20.0f);

		if((transform.localScale - _newScale).magnitude <= SCALE_CLOSE)
		{
			transform.localScale = _newScale;

			enabled = false;
		}
	}
	#endregion

	#region Methods
	/// <summary>
	/// Adjusts the bar by the percentage passed in (range of 0.0f - 100.0f).
	/// </summary>
	/// <param name="percent">Percent.</param>
	public void ModifyPercent(float percent)
	{
		_scaleToAdd = _scalePerPercent * percent;

		if(!_scaleX)
			_scaleToAdd.x = 0.0f;
		if(!_scaleY)
			_scaleToAdd.y = 0.0f;
		if(!_scaleZ)
			_scaleToAdd.z = 0.0f;

		if(_canAnimate)
		{
			if(!enabled)
				enabled = true;

			_newScale = transform.localScale + _scaleToAdd;
		} else
			transform.AddXYZScale(_scaleToAdd);
	}
	/// <summary>
	/// Sets the bar's percentage.
	/// </summary>
	/// <param name="percent">Percent.</param>
	public void SetPercent(float percent)
	{
		_scaleToAdd = _scalePerPercent * percent;

		if(!_scaleX)
			_scaleToAdd.x = transform.localScale.x;
		if(!_scaleY)
			_scaleToAdd.y = transform.localScale.y;
		if(!_scaleZ)
			_scaleToAdd.z = transform.localScale.z;

		transform.SetXYZScale(_scaleToAdd);
	}
	#endregion
}
