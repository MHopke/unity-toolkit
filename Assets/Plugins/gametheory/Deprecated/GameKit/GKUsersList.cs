using MiniJSON;
using System.Collections.Generic;

namespace gametheory.iOS.GameKit
{
	[System.Obsolete]
    /// <summary>
    /// Wrapper class that holds a user's GameCenter display name and player id.
    /// </summary>
    public class GKUser
    {
        #region Public Vars
        public string DisplayName;
        public string PlayerID;
        #endregion

        #region Constructors
        public GKUser()
        {
            DisplayName = "Default";
            PlayerID = "-1";
        }

        public GKUser(string id, string name)
        {
            DisplayName = name;
            PlayerID = id;
        }
        #endregion
    }

	[System.Obsolete]
    /// <summary>
    /// Wrapper class containing GKUser list representing the local player's GameCenter friends.
    /// </summary>
    public class GKFriendsList
    {
        #region Private Vars
    	static List<GKUser> friends;
        #endregion

        #region Contructors
    	public GKFriendsList(){ friends = new List<GKUser>(); }
        #endregion

        #region Methods
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

        /// <summary>
        /// Checks if the list has a GKUser with with specified id.
        /// </summary>
        /// <returns><c>true</c>, if the user exists, <c>false</c> otherwise.</returns>
        /// <param name="id">Identifier.</param>
    	bool ListContains(string id)
    	{
    		for(int i =0; i < friends.Count; i++)
    			if(friends[i].PlayerID == id)
    				return true;

    		return false;
    	}
        #endregion

        #region Accessors
    	public GKUser this[int index]
    	{
    		get { return friends [index]; }
    	}
    	public int Count
    	{
    		get { return friends.Count; }
    	}
        #endregion
    }
}