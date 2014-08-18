using UnityEngine;

namespace gametheory.iOS
{
    /// <summary>
    /// Handles callbacks from the native code accessed by iOSBinding.
    /// </summary>
    public class iOSManager : MonoBehaviour 
    {
        #region Events
        /// <summary>
        /// Fires when an iOS AlertView button is pressed.
        /// </summary>
        public static event System.Action alertViewDismissedEvent;
        #endregion

        #region Private Vars
        static iOSManager instance = null;
        #endregion

        #region Unity Methods
        void Awake()
        {
            if (instance == null) 
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            } 
            else
                Destroy(gameObject);
        }
        #endregion

        #region iOS Callbacks
    	void AlertViewDismissed(string message)
    	{
    		if(alertViewDismissedEvent != null)
    			alertViewDismissedEvent();
    	}
        #endregion
    }
}