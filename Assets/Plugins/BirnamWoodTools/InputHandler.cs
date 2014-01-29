#define MULTI_TOUCH
using UnityEngine;
using System;

/// <summary>
/// Watches for base input and translate to useable events
/// </summary>
public class InputHandler : MonoBehaviour
{
	#region Events
	public delegate void TouchInfo(Vector2 touchPosition, int touchID);

	public static event TouchInfo touchEnd;
	public static event TouchInfo touchStart;
	public static event TouchInfo touchStationary;

	public delegate void TouchMoving(Vector2 touch, Vector2 deltaPos,int touchID);
	public static event TouchMoving touchMoving;

	//Fired when a swipe is detected
	public static event Action<bool> swipe;

	//Fired when a shake is detected
	public static event Action shake;

	//Fired when a pinch is detected
	public static event Action<int> pinch;
	#endregion

	#region Constants
	public const int INVALID_FINGER = -1;
	public const float MOVE_THRESHOLD = 0.0001f;
	#endregion

	#region Private Variables
	bool shaken;

	bool checkShake;
	bool checkPinch;

	Vector3 oldAcceleration;
	Vector3 shakeThreshold;

	float shakeResetTime;
	float oldDistance;
	float shakeTimer;

	static InputHandler singleton = null;
	#endregion

	#region Unity Methods
	void Start()
	{
		if(singleton == null)
		{
			oldAcceleration = new Vector3(-2f,-2f,-2f);
			singleton = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
			Destroy(this.gameObject);
	}
	
    // Update is called once per frame
    void Update()
    {
		#if UNITY_EDITOR
		if(Input.GetMouseButtonUp(0) && touchStart != null)
			touchStart(Input.mousePosition,0);
		else if(Input.GetMouseButtonDown(0) && touchEnd != null)
			touchEnd(Input.mousePosition,0);

		if(checkShake && Input.GetMouseButtonDown(1))
		{
			shake();
		}
		#else
		if(checkShake)
		{
			Vector3 acc = Input.acceleration;

			if(!shaken)
			{
				float xDif = Mathf.Abs(oldAcceleration.x - acc.x);
				float yDif = Mathf.Abs(oldAcceleration.y - acc.y);
				float zDif = Mathf.Abs(oldAcceleration.z - acc.z);
				
				if(shake != null && ((shakeThreshold.x != 0.0f && xDif > shakeThreshold.x) || 
				                     (shakeThreshold.y != 0.0f && yDif > shakeThreshold.y) 
				                     || (shakeThreshold.z != 0.0f && zDif > shakeThreshold.z)))
				{
					shake();
					shaken = true;
				}
			}
			else
			{
				shakeTimer += Time.deltaTime;

				if(shakeTimer > shakeResetTime)
				{
					shakeTimer = 0.0f;
					shaken = false;
				}
			}
					
			oldAcceleration = acc;
		}

		for(int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);

			switch(touch.phase)
			{
				case TouchPhase.Began:
				if(touchStart != null)
					touchStart(touch.position, touch.fingerId);
				break;
				case TouchPhase.Ended:
				if(touchEnd != null)
					touchEnd(touch.position, touch.fingerId);
				break;
				case TouchPhase.Moved:
				if(touchMoving != null)
					touchMoving(touch.position, touch.deltaPosition, touch.fingerId);
				break;
				case TouchPhase.Stationary:
				if(touchStationary != null)
					touchStationary(touch.position, touch.fingerId);
				break;
				default:
				break;
			}

			float delta = touch.deltaPosition.x;
			if(Mathf.Abs(delta) > 20.0f)
			{
				if(swipe != null)
				{
					if(delta > 0.0f)
						swipe(true);
					else if(delta < 0.0f)
						swipe(false);
				}
			}
		}

		if(checkPinch && Input.touchCount == 2)
		{
			float distance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;

			if(oldDistance < distance && pinch != null)
				pinch(1);
			else if(oldDistance > distance && pinch != null)
				pinch(-1);

			oldDistance = distance;
		}
		#endif
    }
	#endregion

	#region Enable & Disable Methods
	public static void EnableShake(Vector3 threshold, float resetTime, Action shakeMethod)
	{
		if(singleton.checkShake)
			return;

		singleton.checkShake = true;
		singleton.shakeThreshold = threshold;
		singleton.shakeResetTime = resetTime;
		singleton.oldAcceleration = Input.acceleration;

		shake = shakeMethod;
	}
	public static void DisableShake()
	{
		if(!singleton.checkPinch)
			return;

		singleton.checkShake = false;
		singleton.shakeTimer = 0.0f;

		shake = null;
	}
	public static void EnablePinch(Action<int> pinchMethod)
	{
		if(singleton.checkPinch)
			return;

		singleton.checkPinch = true;

		if(Input.touchCount == 2)
			singleton.oldDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;

		pinch = pinchMethod;
	}
	public static void DisablePinch()
	{
		if(!singleton.checkPinch)
			return;

		singleton.checkPinch = false;

		pinch = null;
	}
	#endregion
}