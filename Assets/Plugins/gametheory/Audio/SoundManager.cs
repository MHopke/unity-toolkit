using UnityEngine;
using System.Collections.Generic;

namespace gametheory.Audio
{
    /// <summary>
    /// Queues Sounds in a list and will play each one after
    /// another, allowing for easy sequential audio playback. Additionally, this is
    /// setup to enable an opening audio sequence.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
    	#region Events
        /// <summary>
        /// Fires when the opening audio sequence is complete
        /// </summary>
    	static public event System.Action openingDoneEvent;
    	#endregion
    	
    	#region Enumerations
    	enum SoundState { NONE, PAUSED, CHIME, TITLE }
    	#endregion
    	
    	#region Public variables
    	public AudioClip AppTitle;
    	public AudioClip OpeningChime;
    	public AudioClip MenuInactivity;
    	public AudioClip Rare;
    	public AudioClip Instructions;
    	public List<AudioClip> GameInactivity;
    	#endregion

    	#region Private variables
        /// <summary>
        /// The list of clips that are queued to play. Does not use a queue because it needs to be
        /// looped through to unload assets when clearing the list.
        /// </summary>
        List<Sound> _clips;
        /// <summary>
        /// Checked against to see if the game should continuing playing audio after it has been paused.
        /// </summary>
    	bool _restoreAudio;

        Sound _currentSound;

        SoundState _soundState;

    	static SoundManager instance = null;  // used to ensure a single instance of this object
    	#endregion

    	#region Unity Methods
    	void Awake()
    	{
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
    		else
    			Destroy(gameObject);
    	}
            
        void Start () 
        {
    		_clips = new List<Sound>();  // initialize sound list
    		PlayChime();
        }
            
        void Update ()
    	{
    		if (_soundState == SoundState.PAUSED)
    			return;

    		// check if there is no audio playing
    		if (!GetComponent<AudioSource>().isPlaying) 
    		{
    			// if there are clips to play
    			if (_clips.Count > 0) 
    			{
    				// if current sound still playing, handle it
    				if(_currentSound != null)
    				{
    					if(_currentSound.DelayTime > 0.0f)
    					{
    						_currentSound.DelayTime -= Time.deltaTime;
    						return;
    					}

                        _currentSound.Finish();
    				}

    				// play next sound clip
    				_currentSound = _clips[0];
    				GetComponent<AudioSource>().clip = _clips[0].Clip;
    				GetComponent<AudioSource>().Play();
    				_clips.RemoveAt(0);
    			}
    			else 
    			{
    				// otherwise handle sound states
    				if(_soundState == SoundState.CHIME)
    					PlayTitle();
    				else if(_soundState == SoundState.TITLE)
    				{
    					_soundState = SoundState.NONE;

    					if(openingDoneEvent != null)
    						openingDoneEvent();
    				} 
    				else
    				{
    					// no states and no sounds - set all audio off
    					if(_currentSound != null)
    					{
                            if(_currentSound.DelayTime > 0.0f)
    						{
                                _currentSound.DelayTime -= Time.deltaTime;
    							return;
    						}

                            _currentSound.Finish();

    						_currentSound = null;
    					}
    				}
    			}
    		} 
        }
    	#endregion

    	#region Audio Control Methods
    	/// <summary>
        /// Adds a Sound to the list of sounds.
        /// </summary>
        /// <param name="sound">Sound.</param>
    	public static void AddClip(Sound sound)
    	{
    		instance._clips.Add(sound);
    	}
    	/// <summary>
        /// Adds a Sound to the list of sounds.
        /// </summary>
        /// <param name="clip">Clip.</param>
    	public static void AddClip(AudioClip clip)
    	{
    		instance._clips.Add(new Sound(clip));
    	}

    	/// <summary>
        /// Clears the Sounds in _clips and stops any currently playing audio.
        /// </summary>
    	public static void ClearSounds()
    	{
    		Stop();

    		// unload any assets before clearing the list
            for (int i = 0; i < instance._clips.Count; i++)
                instance._clips[i].Clear();

    		instance._clips.Clear();
    		instance._currentSound = null;
    	}

    	/// <summary>
        /// Stop the current playing Sound.
        /// </summary>
    	public static void Stop()
    	{
    		instance.GetComponent<AudioSource>().Stop();
    	}

    	/// <summary>
        /// Pause the current playing Sound.
        /// </summary>
        /// <param name="pause">If set to <c>true</c> pause the Sound.</param>
    	public static void Pause(bool pause)
    	{
    		// pause
    		if(pause)
    		{
    			if(instance.GetComponent<AudioSource>().isPlaying)
    				instance._restoreAudio = true;

    			instance.GetComponent<AudioSource>().Pause();
    			instance.enabled = false;
    		}
    		// unpause
    		else
    		{
    			if(instance._restoreAudio)
    			{
    				instance.GetComponent<AudioSource>().Play();
    				instance._restoreAudio = false;
    			}

    			instance.enabled = true;
    		}
    	}

    	/// <summary>
        /// Causes the new Sound to be "immediately" played by clearing the Sound list.
        /// </summary>
        /// <param name="sound">Sound.</param>
    	public static void PlayClipImmediately(Sound sound)
    	{
    		ClearSounds ();

    		AddClip (sound);
    	}

    	/// <summary>
        /// Causes the new Sound to be "immediately" played by clearing the Sound list.
        /// </summary>
        /// <param name="clip">Clip.</param>
    	public static void PlayClipImmediately(AudioClip clip)
    	{
    		ClearSounds();

    		AddClip(clip);
    	}

    	/// <summary>
        /// Play a random Sound generated from the list of the inactive AudioClips.
        /// </summary>
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
        /// <summary>
        /// Plays the opening chime.
        /// </summary>
    	public void PlayChime()
    	{
    		PlayClipImmediately(new Sound(OpeningChime));
    		_soundState = SoundState.CHIME;
    	}
        /// <summary>
        /// Plays the title clip.
        /// </summary>
    	public void PlayTitle()
    	{
    		PlayClipImmediately(new Sound(AppTitle));
    		_soundState = SoundState.TITLE;
    	}
        /// <summary>
        /// Skips the opening audio sequence.
        /// </summary>
    	public static void SkipOpening()
    	{
    		ClearSounds();
    		instance._soundState = SoundState.NONE;
    	}
    	#endregion
    }
}