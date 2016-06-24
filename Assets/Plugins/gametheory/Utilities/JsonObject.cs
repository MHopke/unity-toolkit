using UnityEngine;
using System.Collections.Generic;
using MiniJSON;

namespace gametheory.Utilities
{
	public class JsonObject 
	{
	    #region Constants
	    public const string INVALID = "invalid";
	    #endregion

	    #region Private Variables
	    Dictionary<string,object> _fields;
	    #endregion

	    #region Constructors
	    public JsonObject(string json)
	    {
	        //Debug.Log("data: " + json);
	        if (json != null)
	        {
	            if (json != "")
	            {
	                _fields = Json.Deserialize(json) as Dictionary<string,object>;

	                if(_fields != null)
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
	    public void SetField(string key, object field)
	    {
	        if (_fields.ContainsKey(key))
	            _fields[key] = field;
	        else
	            _fields.Add(key, field);
	    }
	    public override string ToString()
	    {
	        return Json.Serialize(_fields);
	    }
	    #endregion

	    #region Accessors
	    public Dictionary<string,object> Fields
	    {
	        get { return _fields; }
	    }
	    #endregion
	}
}