using UnityEngine;
using System.Collections;

public class UIPercentBar : UISprite 
{
	#region Constants
	float SCALE_CLOSE = 0.1f;
	#endregion

	#region Public Variables
	public bool Animate = true;
	public Vector3 _maximumScale;
	#endregion

	#region Private Variable
	bool _scale;
	Vector3 _scalePerPercent;
	Vector3 _newScale;
	#endregion

	#region Init
	public override void Init(Vector2 offset,float speed)
	{
		Vector3 scale = _maximumScale - new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
		scale.Scale(new Vector3(0.01f,0.01f,0.01f));

		_scalePerPercent = scale;

		base.Init(offset,speed);
	}
	#endregion

	#region Update
	protected override void Update()
	{
		base.Update();

		if(_scale)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, _newScale, Time.deltaTime * 20.0f);

			if((transform.localScale - _newScale).magnitude <= SCALE_CLOSE)
			{
				transform.localScale = _newScale;

				_scale = false;
				enabled = false;
			}
		}
	}
	protected override bool CanDisable()
	{
		return false;
	}
	#endregion

	#region Methods
	/// <summary>
	/// Adjusts the bar by the percentage based in (out of 100.0f).
	/// </summary>
	/// <param name="percent">Percent.</param>
	public void AdjustBar(float percent)
	{
		if(Animate)
		{
			if(!enabled)
				enabled = true;

			_scale = true;

			_newScale = transform.localScale + _scalePerPercent * percent;
		} else
			transform.ScaleXYZ(_scalePerPercent * percent);
	}
	#endregion
}
