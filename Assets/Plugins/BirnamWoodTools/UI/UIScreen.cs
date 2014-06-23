using UnityEngine;

/// <summary>
/// This class acts as an additional abstraction of Unity's Screen class.
/// It's designed to be utilized with any UI/auto sizing objects.
/// </summary>
public class UIScreen : MonoBehaviour {

	#region Public Variables
	public bool _portrait;

	public float DesignedCameraSize;

    public Vector2 DesignedResolution;

	[HideInInspector]
    public Vector2 _aspectRatio;
	#endregion

	#region Private Variables
	float _cameraRatio;
	static UIScreen instance = null;
	#endregion

	#region Unity Methods
	// Use this for initialization
	void Awake () 
    {
		if(instance == null)
		{
			_aspectRatio = new Vector2((float)Screen.width / DesignedResolution.x,
				(float)Screen.height / DesignedResolution.y);

			Camera.main.orthographicSize = (Screen.height / 2.0f) / 100f;/*(100f * ((_portrait) ? _aspectRatio.y : _aspectRatio.x))*/;

			_cameraRatio = Camera.main.orthographicSize / DesignedCameraSize;

			//animation.clip.

			instance = this;
		} else
			Destroy(gameObject);
	}
	#endregion

	#region Methods
	public static void AdjustForResolution(ref Rect rect, ScreenSetting setting)
	{
		rect.x *= AspectRatio.x;
		rect.y *= AspectRatio.y;

		if(setting.StretchX)
			rect.width *= AspectRatio.x;
		else
			rect.width *= instance._cameraRatio;

		if(setting.StretchY)
			rect.height *= AspectRatio.y;
		else
			rect.height *= instance._cameraRatio;
	}
	public static void AdjustForResolution(Transform transform, ScreenSetting setting)
	{
		if(setting.UseParent)
			return;

		if(setting.StretchX)
			transform.ScaleX(AspectRatio.x);
		else
			transform.ScaleX(instance._cameraRatio);

		if(setting.StretchY)
			transform.ScaleY(AspectRatio.y);
		else
			transform.ScaleY(instance._cameraRatio);
	}
	#endregion

	#region Accessors
	public static bool Portrait
	{
		get { return instance._portrait; }
	}
	public static Vector2 AspectRatio
	{
		get { return instance._aspectRatio; }
	}
	public static float CameraRatio
	{
		get { return instance._cameraRatio; }
	}
	#endregion
}

[System.Serializable]
public class ScreenSetting
{
	public bool UseParent;
	public bool StretchX;
	public bool StretchY;
}
