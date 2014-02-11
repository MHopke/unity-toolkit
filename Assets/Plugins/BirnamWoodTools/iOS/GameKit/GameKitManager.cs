using UnityEngine;
using System;

/// <summary>
/// Game kit manager. Place this on the single BWGPluginsManager GameObject.
/// </summary>
public class GameKitManager : MonoBehaviour {

	#region Events
	//Fired when Game Center has been authenticated (or disabled)
	public static event Action<bool> authenticationEvent;

	//Fired when the local player's friend list has been retrieved
	public static event Action<string> retrievedFriendsEvent;

	//Fired when the local player's achievments have been loaded
	public static event Action<string> achievementsLoaded;
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
