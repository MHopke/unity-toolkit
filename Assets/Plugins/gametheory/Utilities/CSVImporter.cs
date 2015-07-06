using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace gametheory.Utilities
{
    public static class CSVImporter 
    {
    	#region Methods
        public static List<T> GenerateList<T>(string filePath)
        {
            CSVMap map = new CSVMap(filePath);
            List<T> list = new List<T>();
            for (int index = 0; index < map.Contents.Count; index++)
            {
                T obj = Activator.CreateInstance<T>();
                System.Type type = obj.GetType();
                PropertyInfo info = null;

                for (int sub = 0; sub < map.Headers.Length; sub++)
                {
					if(!string.IsNullOrEmpty(map.Headers[sub]))
					{
	                    info = type.GetProperty(map.Headers[sub]);
						if(info != null)
	                    	info.SetValue(obj,Convert.ChangeType(map.Contents[index][sub],info.PropertyType) ,null);
					}
                }

                list.Add(obj);
            }

            return list;
        }
    	#endregion
    }
    public class CSVMap
    {
    	#region Constants
    	const char ROW = '\n';
    	const char COLUMN = ',';
    	#endregion

    	#region Public Vars
        public string[] Headers;
        public List<string[]> Contents;
    	#endregion

    	#region Constructors
    	public CSVMap()
    	{
            Contents = new List<string[]>();
    	}
        public CSVMap(string filePath)
    	{
            Contents = new List<string[]>();

    		TextAsset text = null;
    		string fileContent = "";
    		string[] rows = null;

            text = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
            fileContent = text.text;

            rows = fileContent.Split(ROW);

            if (rows.Length > 0)
                Headers = rows[0].Split(COLUMN);

            if (rows.Length > 1)
            {
                for (int index = 1; index < rows.Length; index++)
                {
                    Contents.Add(rows[index].Split(COLUMN));
                }
            }
    	}
    	#endregion
    }
}