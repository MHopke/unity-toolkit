public enum GKTurnBasedMatchStatus { UNKNOWN = 0, OPEN, ENDED }

namespace gametheory.iOS.GameKit
{
	[System.Obsolete]
    /// <summary>
    /// Simple abstraction of the iOS GKTurnBasedMath. It is
    /// designed to be a base class which your actual "matches" inherit
    /// from.
    /// </summary>
    public class GKTurnBasedMatch
    {
        #region Private Variables
        /// <summary>
        /// Indicates if it is local player's turn.
        /// </summary>
        protected bool currentParticipant;

    	/// <summary>
        /// Unique ID of the match. Assigned by GameCenter.
        /// </summary>
    	protected string matchID;
        /// <summary>
        /// The other participant in the match. Can be changed to a list<string> to enable
        /// multiple other participants.
        /// </summary>
    	protected string participant;

    	protected GKTurnBasedMatchStatus status;
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
}