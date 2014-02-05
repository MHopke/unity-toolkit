using MiniJSON;
using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_IPHONE
public class GKFriendsList
{
	static List<GKFriend> friends;

	Rect defaultRect;

	public GKFriendsList(){ friends = new List<GKFriend>(); }

	/// <summary>
	/// Sets the friends list. Rewrites the list each time this is called.
	/// </summary>
	/// <param name="list">JSON formed list.</param>
	public void SetFriendsList(string list, Rect displayRect)
	{
		friends = new List<GKFriend>();

		var dict = Json.Deserialize(list) as Dictionary<string,object>;

		int i = 0;
		defaultRect = displayRect;
		foreach(KeyValuePair<string,object> pair in dict)
		{
			//Debug.Log(pair.Key + " " + pair.Value);
			if(!ListContains(pair.Key))
			{
				friends.Add(new GKFriend(pair.Key, (string)pair.Value,
					displayRect,(i%2 == 0) ? "ListEven" : "SubtitleBlue"));
				displayRect.y += displayRect.height;
				i++;
			}
		}
	}

	public void Draw()
	{
		if(friends.Count == 0)
			GUI.Label(defaultRect, "You have no Game Center friends.", "ListEven");
		else
		{
			for(int i = 0; i < friends.Count; i++)
				friends[i].Draw();
		}
	}

	bool ListContains(string id)
	{
		for(int i =0; i < friends.Count; i++)
			if(friends[i].playerID == id)
				return true;

		return false;
	}

	public GKFriend this[int index]
	{
		get { return friends [index]; }
	}
	public int Count
	{
		get { return friends.Count; }
	}
}

public class GKFriend
{
	//Fired when a friend is chosen for an invitation
	public static event Action<GKFriend> friendSelectedEvent;

	public string displayName;
	public string playerID;

	//GUI rect stored for ease of use
	public Rect rect;
	public string style;

	public GKFriend(string id, string name,Rect rectangle,string styleParam)
	{
		displayName = name;
		playerID = id;
		rect = rectangle;
		style = styleParam;
	}

	public void Draw()
	{
		if(GUI.Button(rect, displayName, style))
		{
			if(friendSelectedEvent != null)
				friendSelectedEvent(this);
			//friendName = friends[i].displayName;
			//PopUpOneField.Activate("SEND AN INVITE", "Are you sure you want challenge " + friends[i].displayName + "?", InviteConfirm);
			//GameKitBinding.CreateMatch(friends[i].playerID);
		}
	}
}
#endif