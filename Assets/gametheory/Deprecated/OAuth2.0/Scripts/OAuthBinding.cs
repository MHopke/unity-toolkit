using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class OAuthBinding 
{
    #region Constants
    public const char FIRST_FIELD_DELIM = '?';
    public const char FIELD_DELIM = '&';
    public const char FIELD_VALUE = '=';
    #endregion

    #region Methods
    public static void RequestAuthCode(string url)
    {
        Application.OpenURL(url);
    }
    #endregion
}

public class UriString
{
    #region Private Vars
    string uri;
    #endregion

    #region Constructors
    public UriString(string baseUri)
    {
        uri = baseUri;
    }
    #endregion

    #region Methods
    public void AddFirstField(string field, string value)
    {
        uri += "?" + field + "=" + value;
    }
    public void AddField(string field, object value)
    {
        uri += "&" + field + "=" + value;
    }
    public override string ToString()
    {
        return uri;
    }
    #endregion
}

public class UriRedirect
{
    #region Private Vars
    string _redirectUri;
    Dictionary<string,string> _fields;
    #endregion

    #region Constructors
    public UriRedirect(string response)
    {
        string[] responseParts = response.Split(OAuthBinding.FIRST_FIELD_DELIM);
        
        _redirectUri = responseParts[0];

        _fields = new Dictionary<string, string>();

        if(responseParts.Length > 1)
        {
            string[] fieldArr = responseParts[1].Split(OAuthBinding.FIELD_DELIM);

            for(int i = 0; i < fieldArr.Length;i++)
            {
                string [] parts = fieldArr[i].Split(OAuthBinding.FIELD_VALUE);

                if(parts.Length > 1)
                {
                    _fields.Add(parts[0],parts[1]);
                }
            }
        }
    }
    #endregion

    #region Methods
    public bool ContainsField(string fieldName)
    {
        return _fields.ContainsKey(fieldName);
    }
    #endregion

    #region Accessors
    public string RedirectUri
    {
        get { return _redirectUri; }
    }
    public string this[string key]
    {
        get {return (_fields.ContainsKey(key)) ? _fields[key] : "";}
    }
    #endregion
}
