using UnityEngine;
using System;

/// <summary>
/// Game kit manager. Place this on the single BWGPluginsManager GameObject.
/// </summary>
public class GameKitManager : MonoBehaviour {

	#region Events
	//Fired when Game Center has been authenticated (or disabled)
	public static event Action<bool> gameCenterAuthenticatedEvent;

	//Fired when the local player's friend list has been retrieved
	public static event Action<string> retrievedFriendsEvent;

	//Fired when the local player's achievments have been loaded
	public static event Action<string> achievementsLoaded;
	#endregion

	#region Callbacks
	void GameCenterStatus(string status)
	{
		bool authenticated;
		if(status == "true")
		{
			authenticated = true;
		}
		else
		{
			authenticated = false;
		}

		#if UNITY_IPHONE
		GameKitBinding.Enabled = authenticated;
		#endif

		if(gameCenterAuthenticatedEvent != null)
			gameCenterAuthenticatedEvent(authenticated);
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
