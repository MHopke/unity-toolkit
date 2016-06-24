using System.Runtime.InteropServices;

namespace gametheory.iOS.GameKit
{
	[System.Obsolete]
    /// <summary>
    /// Provides access to iOS' Turnbased Match functionality.
    /// </summary>
    public class TurnBasedMatchBinding
    {
        #region Methods
    	[DllImport ("__Internal")]
    	static extern void GKDeclineInvite();

    	public static void DeclineInvitation()
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE && UNITY_IPHONE
    		GKDeclineInvite();
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKAcceptInvite();

    	public static void AcceptInvitation()
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		GKAcceptInvite();
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKCreateMatch(string friend);

    	/// <summary>
    	/// Creates a match with the friend's player ID
    	/// </summary>
    	/// <param name="friend">Friend.</param>
    	public static void CreateMatch(string friend)
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		if(GameKitBinding.Enabled)
    			GKCreateMatch(friend);
    		else
    			iOSBinding.PopAlertWithHeaderAndText("Failed to Challenge Friend","Game Center is not enabled.");
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKLoadMatch(string matchID);

    	public static void LoadMatch(string matchID)
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		if(GameKitBinding.Enabled)
    			GKLoadMatch(matchID);
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKLoadMatches();
    	public static void LoadMatches()
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		if(GameKitBinding.Enabled)
    			GKLoadMatches();
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKAdvanceTurn();

    	public static void AdvanceTurn()
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		if(GameKitBinding.Enabled)
    			GKAdvanceTurn();
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKEndGame();

    	public static void EndGame()
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		if(GameKitBinding.Enabled)
    			GKEndGame();
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKQuitMatch(string id);

    	public static void QuitMatch(string id)
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		if(GameKitBinding.Enabled)
    			GKQuitMatch(id);
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKRemoveMatch(string id);

    	public static void RemoveMatch(string id)
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		if(GameKitBinding.Enabled)
    			GKRemoveMatch(id);
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKUpdateMatchData(byte[] data, int length);

    	public static void UpdateMatchData(string data)
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		byte[] arr = System.Text.Encoding.UTF8.GetBytes(data);
    			GKUpdateMatchData(arr,arr.Length);
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKSaveMatchData(byte[] data, int length);

    	public static void SaveMatchData(string data)
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		byte[] arr = System.Text.Encoding.UTF8.GetBytes(data);
    			GKSaveMatchData(arr,arr.Length);
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKLoadMatchData();

    	public static void LoadMatchData()
    	{
    		#if !UNITY_EDITOR && UNITY_IPHONE
    		GKLoadMatchData();
    		#endif
    	}
        #endregion
    }
}