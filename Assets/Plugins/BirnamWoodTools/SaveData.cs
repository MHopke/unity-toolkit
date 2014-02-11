using UnityEngine;
using System;

/// <summary>
/// Manages Saving and Loading Data. Designed to be de-centralized and sends
/// loadDataEvent and saveDataEvent to listeners when triggered. Should be 
/// placed after all other scripts in Unity Script Execution Order.
/// </summary>
public class SaveData : MonoBehaviour {

	#region Events
	public static event Action _loadDataEvent;
	public static event Action _saveDataEvent;
	#endregion

	#region Private Variables
	bool _skipSaving;

	static SaveData _instance = null;
	#endregion

	#region Unity Methods
	// Use this for initialization
	void Start () 
	{
		Load();

		_instance = this;
	}

	void OnApplicationPause(bool pause)
	{
		if(pause)
		{
			Save();
		} else if(_skipSaving)
			_skipSaving = false;
	}
	#endregion

	#region Methods
	public static void SkipSaving()
	{
		_instance._skipSaving = true;
	}
	public static void Save()
	{
		if(!_instance._skipSaving && _saveDataEvent != null)
			_saveDataEvent();
	}
	public static void Load()
	{
		if(_loadDataEvent != null)
			_loadDataEvent();
	}
	#endregion
}
