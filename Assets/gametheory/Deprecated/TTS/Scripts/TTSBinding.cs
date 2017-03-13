#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Runtime.InteropServices;

namespace gametheory.iOS
{
	[System.Obsolete]
    /// <summary>
    /// Provides access to iOS 7's text-to-speech functionality.
    /// </summary>
    public static class TTSBinding 
    {
    	[DllImport ("__Internal")]
    	static extern void iTextToSpeech(string text);

    	public static void SpeakText(string text)
    	{
    		#if !UNITY_EDITOR
    		iTextToSpeech(text);
    		#endif
    	}
    }
}