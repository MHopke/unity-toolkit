using UnityEngine;
using System;

/// <summary>
/// Watches for base input and translate to useable events
/// </summary>
public class InputHandler : MonoBehaviour
{
	#region Events
	public delegate void TouchInfo(Vector2 touchPosition, int touchID);

	public static event TouchInfo touchEndEvent;
	public static event TouchInfo touchStartEvent;
	public static event TouchInfo touchStationaryEvent;

	public delegate void TouchMoving(Vector2 touch, Vector2 deltaPos,int touchID);
	public static event TouchMoving touchMovingEvent;

	//Fired when a swipe is detected
	public static event Action<bool> swipeEvent;

	//Fired when a shake is detected
	public static event Action shakeEvent;

	//Fired when a pinch is detected. False indicates a pull
	public static event Action<bool> pinchEvent;
	#endregion

	#region Constants
	public const int INVALID_FINGER = -1;
	public const float SWIPE_THRESHOLD = 20.0f;
	public const float MOVE_THRESHOLD = 0.0001f;
	#endregion

	#region Private Variables
	bool _shaken;
	bool _checkShake;
	bool _checkPinch;

	//Used to determine if the monobehaviour can be disabled
	//when input checks such as shake, or pinch are disabled;
	int _checkState = 0;

	float _shakeResetTime;
	float _oldDistance;
	float _shakeTimer;

	Vector3 _oldAcceleration;
	Vector3 _shakeThreshold;
	#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
	bool _mouseDown;
	Vector3 _oldMousePosition;
	#endif

	static InputHandler _instance = null;
	#endregion

	#region Unity Methods
	void Start()
	{
		if(_instance == null)
		{
			_instance = this;

			DontDestroyOnLoad(this.gameObject);

			//CheckToDisable();
		}
		else
			Destroy(this.gameObject);
	}
	
    // Update is called once per frame
    void Update()
    {
		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		if(Input.GetMouseButtonDown(0))
		{
			_mouseDown = true;

			_oldMousePosition = Input.mousePosition;

			if(touchStartEvent != null)
				touchStartEvent(Input.mousePosition,0);
		}
		else if(Input.GetMouseButtonUp(0))
		{
			_mouseDown = false;
			if(touchEndEvent != null)
				touchEndEvent(Input.mousePosition,0);
		}
		else if(_mouseDown)
		{
			if(_oldMousePosition != Input.mousePosition)
			{
				if(touchMovingEvent != null)
					touchMovingEvent(Input.mousePosition,Input.mousePosition - _oldMousePosition,0);
			}
			else if(_oldMousePosition == Input.mousePosition)
			{
				if(touchStationaryEvent != null)
					touchStationaryEvent(Input.mousePosition,0);
			}

			_oldMousePosition = Input.mousePosition;
		}

		if(_checkShake && Input.GetMouseButtonDown(1) && shakeEvent != null)
			shakeEvent();
		//#else
		if(_checkShake)
		{
			Vector3 acc = Input.acceleration;

			if(!_shaken)
			{
				float xDif = Mathf.Abs(_oldAcceleration.x - acc.x);
				float yDif = Mathf.Abs(_oldAcceleration.y - acc.y);
				float zDif = Mathf.Abs(_oldAcceleration.z - acc.z);
				
				if(shakeEvent != null && ((_shakeThreshold.x != 0.0f && xDif > _shakeThreshold.x) || 
				                     (_shakeThreshold.y != 0.0f && yDif > _shakeThreshold.y) 
				                     || (_shakeThreshold.z != 0.0f && zDif > _shakeThreshold.z)))
				{
					shakeEvent();
					_shaken = true;
				}
			}
			else
			{
				_shakeTimer += Time.deltaTime;

				if(_shakeTimer > _shakeResetTime)
				{
					_shakeTimer = 0.0f;
					_shaken = false;
				}
			}
					
			_oldAcceleration = acc;
		}

		for(int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);

			switch(touch.phase)
			{
			case TouchPhase.Began:
				if(touchStartEvent != null)
					touchStartEvent(touch.position, touch.fingerId);
				break;
			case TouchPhase.Ended:
				if(touchEndEvent != null)
					touchEndEvent(touch.position, touch.fingerId);
				break;
			case TouchPhase.Moved:
				if(touchMovingEvent != null)
					touchMovingEvent(touch.position, touch.deltaPosition, touch.fingerId);
				break;
			case TouchPhase.Stationary:
				if(touchStationaryEvent != null)
					touchStationaryEvent(touch.position, touch.fingerId);
				break;
			}

			float delta = touch.deltaPosition.x;
			if(Mathf.Abs(delta) > SWIPE_THRESHOLD)
			{
				if(swipeEvent != null)
				{
					if(delta > 0.0f)
					swipeEvent(true);
					else if(delta < 0.0f)
					swipeEvent(false);
				}
			}
		}

		if(_checkPinch && pinchEvent != null && Input.touchCount == 2)
		{
			float distance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;

			if(_oldDistance < distance)
				pinchEvent(false);
			else if(_oldDistance > distance)
				pinchEvent(true);

			_oldDistance = distance;
		}
		#endif
    }
	#endregion

	#region Enable & Disable Methods
	public static void EnableShake(Vector3 threshold, float resetTime, Action shakeMethod)
	{
		if(_instance._checkShake)
			return;

		_instance._checkShake = true;
		_instance._shakeThreshold = threshold;
		_instance._shakeResetTime = resetTime;
		_instance._oldAcceleration = Input.acceleration;

		shakeEvent = shakeMethod;

		_instance.enabled = true;

		_instance._checkState++;
	}
	public static void DisableShake()
	{
		if(!_instance._checkPinch)
			return;

		_instance._checkShake = false;
		_instance._shakeTimer = 0.0f;

		shakeEvent = null;

		_instance.CheckToDisable();
	}
	public static void EnablePinch(Action<bool> pinchMethod)
	{
		if(_instance._checkPinch)
			return;

		_instance._checkPinch = true;

		if(Input.touchCount == 2)
			_instance._oldDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;

		pinchEvent = pinchMethod;

		_instance.enabled = true;

		_instance._checkState++;
	}
	public static void DisablePinch()
	{
		if(!_instance._checkPinch)
			return;

		_instance._checkPinch = false;

		pinchEvent = null;

		_instance.CheckToDisable();
	}

	void CheckToDisable()
	{
		if(_checkState-- == 0)
			enabled = false;
	}
	#endregion
}