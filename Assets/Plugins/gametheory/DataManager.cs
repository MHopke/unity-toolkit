using UnityEngine;
#if UNITY_WEBPLAYER
using MiniJSON;
using System.Collections.Generic;
#else
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace gametheory
{
    /// <summary>
    /// A singleton designed to provide a centralized access point to data that needs to be saved/loaded
    /// and accessed across from multiple Monobehaviours.
    /// </summary>
    public class DataManager : MonoBehaviour 
    {
        #if UNITY_WEBPLAYER
        const string DATA_KEY = "DATA";

        const string AUDIO_KEY = "Audio";
        #endif

    	#region Private Variables
    	string _fileName = "settings.dat";
    	string _filePath;

    	GameData _gameData;

    	public static DataManager instance = null;
    	#endregion

    	#region Unity Methods
    	void Awake()
    	{
    		if(instance == null)
    		{
    			instance = this;
                DontDestroyOnLoad(gameObject);
    		}
    		else
    			Destroy(gameObject);
    	}

    	// Use this for initialization
    	void Start () 
    	{
            #if UNITY_WEBPLAYER
            #else
    		_filePath = Application.persistentDataPath + "/" + _fileName;

    		#if CLEAR_DATA
    		File.Delete(_filePath);
    		#endif
            #endif

    		LoadSettings();
    	}
    	#endregion

    	#region Methods
    	void SaveSettings()
        {
            #if UNITY_WEBPLAYER
            Dictionary<string,object> items = new Dictionary<string,object>();

            //for each item add an element
            items.Add(AUDIO_KEY, _gameData.AudioOn);

            PlayerPrefs.SetString(DATA_KEY, Json.Serialize(items));
            PlayerPrefs.Save();
            #else
    		BinaryFormatter formatter = new BinaryFormatter ();
    		FileStream file = File.Create(_filePath);

    		formatter.Serialize (file, _gameData);

    		file.Close ();
            #endif
    	}

    	void LoadSettings()
    	{
    		_gameData = new GameData();

            #if UNITY_WEBPLAYER
            string json = PlayerPrefs.GetString(DATA_KEY,"");

            if(json != "")
            {
                Dictionary<string,object> items = Json.Deserialize(json) as Dictionary<string,object>;

                //unload each item
                if(items.ContainsKey(AUDIO_KEY))
                    _gameData.AudioOn = (bool)items[AUDIO_KEY];
            }
            #else
    		if(File.Exists(_filePath))
    		{
    			BinaryFormatter formatter = new BinaryFormatter();
    			FileStream file = File.Open(_filePath, FileMode.Open);

    			_gameData = (GameData)formatter.Deserialize(file);

    			file.Close();
    		}
    		else
    		{
    			//do some initial data stuff here
    		}
            #endif
    	}
    	#endregion

    	#region Accessors
        public static GameData Data
    	{
    		get { return instance._gameData; }
    		set { instance._gameData = value; }
    	}
    	#endregion
    }
}