using UnityEngine;
using System.Collections;

public class OAuthManager : MonoBehaviour 
{
    #region Event
    public static event System.Action<string> receivedAuthCode;
    #endregion

    #region Private Vars
    static OAuthManager Instance = null;
    #endregion

    #region Unity Methods
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    #region iOS Callbacks
    void ReceivedAuthenticationCode(string response)
    {
        //Debug.Log("received auth code");
        if(receivedAuthCode != null)
            receivedAuthCode(response);
    }
    #endregion
}
