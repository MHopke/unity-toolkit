using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// This class acts as an additional abstraction of Unity's Screen class.
    /// It is designed to be utilized by any UI/auto sizing objects. 
    /// </summary>
    public class UIScreen : MonoBehaviour {

    	#region Public Variables
        public Vector2 DesignedResolution;

        public Camera _uiCamera;

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

                float orthographicSize = _uiCamera.orthographicSize;

                _uiCamera.orthographicSize = (Screen.height / 2.0f) / 100f;

                _cameraRatio = _uiCamera.orthographicSize / orthographicSize;

    			instance = this;
    		} else
    			Destroy(gameObject);
    	}
    	#endregion

    	#region Methods
    	public static void AdjustRect(ref Rect rect, ScreenSetting setting)
    	{
    		rect.x *= AspectRatio.x;
    		rect.y *= AspectRatio.y;

    		if(setting.ScaleX)
    			rect.width *= AspectRatio.x;
    		else
    			rect.width *= CameraRatio;

    		if(setting.ScaleY)
    			rect.height *= AspectRatio.y;
    		else
    			rect.height *= CameraRatio;
    	}
    	public static void AdjustTransform(Transform transform, ScreenSetting setting)
    	{
            transform.SetXPosition(transform.position.x * AspectRatio.x);
            transform.SetYPosition(transform.position.y * AspectRatio.y);

    		if(setting.UseParent)
    			return;

    		if(setting.ScaleX)
    			transform.ScaleX(AspectRatio.x);
    		else
    			transform.ScaleX(CameraRatio);

    		if(setting.ScaleY)
    			transform.ScaleY(AspectRatio.y);
    		else
    			transform.ScaleY(CameraRatio);
    	}
    	#endregion

    	#region Accessors
    	public static Vector2 AspectRatio
    	{
    		get { return instance._aspectRatio; }
    	}
    	public static float CameraRatio
    	{
    		get { return instance._cameraRatio; }
    	}
        public static Camera UICamera
        {
            get { return instance._uiCamera; }
        }
    	#endregion
    }

    [System.Serializable]
    public class ScreenSetting
    {
    	public bool UseParent;
    	public bool ScaleX;
    	public bool ScaleY;
    }
}