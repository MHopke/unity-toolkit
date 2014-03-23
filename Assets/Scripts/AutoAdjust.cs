using UnityEngine;
using System.Collections;

public class AutoAdjust : MonoBehaviour {

	public enum ConstraintType { PIXEL = 0, PERCENT }

	public float _zDepth;

	public ConstraintType _constraintType;

	public Rect _contraints;

	// Use this for initialization
	void Start () 
	{
		Debug.Log(Camera.main.WorldToViewportPoint(transform.position));

		if(_constraintType == ConstraintType.PERCENT)
			transform.position = Camera.main.ViewportToWorldPoint(new Vector3(_contraints.x, 1.0f - _contraints.y,_zDepth));
		else if(_constraintType == ConstraintType.PIXEL)
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_contraints.x,Screen.height - _contraints.y,_zDepth));
	}

	public void ChangeConstraints(ConstraintType type)
	{
		if(type == ConstraintType.PERCENT)
		{
			_contraints.x = _contraints.x / Screen.width;
			_contraints.y = _contraints.y / Screen.height;
		} else if(type == ConstraintType.PIXEL)
		{
			_contraints.x = _contraints.x * Screen.width;
			_contraints.y = _contraints.y * Screen.height;
		}
	}
}
