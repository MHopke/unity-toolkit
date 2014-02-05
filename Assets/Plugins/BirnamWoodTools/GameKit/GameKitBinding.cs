using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_IPHONE
public class GameKitBinding
{
	public static bool Enabled = false;

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
	static extern void GKDeclineInvite();

	public static void DeclineInvitation()
	{
		#if !UNITY_EDITOR
		GKDeclineInvite();
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKAcceptInvite();

	public static void AcceptInvitation()
	{
		#if !UNITY_EDITOR
		GKAcceptInvite();
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

	[DllImport ("__Internal")]
	static extern void GKCreateMatch(string friend);

	/// <summary>
	/// Creates a match with the friend's player ID
	/// </summary>
	/// <param name="friend">Friend.</param>
	public static void CreateMatch(string friend)
	{
		#if !UNITY_EDITOR
		if(Enabled)
			GKCreateMatch(friend);
		else
			iOSBinding.PopAlertWithHeaderAndText("Failed to Challenge Friend","Game Center is not enabled.");
		#endif
	}
	
	[DllImport ("__Internal")]
	static extern void GKLoadMatch(string matchID);

	public static void LoadMatch(string matchID)
	{
		#if !UNITY_EDITOR
		if(Enabled)
			GKLoadMatch(matchID);
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKLoadMatches();
	public static void LoadMatches()
	{
		#if !UNITY_EDITOR
		if(Enabled)
			GKLoadMatches();
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKAdvanceTurn();

	public static void AdvanceTurn()
	{
		#if !UNITY_EDITOR
		if(Enabled)
			GKAdvanceTurn();
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKEndGame();

	public static void EndGame()
	{
		#if !UNITY_EDITOR
		if(Enabled)
			GKEndGame();
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKQuitMatch(string id);

	public static void QuitMatch(string id)
	{
		#if !UNITY_EDITOR
		if(Enabled)
			GKQuitMatch(id);
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKRemoveMatch(string id);

	public static void RemoveMatch(string id)
	{
		#if !UNITY_EDITOR
		if(Enabled)
			GKRemoveMatch(id);
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKUpdateMatchData(byte[] data, int length);

	public static void UpdateMatchData(string data)
	{
		#if !UNITY_EDITOR
		byte[] arr = System.Text.Encoding.UTF8.GetBytes(data);
		GKUpdateMatchData(arr,arr.Length);
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKSaveMatchData(byte[] data, int length);

	public static void SaveMatchData(string data)
	{
		#if !UNITY_EDITOR
		byte[] arr = System.Text.Encoding.UTF8.GetBytes(data);
		GKSaveMatchData(arr,arr.Length);
		#endif
	}

	[DllImport ("__Internal")]
	static extern void GKLoadMatchData();

	public static void LoadMatchData()
	{
		#if !UNITY_EDITOR
		GKLoadMatchData();
		#endif
	}
}
#endif
