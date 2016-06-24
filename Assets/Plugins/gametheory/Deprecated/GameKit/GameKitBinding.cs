using System.Runtime.InteropServices;

namespace gametheory.iOS.GameKit
{
	[System.Obsolete]
    /// <summary>
    /// Provides access to iOS' GameKit functionality.
    /// </summary>
    public class GameKitBinding
    {
    	#region Public Variables
    	public static bool Enabled = false;
    	#endregion

    	#region Private Variables
    	public static GKUser _localUser;
    	#endregion

    	#region External Methods
    	[DllImport ("__Internal")]
    	static extern void GKAuthenticatePlayer();

    	public static void AuthenticatePlayer()
    	{
    		#if !UNITY_EDITOR
    		GKAuthenticatePlayer();
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKRetrieveFriends();
    	
    	public static void RetrieveFriends()
    	{
    		#if !UNITY_EDITOR
    		GKRetrieveFriends();
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKPostLeaderboardScore(int score, string leaderboardIdentifier);

    	public static void PostLeaderboardScore(int score,string leaderboardIdentifier)
    	{
    		#if !UNITY_EDITOR
    		if(Enabled)
    			GKPostLeaderboardScore(score,leaderboardIdentifier);
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKViewLeaderboard(string leaderboardIdentifier);

    	public static void ViewLeaderboard(string leaderboardIdentifier)
    	{
    		#if !UNITY_EDITOR
    		if(Enabled)
    			GKViewLeaderboard(leaderboardIdentifier);
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void GKReportAchievement(string identifier, float progress);

    	public static void ReportAchievement(string identifier, float progress)
    	{
    		#if !UNITY_EDITOR
    		if(Enabled)
    			GKReportAchievement(identifier,progress);
    		#endif
    	}
    	#endregion
    }
}