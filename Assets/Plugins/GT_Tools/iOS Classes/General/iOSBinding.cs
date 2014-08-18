#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Runtime.InteropServices;

namespace gametheory.iOS
{
    /// <summary>
    /// Provides access to general iOS functionality such as view alerts.
    /// </summary>
    public static class iOSBinding
    {
    	[DllImport ("__Internal")]
    	static extern void iPopAlertWithHeaderAndText(string header, string text);

    	public static void PopAlertWithHeaderAndText(string header, string text)
    	{
    		#if !UNITY_EDITOR
    		iPopAlertWithHeaderAndText(header,text);
    		#else
    		Debug.Log(header + ": " + text);
    		#endif
    	}
    }
}