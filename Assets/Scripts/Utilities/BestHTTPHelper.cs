#define BEST_HTTP
#if BEST_HTTP

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using MiniJSON;

namespace gametheory.Utilities
{
    public class BestHTTPHelper : MonoBehaviour
	{
	    #region Constants
	    //http keys
	    const string CONTENT_TYPE = "application/json";
	    const string CONTENT_LENGTH = "Content-Length";
        #endregion

        #region Public Vars
        public static BestHTTPHelper Instance;
        #endregion

        #region Unity Methods
        void Start()
        {
            enabled = false;

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
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
        public IEnumerator CallToServer(string url, HTTPMethods method, Dictionary<string, string> dictParameters, WWWForm formParameters, List<HTTPTuple> tupleParameters, Action<Dictionary<string, object>> successCallback = null, Action<string> requestNotOKCallback = null, Action<string> failureCallback = null, Action requestFailureCallback = null)
        {
            HTTPRequest request = new HTTPRequest(new Uri(url), method);
            request.SetHeader("Accept", "application/json");
            if (dictParameters != null)
            {
                foreach (KeyValuePair<string, string> parameter in dictParameters)
                {
                    request.AddField(parameter.Key, parameter.Value);
                }
            }
            if (formParameters != null)
                request.SetFields(formParameters);
            if (tupleParameters != null)
            {
                for (int i = 0; i < tupleParameters.Count; i++)
                {
                    request.AddField(tupleParameters[i].Key, tupleParameters[i].Value);
                }
            }
            request.Send();
            yield return StartCoroutine(request);

            if (request.State == HTTPRequestStates.Finished)
            {
                if (request.Response.IsSuccess)
                {
                    Dictionary<string, object> dict = Json.Deserialize(request.Response.DataAsText) as Dictionary<string, object>;
                    if (dict["status"].ToString() == "ok")
                    {
                        if (successCallback != null)
                            successCallback(dict);
                    }
                    else
                    {
                        if (requestNotOKCallback != null)
                            requestNotOKCallback(request.Response.DataAsText);
                    }
                }
                else
                {
                    if (failureCallback != null)
                        failureCallback(request.Response.DataAsText);
                }
            }
            else
            {
                if (requestFailureCallback != null)
                    requestFailureCallback();
            }
        }
        #endregion
    }

    public class HTTPTuple
    {
        public string Key;
        public string Value;

        public HTTPTuple(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
#endif