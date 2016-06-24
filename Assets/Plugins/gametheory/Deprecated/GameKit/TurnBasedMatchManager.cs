using UnityEngine;
using System;

namespace gametheory.iOS.GameKit
{
	[System.Obsolete]
    /// <summary>
    /// Handles callbacks from the native code accessed by TurnBasedMatchBinding.
    /// </summary>
    public class TurnBasedMatchManager : MonoBehaviour 
    {
    	#region Events
    	//Fired when a player has received an invitation to a match
    	public static event Action<string> inviteReceivedEvent;

    	//Fired when the requested match data is loaded
    	public static event Action<string> matchDataLoadedEvent;

    	//Fired when match data has been successuflly saved;
    	public static event Action matchDataSavedEvent;

    	//Fired when the local player's open matches have been loaded
    	public static event Action<string> matchesLoadedEvent;

    	//Fired when a match has successfully been created
    	public static event Action<string> matchCreationSuccessEvent;

    	//Fired when a match fails being created
    	public static event Action matchCreationFailureEvent;

    	//Fired when a match endeds without an error;
    	public static event Action matchEndedEvent;

    	//Fired when a match has an error when ending
    	public static event Action matchEndedErroEvent;

    	//Fired when a match has been removed from the list
    	public static event Action<string> matchRemovedEvent;

    	//Fired when the player quits a match
    	public static event Action<string> matchQuitEvent;

    	//Fired when it becomes the local players turn in a match
    	public static event Action<string> turnNotificationEvent;

    	//Fired when the requested match is loaded
    	public static event Action<string> matchSuccessfullyLoadedEvent;

    	//Fired when the current match has successfully advanced its turn
    	public static event Action turnAdvancedSuccessEvent;

    	//Fired when the advancing the turn returns an error
    	public static event Action turnAdvancedErrorEvent;
    	#endregion

    	#region Callbacks
    	void InvitationReceived(string json)
    	{
    		if(inviteReceivedEvent != null)
    			inviteReceivedEvent(json);
    	}

    	void MatchesLoaded(string json)
    	{
    		if(matchesLoadedEvent != null)
    			matchesLoadedEvent(json);
    	}

    	void MatchCreationSuccess(string matchID)
    	{
    		if(matchCreationSuccessEvent != null)
    			matchCreationSuccessEvent(matchID);
    	}

    	void MatchCreationFailure(string junk)
    	{
    		if(matchCreationFailureEvent != null)
    			matchCreationFailureEvent();
    	}

    	void MatchEnded()
    	{
    		if(matchEndedEvent != null)
    			matchEndedEvent();
    	}

    	void MatchEndedError()
    	{
    		if(matchEndedErroEvent != null)
    			matchEndedErroEvent();
    	}

    	void MatchRemoved(string data)
    	{
    		if(matchRemovedEvent != null)
    			matchRemovedEvent(data);
    	}

    	void MatchQuit(string matchID)
    	{
    		if(matchQuitEvent != null)
    			matchQuitEvent(matchID);
    	}

    	void MatchLoaded(string json)
    	{
    		if(matchSuccessfullyLoadedEvent != null)
    			matchSuccessfullyLoadedEvent(json);
    	}

    	void TurnNotification(string json)
    	{
    		if(turnNotificationEvent != null)
    			turnNotificationEvent(json);
    	}

    	void MatchDataLoaded(string data)
    	{
    		//System.Text.Encoding.UTF8.GetString(data);
    		if(matchDataLoadedEvent != null)
    			matchDataLoadedEvent(data);
    	}

    	void MatchDataSaved(string data)
    	{
    		if(matchDataSavedEvent != null)
    			matchDataSavedEvent();
    	}

    	void TurnAdvancedSucess(string data)
    	{
    		if(turnAdvancedSuccessEvent != null)
    			turnAdvancedSuccessEvent();
    	}

    	void TurnAdvancedError(string json)
    	{
    		if(turnAdvancedErrorEvent != null)
    			turnAdvancedErrorEvent();
    	}
    	#endregion
    }
}