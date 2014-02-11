using MiniJSON;
using System;
using System.Collections.Generic;

/// <summary>
/// GK friends list. Holds a list of GKFriend representing the user's GameCenter Friends.
/// </summary>
public class GKFriendsList
{
	static List<GKUser> friends;
	public GKFriendsList(){ friends = new List<GKUser>(); }

	/// <summary>
	/// Sets the friends list. Rewrites the list each time this is called.
	/// </summary>
	/// <param name="list">JSON formed list.</param>
	public void SetFriendsList(string list)
	{
		friends = new List<GKUser>();

		var dict = Json.Deserialize(list) as Dictionary<string,object>;

		foreach(KeyValuePair<string,object> pair in dict)
		{
			//Debug.Log(pair.Key + " " + pair.Value);
			if(!ListContains(pair.Key))
				friends.Add(new GKUser(pair.Key, (string)pair.Value));
		}
	}

	bool ListContains(string id)
	{
		for(int i =0; i < friends.Count; i++)
			if(friends[i].playerID == id)
				return true;

		return false;
	}

	public GKUser this[int index]
	{
		get { return friends [index]; }
	}
	public int Count
	{
		get { return friends.Count; }
	}
}

/// <summary>
/// GK friend. Holds the user's GameCenter display name and player id.
/// </summary>
public class GKUser
{
	public string displayName;
	public string playerID;

	public GKUser()
	{
		displayName = "Default";
		playerID = "-1";
	}

	public GKUser(string id, string name)
	{
		displayName = name;
		playerID = id;
	}
}