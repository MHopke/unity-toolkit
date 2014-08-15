using UnityEngine;

namespace gametheory
{
    /// <summary>
    /// This class contains all functionality for a stored sound clip and its related
    /// information. It is used in the SoundManager class.
    /// </summary>
    public class Sound 
    {
    	#region Public Variables
    	public bool runTimeLoaded;
    	public bool flaggedForEvent;
    	public float delayTime;
    	public AudioClip clip;
    	#endregion

    	#region Constructors
    	public Sound(){}

    	public Sound(AudioClip clipIn, bool loadedAtRunTime=false, bool flagged=false, float delay=0.0f)
    	{
    		clip = clipIn;
    		runTimeLoaded = loadedAtRunTime;
    		flaggedForEvent = flagged;
    		delayTime = delay;
    	}
    	#endregion
    }
}