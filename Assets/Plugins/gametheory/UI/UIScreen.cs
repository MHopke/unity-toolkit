using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// Handles the scaling of the UI Camera and UI elements so that they appear
    /// correctly on different resolutions and aspect ratios.
    /// </summary>
    public class UIScreen : MonoBehaviour 
    {

    	#region Public Variables
        /// <summary>
        /// The base resolution the game is being designed to be played in.
        /// </summary>
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
    		if(setting.UseParent)
    			return;

            if (!setting.SkipPosition)
            {
                transform.SetXPosition(transform.position.x * AspectRatio.x);
                transform.SetYPosition(transform.position.y * AspectRatio.y);
            }

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

    /// <summary>
    /// Used to determine how a UI element should be adjusted when appearing on
    /// different resolutions.
    /// </summary>
    [System.Serializable]
    public class ScreenSetting
    {
    	public bool UseParent;
        public bool SkipPosition;
    	public bool ScaleX;
    	public bool ScaleY;
    }
}