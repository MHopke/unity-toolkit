using UnityEngine;
using gametheory.UI;
using gametheory.Localization;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class VariableInfo
{
	#region Public Variables
	public List<string> ClassNames,
					    FilePaths,
						DirectoryPaths;
	
	public Dictionary<string, Dictionary<string, string>> VariableMaps;
	#endregion
}

/// <summary>
/// A premade component that will generate the Key/Value pairs that will
/// be written into the xml file.
/// </summary>
public class GenerateKeys : UIList 
{
	#region Public Variables
    [Tooltip("Folder where localized scripts are located")]
    public string ScriptsDirectory;

    public KeyValueUI KeyValuePrefab;

    public ExtendedText ClassLabelPrefab;

    [Tooltip("Folders that should be ignored in the scripts directory")]
	public List<string> IgnoreDirectories;
    [Tooltip("Classes that should be ignored in the scripts directory")]
    public List<string> IgnoreClasses;
	#endregion

	#region Private Variables
	private int _currentTotalCount;
	private string _assemblyName,
				   _directoryPath;

	private VariableInfo _info;
	#endregion

	#region Unity Methods
	void Awake()
	{
		_currentTotalCount = 0;
		_assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
		_info = new VariableInfo();

		GetAllFileNames();
		GetVariableNames();

		foreach(var kvp in _info.VariableMaps)
		{
			var label = (ExtendedText)Instantiate(ClassLabelPrefab, Vector3.zero, Quaternion.identity);
			label.Text = kvp.Key;
			AddListElement(label);

			_currentTotalCount++;

			InstantiateListObject(kvp.Value, kvp.Key);
		}

		GenerateXML.GetVariableInfo += GetVariableInfo;
	}
	#endregion

	#region Methods
	private void GetAllFileNames()
	{
		_directoryPath = Application.dataPath + "/" + ScriptsDirectory;

		_info.FilePaths = new List<string>(Directory.GetFiles(_directoryPath).Where(x => !x.Split('.').Contains("meta") && 
		                                                                       !x.Split('.').Contains("DS_Store"))); 

		_info.DirectoryPaths = new List<string>(Directory.GetDirectories(_directoryPath));

		_info.ClassNames = new List<string>(_info.FilePaths.Select(x => Path.GetFileName(x).Split('.')[0]));
		
		if(IgnoreDirectories.Count > 0)
			IgnoreDirectories.ForEach(x => _info.DirectoryPaths.Remove(_directoryPath + "/" + x));

		foreach(var directory in _info.DirectoryPaths)
			GetFileNames(directory);
	}

	private void GetFileNames(string directory)
	{
		var paths = new List<string>(Directory.GetFiles(directory).Where(x => !x.Split('.').Contains("meta")));
		var names = new List<string>(paths.Select(x => Path.GetFileName(x).Split('.')[0]));

		_info.FilePaths.AddRange(paths);
		_info.ClassNames.AddRange(names);
	}

	private void GetVariableNames()
	{
		_info.VariableMaps = new Dictionary<string, Dictionary<string, string>>();

		foreach(var className in _info.ClassNames)
		{
            if(!IgnoreClasses.Contains(className) && className != "")
            {
                //Debug.Log(className);
                var variableList = new Dictionary<string, string>();
                var handle = Activator.CreateInstance(_assemblyName, className);
                var obj = handle.Unwrap();

                var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                var stringFields = fields.Where(x => ((FieldInfo)x).FieldType == typeof(string));

                foreach (FieldInfo field in stringFields)
                {
                    var attrs = (LocalizationKey[])field.GetCustomAttributes
                        (typeof(LocalizationKey),false);

                    if (attrs.Length > 0)
                    {
                        variableList.Add(attrs[0].Key, (string)field.GetValue(obj));
                    }
                }

                _info.VariableMaps.Add(className, variableList);
            }
		}
	}

	private void InstantiateListObject(Dictionary<string, string> data, string classKey)
	{		
		//set up list
		_currentTotalCount += data.Count;
		var startIndex = ListItems.Count;

		if(ListItems.Count != _currentTotalCount)
		{
			if(ListItems.Count < _currentTotalCount)
			{
				for(int i = startIndex; i < _currentTotalCount; i++)
				{
					KeyValueUI ui = (KeyValueUI)GameObject.Instantiate(KeyValuePrefab,Vector3.zero,Quaternion.identity);
					AddListElement(ui);
				}
			}
			else
			{
				RemoveListElements(ListItems.Count - _currentTotalCount);
			}
		}
		
		if(ListItems.Count == 0)
		{
			if(EmptyListItem)
				EmptyListItem.Present();
		}
		else
		{
			string key = "";
			var keys = data.Keys.ToList();

			for(int i = startIndex; i < ListItems.Count; i++)
			{
				key = keys[i - startIndex];
                if (data[key] != null)
					(ListItems[i] as KeyValueUI).Initialize(classKey, key, data[key], OnSubmit);
                else
                    Debug.Log(classKey + " " + key);
			}
		}
	}
	#endregion

	#region Event Hooks
	private void OnSubmit(string classKey, string key, string value)
	{
		_info.VariableMaps[classKey][key] = value;
	}

	private void GetVariableInfo(ref Dictionary<string, Dictionary<string, string>> variableMap)
	{
		variableMap = _info.VariableMaps;
	}
	#endregion
}
