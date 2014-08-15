#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Runtime.InteropServices;

namespace gametheory.iOSTools
{
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