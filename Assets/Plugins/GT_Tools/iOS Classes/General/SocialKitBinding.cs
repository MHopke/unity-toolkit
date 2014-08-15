#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Runtime.InteropServices;

namespace gametheory.iOSTools.Social
{
    public static class SocialKitBinding 
    {
    	[DllImport ("__Internal")]
    	static extern void iPostToTwitter(string message);

    	public static void PostToTwitter(string message)
    	{
    		#if !UNITY_EDITOR
    		iPostToTwitter(message);
    		#endif
    	}

    	[DllImport ("__Internal")]
    	static extern void iPostToFacebook(string message);

    	public static void PostToFacebook(string message)
    	{
    		#if !UNITY_EDITOR
    		iPostToFacebook(message);
    		#endif
    	}
    }
}