using UnityEngine;

using System;
using System.IO;
using System.Collections;

public class JSONDatabase : MonoBehaviour 
{
    #region Constants
    const string FILE_ENDING = ".bytes";
    #endregion

    #region Public Vars
    public TextAsset DatabaseFile;
    #endregion

    #region Methods
    public void Setup()
    {
        if(!File.Exists(Path.Combine(Application.persistentDataPath,DatabaseFile.name + FILE_ENDING)))
        {
            //Debug.Log("file does not exist");

            Save(DatabaseFile.name,DatabaseFile.text);
        }
        //else
        //    Debug.Log("file exists!");
    }
    public string GetData()
    {
        return Load(DatabaseFile.name);
    }

    public static void Save(string path, string json)
    {
        SaveExactPath(Path.Combine(Application.persistentDataPath, path),json);
        //Debug.Log(path);
    }
    public static void SaveExactPath(string path, string json)
    {
        StreamWriter writer = new StreamWriter(path + FILE_ENDING);
        
        writer.Write(json);
        
        writer.Close();
    }
    public static string Load(string path)
    {
        StreamReader reader = new StreamReader(Path.Combine(Application.persistentDataPath,
                                                            path + FILE_ENDING));
        
        string contents = reader.ReadToEnd();

        //Debug.Log(contents);

        reader.Close();
        
        return contents;
    }
    #endregion
}
