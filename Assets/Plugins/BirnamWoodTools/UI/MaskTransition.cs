#define SCALE
using UnityEngine;

public class MaskTransition : SpriteRoot 
{
	#region Events
	public static event System.Action<string> transitionStartedEvent;
	public static event System.Action<string> transitionFinishedEvent;
	#endregion

	#region Constants
	const float CLOSE_ENOUGH = 0.65f;
	#endregion

	#region Public Variables
	public float _speed;
	public Vector3 _targetPosition;

	public Vector3 _targetScale;
	#endregion

	#region Private Variables
	bool _transitionIn;

	float _moveRate;

	string _controllerId;

	Vector3 _originalPosition;
	Vector3 _originalScale;
	#endregion

	// Use this for initialization
	new void Start () 
	{
		//base.Start();

		#if SCALE
		_originalScale = transform.localScale;
		_targetScale.x = _originalScale.x;
		_targetScale.z = _originalScale.z;
		#if UNITY_IOS
		if(Screen.height == 480 && Screen.width == 320)
			_targetScale.y *= 0.88f;
		#endif
		#else
		_originalPosition = transform.position;

		_targetPosition.z = _originalPosition.z;
		#endif
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_transitionIn)
		{
			#if SCALE
			_moveRate = 1.0f / (transform.localScale - _targetScale).magnitude * Time.deltaTime;

			transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, _moveRate * _speed);

			if(Mathf.Abs((transform.localScale - _targetScale).magnitude) <= CLOSE_ENOUGH)
			{
				transform.localScale = _targetScale;
				_transitionIn = false;
				UINavigationController.NavigateToController(_controllerId);
			}
			#else
			_moveRate = 1.0f / (transform.position - _targetPosition).magnitude * Time.deltaTime;

			transform.position = Vector3.Lerp(transform.position, _targetPosition, _moveRate * _speed);

			if(Mathf.Abs((transform.position - _targetPosition).magnitude) <= CLOSE_ENOUGH)
			{
				transform.position = _targetPosition;
				_transitionIn = false;
				UINavigationController.NavigateToController(_controllerId);
				_controllerId = "";
			}
			#endif
		} else
		{
			#if SCALE
			_moveRate = 1.0f / (transform.localScale - _originalScale).magnitude * Time.deltaTime;

			transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, _moveRate * _speed);

			if(Mathf.Abs((transform.localScale - _originalScale).magnitude) <= CLOSE_ENOUGH)
			{
				transform.localScale = _originalScale;

				if(transitionFinishedEvent != null)
					transitionFinishedEvent(_controllerId);

				_controllerId = "";

				enabled = false;
			}
			#else
			_moveRate = 1.0f / (transform.position - _originalPosition).magnitude * Time.deltaTime;

			transform.position = Vector3.Lerp(transform.position, _originalPosition, _moveRate * _speed);

			if(Mathf.Abs((transform.position - _originalPosition).magnitude) <= CLOSE_ENOUGH)
			{
				transform.position = _originalPosition;

				enabled = false;
			}
			#endif
		}
	}

	public void TransitionIn(string controllerId)
	{
		if(enabled)
			transform.localScale = _originalScale;

		_controllerId = controllerId;

		enabled = true;

		_transitionIn = true;

		if(transitionStartedEvent != null)
			transitionStartedEvent(UINavigationController.CurrentController.name);
	}
}
