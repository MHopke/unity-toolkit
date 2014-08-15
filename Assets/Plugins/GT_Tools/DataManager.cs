using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace gametheory
{
    public class DataManager : MonoBehaviour 
    {
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
    		}
    		else
    			Destroy(gameObject);
    	}

    	// Use this for initialization
    	void Start () 
    	{
    		_filePath = Application.persistentDataPath + "/" + _fileName;

    		#if CLEAR_DATA
    		File.Delete(_filePath);
    		#endif

    		LoadSettings();
    	}
    	#endregion

    	#region Methods
    	void SaveSettings()
    	{
    		BinaryFormatter formatter = new BinaryFormatter ();
    		FileStream file = File.Create(_filePath);

    		formatter.Serialize (file, _gameData);

    		file.Close ();
    	}

    	void LoadSettings()
    	{
    		_gameData = new GameData();

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