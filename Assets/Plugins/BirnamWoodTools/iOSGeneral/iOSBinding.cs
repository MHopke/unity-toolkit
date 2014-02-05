using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_IPHONE
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

	[DllImport ("__Internal")]
	static extern void iChangeToPortrait();
	[DllImport ("__Internal")]
	static extern void iChangeToLandscape();

	public static void ChangeOrientation(DeviceOrientation orientation)
	{
		#if !UNITY_EDITOR
		if(orientation == DeviceOrientation.Portrait)
			iChangeToPortrait();
		else if(orientation == DeviceOrientation.LandscapeRight)
			iChangeToLandscape();
		#endif
	}

	public static void LockOrientation(bool locked)
	{
		if(locked)
		{
			Screen.autorotateToLandscapeLeft = Screen.autorotateToPortraitUpsideDown = 
				Screen.autorotateToLandscapeRight = Screen.autorotateToPortrait = false;
		}
		else
		{
			Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.autorotateToLandscapeLeft = Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeRight = Screen.autorotateToPortrait = true;
		}
	}

	[DllImport ("__Internal")]
	static extern void iEnableBackgroundAudio();

	public static void SetProperAudioCategory()
	{
		#if !UNITY_EDITOR
		iEnableBackgroundAudio();
		#endif
	}

	[DllImport ("__Internal")]
	static extern void iResetAudio();

	public static void ResetAudioCategory()
	{
		#if !UNITY_EDITOR
		iResetAudio();
		#endif
	}
}
#endif
