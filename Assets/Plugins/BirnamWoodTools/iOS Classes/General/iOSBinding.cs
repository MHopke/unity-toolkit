#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Runtime.InteropServices;

/// <summary>
/// Bridge for generic iOS calls, such as view alerts.
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

	[DllImport ("__Internal")]
	static extern void iTextToSpeech(string text);

	public static void SpeakText(string text)
	{
		#if !UNITY_EDITOR
		iTextToSpeech(text);
		#endif
	}
}
