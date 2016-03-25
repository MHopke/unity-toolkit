using UnityEngine;

namespace gametheory.Audio
{
    /// <summary>
    /// This class contains all functionality for a stored AudioClip and its related
    /// information. It is utilized by the SoundManager class to create a queue of Sounds.
    /// </summary>
    public class Sound 
    {
        #region Events
        /// <summary>
        /// Fires when this sound finishes playing, but only if the flaggedForEvent boolean is set.
        /// </summary>
        static public event System.Action audioDidFinish;
        #endregion

    	#region Private Var
        /// <summary>
        /// Was _clip loaded from during runtime?
        /// </summary>
    	bool _runTimeLoaded;
        /// <summary>
        /// Should this Sound trigger the audioFinished event.
        /// </summary>
    	bool _flaggedForEvent;

        /// <summary>
        /// The time (in seconds) to delay playing this Sound.
        /// </summary>
    	float _delayTime;

    	AudioClip _clip;
    	#endregion

    	#region Constructors
    	public Sound(){}

    	public Sound(AudioClip clipIn, bool loadedAtRunTime=false, bool flagged=false, float delay=0.0f)
    	{
    		_clip = clipIn;
    		_runTimeLoaded = loadedAtRunTime;
    		_flaggedForEvent = flagged;
    		_delayTime = delay;
    	}
    	#endregion

        #region Methods
        /// <summary>
        /// "Finishes" the Sound by checks if _clip should be unloaded and if the Sound should fire its event.
        /// </summary>
        public void Finish()
        {
            Clear();

            if (_flaggedForEvent && audioDidFinish != null)
                audioDidFinish();
        }
        /// <summary>
        /// Checks if _clip should be unloaded
        /// </summary>
        public void Clear()
        {
            if (_runTimeLoaded)
                Resources.UnloadAsset(_clip);
        }
        #endregion

        #region Accessors
        public float DelayTime
        {
            get { return _delayTime; }
            set { _delayTime = value; }
        }

        public AudioClip Clip
        {
            get { return _clip; }
        }
        #endregion
    }
}