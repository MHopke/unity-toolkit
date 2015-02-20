using UnityEngine;
using System.Collections.Generic;
using MiniJSON;

public class JsonResponse 
{
    #region Constants
    public const string INVALID = "invalid";
    #endregion

    #region Private Variables
    Dictionary<string,object> _fields;
    #endregion

    #region Constructors
    public JsonResponse(string json)
    {
        //Debug.Log("data: " + json);
        if (json != null)
        {
            if (json != "")
            {
                _fields = Json.Deserialize(json) as Dictionary<string,object>;

                return;
            }
        }

        _fields = new Dictionary<string, object>();
    }
    #endregion

    #region Methods
    public bool HasField(string key)
    {
        return _fields.ContainsKey(key);
    }
    public T GetField<T>(string key)
    {
        if (_fields.ContainsKey(key))
        {
            return (T)_fields[key];
        }

        return default(T);
    }
    public object GetField(string key)
    {
        if (_fields.ContainsKey(key))
            return _fields[key];

        return INVALID;
    }
    #endregion
}
