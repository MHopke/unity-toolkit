public enum GKTurnBasedMatchStatus { UNKNOWN = 0, OPEN, ENDED }

/// <summary>
/// Simple abstraction of the iOS GKTurnBasedMath. It's 
/// meant to be a base class which your actual "matchs" inherit
/// from.
/// </summary>
public class GKTurnBasedMatch
{
	#region Private Variables
	//Unique ID of the associated GameCenter Game
	protected string matchID;
	protected string participant;
	protected GKTurnBasedMatchStatus status;
	protected bool currentParticipant;
	#endregion

	#region Constructors
	public GKTurnBasedMatch()
	{
		matchID = "";
	}

	public GKTurnBasedMatch(string id)
	{
		matchID = id;
	}
	#endregion
	
	#region Accessors
	public string ID
	{
		get { return matchID; }
		set { matchID = value; }
	}
	public string Participant
	{
		get { return participant; }
		set { participant = value; }
	}
	public GKTurnBasedMatchStatus Status
	{
		get { return status; }
		set { status = value; }
	}
	#endregion
}