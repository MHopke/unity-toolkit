//#define BEST_HTTP
#if BEST_HTTP
using UnityEngine;
using System.Collections.Generic;

using BestHTTP;

using MiniJSON;

namespace gametheory.Utilities
{
	public class BestHTTPHelper
	{
	    #region Constants
	    //http keys
	    const string CONTENT_TYPE = "application/json";
	    const string CONTENT_LENGTH = "Content-Length";
	    #endregion

	    #region Methods
	    public static void AppendAuthenticationHeaders(ref HTTPRequest request, string token)
	    {
	        request.AddHeader("Authorization", "Bearer " + token);
	        AppendContentHeader(ref request);
	    }
	    public static void AppendContentHeader(ref HTTPRequest request)
	    {
	        request.AddHeader("Content-Type", CONTENT_TYPE);
	    }
	    public static void AppendBody(ref HTTPRequest request, string body)
	    {
	        byte[] data = System.Text.Encoding.UTF8.GetBytes(body);
	        request.AddHeader(CONTENT_LENGTH,data.Length.ToString());
	        request.RawData = data;
	    }
	    public static string AppendQueryParameter(string url, string paramName, object param,bool first=false)
	    {
	        if (first)
	            return url += "?" + paramName + "=" + param;
	        else
	            return url += "&" + paramName + "=" + param;
	    }
	    public static List<object> ParseGETList(string json, string key)
	    {
	        Dictionary<string,object> dict = Json.Deserialize(json) as Dictionary<string,object>;

	        if (dict.ContainsKey(key))
	            return dict[key] as List<object>;
	        else
	            return null;
	    }
	    public static JsonObject ParseResponse(string json)
	    {
	        if (!string.IsNullOrEmpty(json))
	            return new JsonObject(json);
	        else
	            return null;
	    }
	    #endregion
	}
}
#endif
