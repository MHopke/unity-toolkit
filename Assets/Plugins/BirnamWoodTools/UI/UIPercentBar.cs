using UnityEngine;
using System.Collections;

public class UIPercentBar : UISprite 
{
	#region Public Variables
	public Vector3 _maximumScale;
	#endregion

	#region Private Variable
	bool _scale;
	Vector3 _scalePerPercent;
	Vector3 _newScale;
	#endregion

	#region Init
	public override void Init(Vector2 offset)
	{
		Vector3 scale = new Vector3(_maximumScale.x,_maximumScale.y,_maximumScale.z);
		scale.Scale(new Vector3(0.01f,0.01f,0.01f));

		_scalePerPercent = scale;
		base.Init(offset);
	}
	#endregion

	#region Update Methods
	public override void UpdateUIElement(float deltaTime, float speed)
	{
		if(_scale)
		{
			Debug.Log(Vector3.Lerp(transform.localScale, _newScale, deltaTime * 200.0f));

			if(transform.localScale == _newScale)
				_scale = false;
		}

		base.UpdateUIElement(deltaTime, speed);
	}
	#endregion

	#region Methods
	public void AdjustBar(float percent)
	{
		_newScale = transform.localScale + _scalePerPercent * percent * 100.0f;

		Debug.Log(_newScale.x);

		_scale = true;
	}
	#endregion
}
