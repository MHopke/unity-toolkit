using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    #region Public Vars
    public RectTransform panel;
    #endregion

    #region Private Vars
    Rect _lastSafeArea;
    #endregion

    #region Unity Methods
    void Awake()
    {
    	#if UNITY_EDITOR
    	enabled = false;
    	#elif UNITY_IOS
        //Debug.Log(UnityEngine.iOS.Device.generation);
        if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX 
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneUnknown
            || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.Unknown
           )
        {
            enabled = true;
        }
        else
            enabled = false;
        #endif
    }
    void Update()
    {
        Rect safeArea = Screen.safeArea;

        if (safeArea != _lastSafeArea)
            ApplySafeArea(safeArea);
    }
    #endregion

    #region Methods
    void ApplySafeArea(Rect area)
    {
        var anchorMin = area.position;
        var anchorMax = area.position + area.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

        _lastSafeArea = area;
    }
    #endregion
}
