using UnityEngine;
using System;

namespace gametheory.iOS.GameKit
{
	[System.Obsolete]
    /// <summary>
    /// Handles callbacks from native code accessed by GameKitBinding.
    /// </summary>
    public class GameKitManager : MonoBehaviour 
    {
    	#region Events
    	/// <summary>
        /// Fires when Game Center has been authenticated (or disabled)
        /// </summary>
    	public static event Action<bool> authenticationEvent;

    	/// <summary>
        /// Fires when the local player's friend list has been retrieved.
        /// </summary>
    	public static event Action<string> retrievedFriendsEvent;

    	/// <summary>
        /// Fires when the local player's achievments have been loaded.
        /// </summary>
    	public static event Action<string> achievementsLoaded;
    	#endregion

        #region Private Vars
        static GameKitManager instance =null;
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

    	#region Callbacks
    	void Authenticated(string status)
    	{
    		string[] parsedStatus = status.Split(',');

    		bool authenticated;
    		if(parsedStatus.Length > 1)
    		{
    			authenticated = true;

    			GameKitBinding._localUser = new GKUser(parsedStatus[0], parsedStatus[1]);
    		}
    		else
    		{
    			authenticated = false;

    			GameKitBinding._localUser = new GKUser();
    		}

    		GameKitBinding.Enabled = authenticated;

    		if(authenticationEvent != null)
    			authenticationEvent(authenticated);
    	}

    	void RetrievedFriends(string json)
    	{
    		if(retrievedFriendsEvent != null)
    			retrievedFriendsEvent(json);
    	}

    	void AchievementsLoaded(string json)
    	{
    		if(achievementsLoaded != null)
    			achievementsLoaded(json);
    	}
    	#endregion
    }
}