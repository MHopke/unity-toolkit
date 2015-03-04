using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.UI;

public class GenerateXML : MonoBehaviour 
{
	#region Events
	public delegate void GetInfoEvent(ref Dictionary<string, Dictionary<string, string>> variableMap);
	public static event GetInfoEvent GetVariableInfo;
	#endregion

	#region Public Variables
	public InputField FileNameField;
	#endregion

    #region Private Variables
    private string _fileName;
    private bool _generateXML;
    private XmlSerializer _serializer;
    private TextWriter _writer;
	private Dictionary<string, Dictionary<string, string>> _variableMap;
	#endregion

	#region Unity Methods
	void Awake()
	{
		_fileName = "";
		_generateXML = false;

		FileNameField.onEndEdit.AddListener(GetFileName);
	}

	void OnDestroy()
	{
		GetVariableInfo = null;
		FileNameField.onEndEdit.RemoveListener(GetFileName);
	}

	void Update()
	{
		if(_generateXML)
		{
			_generateXML = false;
			Generate();
		}
	}
	#endregion

	#region Methods
	void Generate()
	{
		_serializer = new XmlSerializer(typeof(XmlDocument));

		var Document = new XmlDocument();
        XmlElement root = Document.CreateElement("root");

        var xmlElement = Document.CreateElement("Classes");

		foreach(var kvp in _variableMap)
		{
			var classElement = Document.CreateElement("Class");
			classElement.SetAttribute("name", kvp.Key);

			foreach(var childKVP in kvp.Value)
			{
				var childElement = Document.CreateElement("Variable");
				childElement.SetAttribute("name", childKVP.Key);
				childElement.InnerText = childKVP.Value;
				classElement.AppendChild(childElement);
			}

			xmlElement.AppendChild(classElement);
		}

        root.AppendChild(xmlElement);

        //add other localized app data here

        Document.AppendChild(root);

		_writer = new StreamWriter(_fileName);
		_serializer.Serialize(_writer, Document);
		_writer.Close();
	}
	#endregion

	#region UI Events
	void GetFileName(string value)
	{
		_fileName = Application.dataPath + "/" + value + ".xml";
	}

	public void GetInfo()
	{
		if(GetVariableInfo != null && _fileName != "")
		{
			GetVariableInfo(ref _variableMap);
			_generateXML = true;
		}
		else if(_fileName == "")
			Debug.Log("Please enter a filename.");
	}
	#endregion
}
