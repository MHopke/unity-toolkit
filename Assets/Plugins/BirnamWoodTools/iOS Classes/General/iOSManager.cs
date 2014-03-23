using UnityEngine;

public class iOSManager : MonoBehaviour {

	//Fired when an alertview button is pressed
	public static event System.Action alertViewDismissedEvent;

	void AlertViewDismissed(string message)
	{
		if(alertViewDismissedEvent != null)
			alertViewDismissedEvent();
	}
}
