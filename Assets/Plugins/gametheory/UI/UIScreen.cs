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

        public Camera UICamera;

    	[HideInInspector]
        public Vector2 AspectRatio;

		public static UIScreen Instance = null;
    	#endregion

    	#region Private Variables
    	float _cameraRatio;
    	#endregion

    	#region Unity Methods
    	// Use this for initialization
    	void Awake () 
        {
    		if(Instance == null)
    		{
    			AspectRatio = new Vector2((float)Screen.width / DesignedResolution.x,
    				(float)Screen.height / DesignedResolution.y);

                float orthographicSize = UICamera.orthographicSize;

                UICamera.orthographicSize = (Screen.height / 2.0f) / 100f;

                _cameraRatio = UICamera.orthographicSize / orthographicSize;

    			Instance = this;
    		} else
    			Destroy(gameObject);
    	}
    	#endregion

    	#region Methods
    	public void AdjustRect(ref Rect rect, ScaleSettings setting)
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
    	public void AdjustTransform(Transform transform, ScaleSettings setting)
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
    	public static float CameraRatio
    	{
    		get { return Instance._cameraRatio; }
    	}
    	#endregion
    }

    /// <summary>
    /// Used to determine how a UI element should be adjusted when appearing on
    /// different resolutions.
    /// </summary>
    [System.Serializable]
    public class ScaleSettings
    {
    	public bool UseParent;
        public bool SkipPosition;
    	public bool ScaleX;
    	public bool ScaleY;
    }
}