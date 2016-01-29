using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace gametheory.Utilities
{
    public static class CSVImporter 
    {
		#region Public Vars
		public static bool LogInfo;
		#endregion

    	#region List Methods
        public static List<T> GenerateList<T>(string filePath)
        {
			return PopulateList<T>(new CSVMap(filePath));
        }
		public static List<T> GenerateList<T>(TextAsset asset)
		{
			return PopulateList<T>(new CSVMap(asset));
		}
		static List<T> PopulateList<T>(CSVMap map)
		{
			List<T> list = new List<T>();

			bool hasInfo = false;
			string header = "";
			int index =0,sub=0;
			System.Type type = typeof(T);
			PropertyInfo info = null;
			PropertyInfo[] properties = type.GetProperties();
			FieldInfo fInfo = null;
			FieldInfo[] fields = type.GetFields();

			for (index = 0; index < map.Contents.Count; index++)
			{
				T obj = Activator.CreateInstance<T>();

				hasInfo = false;
				for(sub = 0; sub < properties.Length; sub++)
				{
					info = properties[sub];

					header = "";

					object[] att = System.Attribute.GetCustomAttributes(info,typeof(CSVColumn),true);
					//Debug.Log(att.Length);

					if(att.Length > 0)
					{
						header = (att[0] as CSVColumn).Name;
						if(map.Headers.Contains(header))
						{
							info.SetValue(obj,Convert.ChangeType(map.Contents[index][header],info.PropertyType) ,null);
						}

						hasInfo = true;
					}
				}

				for(sub = 0; sub < fields.Length; sub++)
				{
					fInfo = fields[sub];

					header = "";

					object[] att = System.Attribute.GetCustomAttributes(fInfo,typeof(CSVColumn),true);

					if(att.Length > 0)
					{
						header = (att[0] as CSVColumn).Name;
						if(map.Headers.Contains(header))
						{
							fInfo.SetValue(obj,Convert.ChangeType(map.Contents[index][header],fInfo.FieldType));
						}
						hasInfo = true;
					}
				}

				if(LogInfo)
					Debug.Log(obj.ToString());

				list.Add(obj);
			}

			return list;
		}
		#endregion

		#region ObservableList Methods
		public static ObservableList<T> GenerateObservableList<T>(string filePath)
		{
			return PopulateObservableList<T>(new CSVMap(filePath));
		}
		public static ObservableList<T> GenerateObservableList<T>(TextAsset asset)
		{
			return PopulateObservableList<T>(new CSVMap(asset));
		}

		static ObservableList<T> PopulateObservableList<T>(CSVMap map)
		{
			ObservableList<T> list = new ObservableList<T>();

			bool hasInfo = false;
			string header = "";
			int index =0, sub = 0;

			System.Type type = typeof(T);
			PropertyInfo info = null;
			PropertyInfo[] properties = type.GetProperties();

			FieldInfo fInfo = null;
			FieldInfo[] fields = type.GetFields();

			for (index = 0; index < map.Contents.Count; index++)
			{
				T obj = Activator.CreateInstance<T>();

				hasInfo = false;
				for(sub = 0; sub < properties.Length; sub++)
				{
					info = properties[sub];

					header = "";

					object[] att = System.Attribute.GetCustomAttributes(info,typeof(CSVColumn),true);
					//Debug.Log(att.Length);

					if(att.Length > 0)
					{
						header = (att[0] as CSVColumn).Name;
						if(map.Headers.Contains(header))
						{
							info.SetValue(obj,Convert.ChangeType(map.Contents[index][header],info.PropertyType) ,null);
						}

						hasInfo = true;
					}
				}

				for(sub = 0; sub < fields.Length; sub++)
				{
					fInfo = fields[sub];

					header = "";

					object[] att = System.Attribute.GetCustomAttributes(fInfo,typeof(CSVColumn),true);

					if(att.Length > 0)
					{
						header = (att[0] as CSVColumn).Name;
						if(map.Headers.Contains(header))
						{
							fInfo.SetValue(obj,Convert.ChangeType(map.Contents[index][header],fInfo.FieldType));
						}
						hasInfo = true;
					}
				}

				if(hasInfo)
				{
					if(LogInfo)
						Debug.Log(obj.ToString());

					list.Add(obj);
				}
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
        public List<string> Headers;
        public List<Dictionary<string,string>> Contents;
    	#endregion

    	#region Constructors
    	public CSVMap()
    	{
			Headers = new List<string>();
			Contents = new List<Dictionary<string,string>>();
    	}
		public CSVMap(TextAsset asset)
		{
			Contents = new List<Dictionary<string, string>>();
			SetupContents(asset.text);
		}
        public CSVMap(string filePath)
    	{
			Contents = new List<Dictionary<string,string>>();

    		string fileContent = "";

			try
			{
	            TextAsset asset = (TextAsset)Resources.Load(filePath, typeof(TextAsset));

				fileContent = asset.text;
			}
			catch(Exception exp)
			{
				StreamReader streamReader = new StreamReader(filePath);
				
				fileContent = streamReader.ReadToEnd();
			}

			SetupContents(fileContent);
    	}
    	#endregion

		#region Methods
		void SetupContents(string content)
		{
			string[] rows = content.Split(ROW);
			int index =0;

			if (rows.Length > 0)
			{
				Headers = new List<string>(rows[0].Split(COLUMN));
				for(index = 0; index < Headers.Count; index++)
					Headers[index] = Headers[index].Trim();
			}

			if (rows.Length > 1)
			{
				for (index = 1; index < rows.Length; index++)
				{
					if(!string.IsNullOrEmpty(rows[index]))
					{
						string[] row = rows[index].Split(COLUMN);

						Dictionary<string,string> dict =new Dictionary<string, string>();
						for(int sub = 0; sub < Headers.Count; sub++)
						{
							dict.Add(Headers[sub],row[sub]);
						}

						Contents.Add(dict);
					}
				}
			}
		}
		#endregion
    }

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,Inherited = true)]
	public class CSVColumn : Attribute
	{
		#region Public Vars
		public string Name;
		#endregion
		
		#region Constructors
		public CSVColumn(string name)
		{
			Name = name;
		}
		#endregion
	}
}