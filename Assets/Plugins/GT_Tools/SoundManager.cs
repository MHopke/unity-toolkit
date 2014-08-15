using UnityEngine;
using System.Collections.Generic;

namespace gametheory
{
    /// <summary>
    /// This class contains all functionality for all audio particular to the Physical
    /// Apps menu system. The SoundManager queues Sounds in a list and will play each one after
    /// another, allowing for easy sequential audio playback. Additionally, this class controls 
    /// the opening audio sequence.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
    	#region Events
    	static public event System.Action openingDoneEvent;
    	static public event System.Action audioDidFinish;
    	#endregion
    	
    	#region Enumerations
    	enum SoundState { NONE, PAUSED, CHIME, TITLE }
    	#endregion
    	
    	#region Public variables
    	public AudioClip AppTitle;  // audio clips for all particular events
    	public AudioClip PAChime;
    	public AudioClip MenuInactivity;
    	public AudioClip Rare;
    	public AudioClip Instructions;
    	public List<AudioClip> GameInactivity;
    	#endregion

    	#region Private variables
    	List<Sound> clips;  // list of sound clips - used like a queue
    	bool audioDelayed;
    	bool _restoreAudio;  // used for pausing and unpausing audio

    	Sound currentSound;
    	SoundState soundState;
    	static SoundManager instance = null;  // used to ensure a single instance of this object
    	#endregion

    	#region Unity Methods
    	void Awake()
    	{
    		if(instance == null)
    			instance = this;
    		else
    			Destroy(gameObject);
    	}

    	// method invoked when object is instantiated
        void Start () 
        {
    		clips = new List<Sound>();  // initialize sound list
    		PlayChime();
        }

    	// update is called once per frame
        void Update ()
    	{
    		if (soundState == SoundState.PAUSED)
    			return;

    		// check if there is no audio playing
    		if (!audio.isPlaying) 
    		{
    			// if there are clips to play
    			if (clips.Count > 0) 
    			{
    				// if current sound still playing, handle it
    				if(currentSound != null)
    				{
    					if(currentSound.delayTime > 0.0f)
    					{
    						currentSound.delayTime -= Time.deltaTime;
    						return;
    					}

    					if(currentSound.runTimeLoaded)
    						Resources.UnloadAsset(currentSound.clip);

    					if(currentSound.flaggedForEvent && audioDidFinish != null)
    						audioDidFinish();
    				}

    				// play next sound clip
    				currentSound = clips[0];
    				audio.clip = clips[0].clip;
    				audio.Play();
    				clips.RemoveAt(0);
    			}
    			else 
    			{
    				// otherwise handle sound states
    				if(soundState == SoundState.CHIME)
    					PlayOpeningName();
    				else if(soundState == SoundState.TITLE)
    				{
    					soundState = SoundState.NONE;

    					if(openingDoneEvent != null)
    						openingDoneEvent();
    				} 
    				else
    				{
    					// no states and no sounds - set all audio off
    					if(currentSound != null)
    					{
    						if(currentSound.delayTime > 0.0f)
    						{
    							currentSound.delayTime -= Time.deltaTime;
    							return;
    						}

    						if(currentSound.runTimeLoaded)
    							Resources.UnloadAsset(currentSound.clip);

    						if(currentSound.flaggedForEvent && audioDidFinish != null)
    							audioDidFinish();

    						currentSound = null;
    					}
    				}
    			}
    		} 
        }
    	#endregion

    	#region Audio Control Methods
    	// method to add passed Sound to Sound list
    	public static void AddClip(Sound sound)
    	{
    		instance.clips.Add(sound);
    	}
    	// method to add passed AudioClip to Sound list
    	public static void AddClip(AudioClip clip)
    	{
    		instance.clips.Add(new Sound(clip));
    	}

    	// method to clear all audio from Sound list and stop all audio from playing
    	public static void ClearOldAudio()
    	{
    		Stop();

    		instance.audioDelayed = false;

    		// unload any assets before clearing the list
    		for(int i = 0; i < instance.clips.Count; i++)
    			if(instance.clips[i].runTimeLoaded)
    				Resources.UnloadAsset(instance.clips[i].clip);

    		instance.clips.Clear();
    		instance.currentSound = null;
    	}

    	// method to stop audio
    	public static void Stop()
    	{
    		instance.audio.Stop();
    	}

    	// method to pause audio
    	public static void Pause(bool pause)
    	{
    		// pause
    		if(pause)
    		{
    			if(instance.audio.isPlaying)
    				instance._restoreAudio = true;

    			instance.audio.Pause();
    			instance.enabled = false;
    		}
    		// unpause
    		else
    		{
    			if(instance._restoreAudio)
    			{
    				instance.audio.Play();
    				instance._restoreAudio = false;
    			}

    			instance.enabled = true;
    		}
    	}

    	// method to play sound immediately - for special cases where audio is time specific (Sound type)
    	public static void PlayClipImmediately(Sound sound)
    	{
    		ClearOldAudio ();

    		AddClip (sound);
    	}

    	// method to play sound immediately - for special cases where audio is time specific (AudioClip type)
    	public static void PlayClipImmediately(AudioClip clip)
    	{
    		ClearOldAudio();

    		AddClip(clip);
    	}

    	// method to play the inactive clip
    	public void PlayInactiveSound()
    	{
    		if(GameInactivity.Count > 0)
    		{
    			int rand = Random.Range(0, GameInactivity.Count);
    			PlayClipImmediately(new Sound(GameInactivity[rand]));
    		}
    	}
    	#endregion

    	#region Opening Audio Methods
    	public void PlayChime()
    	{
    		PlayClipImmediately(new Sound(PAChime));
    		soundState = SoundState.CHIME;
    	}

    	public void PlayOpeningName()
    	{
    		PlayClipImmediately(new Sound(AppTitle));
    		soundState = SoundState.TITLE;
    	}

    	public static void SkipOpening()
    	{
    		ClearOldAudio();
    		instance.soundState = SoundState.NONE;
    	}
    	#endregion

    	#region Accessors
    	public static bool AudioIsPlaying
    	{
    		get { return instance.audio.isPlaying; }
    	}

    	public static SoundManager Instance
    	{
    		get { return instance; }
    	}
    	#endregion
    }
}