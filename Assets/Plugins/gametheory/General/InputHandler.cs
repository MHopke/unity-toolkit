using UnityEngine;
using System;

namespace gametheory
{
    /// <summary>
    /// Watches for base input and translates it into custom events.
    /// </summary>
	[Obsolete("Use the Unity's new UI/Event System")]
	public class InputHandler : MonoBehaviour
    {
    	#region Events
        /// <summary>
        /// Delegate for sending touch events. NOTE: touchPosition is in pixels.
        /// </summary>
    	public delegate void TouchInfo(Vector2 touchPosition, int touchID);

    	static event TouchInfo touchEndEvent;
    	static event TouchInfo touchStartEvent;
    	static event TouchInfo touchStationaryEvent;

    	public delegate void TouchMoving(Vector2 touch, Vector2 deltaPos,int touchID);
    	static event TouchMoving touchMovingEvent;

        /// <summary>
        /// Fires when a swipe is detected.
        /// </summary>
    	public static event Action<bool> swipeEvent;

    	/// <summary>
        /// Fired when a shake is detected.
        /// </summary>
    	public static event Action shakeEvent;

    	/// <summary>
        /// Fires when a pinch/pull is detected. True indicates a pinch and false indicates a pull.
        /// </summary>
    	public static event Action<bool> pinchEvent;

    	/// <summary>
        /// Fires when there has been no input for _inActiveTime
        /// </summary>
    	public static event Action userInActive;
    	#endregion

    	#region Constants
    	public const int INVALID_FINGER = -1;
        /// <summary>
        /// The minimum difference (in pixels) that will trigger a swipe event.
        /// </summary>
    	public const float SWIPE_THRESHOLD = 20.0f;
    	public const float MOVE_THRESHOLD = 0.0001f;
    	#endregion

    	#region Private Variables
    	bool _shaken;
    	bool _checkShake;
    	bool _checkPinch;
        /// <summary>
        /// Determines if the class should register inactivity.
        /// </summary>
        bool _monitorActivity;

    	/// <summary>
        /// Used to determine if the Monobehaviour can be disabled when input checks such as shake, or pinch are disabled.
        /// </summary>
    	int _checkState = 0;
    	int _maxTouches = 1;

    	float _shakeResetTime;
        float _previousFingersDifference;
    	float _shakeTimer;
    	float _inActiveTime;
    	float _inputTimer;

        Vector3 _previousAcceleration;
        /// <summary>
        /// The minimum acceleration difference required to trigger a shake event.
        /// </summary>
    	Vector3 _shakeThreshold;
    	#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    	bool _mouseDown;
        Vector3 _previousMousePosition;
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
    		if(_monitorActivity)
    			_inputTimer += Time.deltaTime;

    		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
    		if(Input.GetMouseButtonDown(0))
    		{
    			_mouseDown = true;

    			_previousMousePosition = Input.mousePosition;

    			if(touchStartEvent != null)
    				touchStartEvent(Input.mousePosition,0);

    			MarkInput();
    		}
    		else if(Input.GetMouseButtonUp(0))
    		{
    			_mouseDown = false;
    			if(touchEndEvent != null)
    				touchEndEvent(Input.mousePosition,0);

    			MarkInput();
    		}
    		else if(_mouseDown)
    		{
    			if(_previousMousePosition != Input.mousePosition)
    			{
    				if(touchMovingEvent != null)
    					touchMovingEvent(Input.mousePosition,Input.mousePosition - _previousMousePosition,0);
    			}
    			else if(_previousMousePosition == Input.mousePosition)
    			{
    				if(touchStationaryEvent != null)
    					touchStationaryEvent(Input.mousePosition,0);
    			}

    			_previousMousePosition = Input.mousePosition;

    			MarkInput();
    		}

    		if(_checkShake && Input.GetMouseButtonDown(1) && shakeEvent != null)
    			shakeEvent();
    		#else
    		if(_checkShake)
    		{
    			Vector3 acc = Input.acceleration;

    			if(!_shaken)
    			{
                    float xDif = Mathf.Abs(_previousAcceleration.x - acc.x);
                    float yDif = Mathf.Abs(_previousAcceleration.y - acc.y);
                    float zDif = Mathf.Abs(_previousAcceleration.z - acc.z);
    				
    				if(shakeEvent != null && ((_shakeThreshold.x != 0.0f && xDif > _shakeThreshold.x) || 
    				                     (_shakeThreshold.y != 0.0f && yDif > _shakeThreshold.y) 
    				                     || (_shakeThreshold.z != 0.0f && zDif > _shakeThreshold.z)))
    				{
    					shakeEvent();
    					_shaken = true;

    					MarkInput();
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
    					
                _previousAcceleration = acc;
    		}

    		_maxTouches = Mathf.Min(1,Input.touchCount);

    		for(int i = 0; i < _maxTouches; i++)
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

    			if(i == 0)
    				MarkInput();
    		}

    		if(_checkPinch && pinchEvent != null && Input.touchCount == 2)
    		{
    			float distance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;

                if(_previousFingersDifference < distance)
    				pinchEvent(false);
                else if(_previousFingersDifference > distance)
    				pinchEvent(true);

                _previousFingersDifference = distance;

    			MarkInput();
    		}
    		#endif

    		if(_monitorActivity && _inputTimer >= _inActiveTime && userInActive != null)
    		{
    			_inputTimer = 0f;

    			userInActive();
    		}

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
    		_instance._previousAcceleration = Input.acceleration;

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
    			_instance._previousFingersDifference = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;

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

    	#region TouchEvent Hooks
    	public static void AddInputStart(TouchInfo method)
    	{
    		touchStartEvent += method;

    		_instance._checkState++;
    	}
    	public static void RemoveInputStart(TouchInfo method)
    	{
    		touchStartEvent -= method;

    		_instance.CheckToDisable();
    	}
    	public static void AddInputStationary(TouchInfo method)
    	{
    		touchStationaryEvent += method;

    		_instance._checkState++;
    	}
    	public static void RemoveInputStationary(TouchInfo method)
    	{
    		touchStationaryEvent -= method;

    		_instance.CheckToDisable();
    	}
    	public static void AddInputEnd(TouchInfo method)
    	{
    		touchEndEvent += method;

    		_instance._checkState++;
    	}
    	public static void RemoveInputEnd(TouchInfo method)
    	{
    		touchEndEvent -= method;

    		_instance.CheckToDisable();
    	}
    	public static void AddInputMoving(TouchMoving method)
    	{
    		touchMovingEvent += method;

    		_instance._checkState++;
    	}
    	public static void RemoveInputMoving(TouchMoving method)
    	{
    		touchMovingEvent -= method;

    		_instance.CheckToDisable();
    	}
    	#endregion

    	void CheckToDisable()
    	{
    		if(_checkState-- == 0)
    			enabled = false;
    	}
    	#endregion

    	#region Other Methods
    	public static void MarkInput()
    	{
    		_instance._inputTimer = 0f;
    	}
    	public static void SetInActiveTime(float time, bool monitor=true)
    	{
    		_instance._inActiveTime = time;

    		MonitorActivity = monitor;
    	}
    	#endregion

    	#region Methods
    	public static bool MonitorActivity
    	{
    		get { return _instance._monitorActivity; }
    		set { _instance._monitorActivity = value; }
    	}
    	#endregion
    }
}