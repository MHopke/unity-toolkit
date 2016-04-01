using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

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

			for (int index = 0; index < map.Contents.Count; index++)
			{
				T obj = Activator.CreateInstance<T>();

				hasInfo = PopulateObject<T>(map.Contents[index], ref obj);

				if(LogInfo)
					Debug.Log(obj.ToString());

				if(hasInfo)
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
			int index =0;

			for (index = 0; index < map.Contents.Count; index++)
			{
				T obj = Activator.CreateInstance<T>();

				hasInfo = PopulateObject<T>(map.Contents[index], ref obj);

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

		#region Methods
		static bool PopulateObject<T>(Dictionary<string,string> entry, ref T obj)
		{
			bool hasInfo = false;
			string header = "";
			int sub=0;
			System.Type type = typeof(T);
			PropertyInfo info = null;
			PropertyInfo[] properties = type.GetProperties();
			FieldInfo fInfo = null;
			FieldInfo[] fields = type.GetFields();
			for(sub = 0; sub < properties.Length; sub++)
			{
				info = properties[sub];

				header = info.Name;

				object att = System.Attribute.GetCustomAttribute(info,typeof(CSVIgnore),true);
				//Debug.Log(att.Length);

				//Debug.Log(header + " " + att);

				if(att == null)
				{
					att = System.Attribute.GetCustomAttribute(info,typeof(CSVColumn),true);
					object converter = System.Attribute.GetCustomAttribute(info,
						typeof(IColumnConverter),true);

					if(att != null)
						header = (att as CSVColumn).Name;

					if(entry.ContainsKey(header))
					{
						if(converter != null)
							info.SetValue(obj,(converter as IColumnConverter).
								Convert(entry[header]) ,null);
						else
							info.SetValue(obj,Convert.ChangeType(entry[header]
								,info.PropertyType) ,null);
					}

					hasInfo = true;
				}
			}

			for(sub = 0; sub < fields.Length; sub++)
			{
				fInfo = fields[sub];

				header = fInfo.Name;//"";

				object att = System.Attribute.GetCustomAttribute(fInfo,typeof(CSVIgnore),true);
				//Debug.Log(att.Length);

				//Debug.Log(header + " " + att);

				if(att == null)
				{
					att = System.Attribute.GetCustomAttribute(fInfo,typeof(CSVColumn),true);
					object converter = System.Attribute.GetCustomAttribute(fInfo,
						typeof(IColumnConverter),true);

					if(att != null)
						header = (att as CSVColumn).Name;

					if(entry.ContainsKey(header))
					{
						if(converter != null)
							fInfo.SetValue(obj,(converter as IColumnConverter)
								.Convert(entry[header]));
						else
							fInfo.SetValue(obj,Convert.ChangeType(entry[header],
								fInfo.FieldType));
					}
					hasInfo = true;
				}
			}

			return hasInfo;
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
							dict.Add(Headers[sub],row[sub].Trim());
						}

						Contents.Add(dict);
					}
				}
			}
		}
		#endregion
    }

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,Inherited = true)]
	public class CSVIgnore : Attribute
	{
		#region Constructors
		public CSVIgnore(){}
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

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,Inherited = true)]
	public abstract class IColumnConverter : Attribute
	{
		#region Constructors
		public IColumnConverter()
		{
		}
		#endregion

		#region Methods
		public virtual object Convert(object obj)
		{
			return obj;
		}
		#endregion
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,Inherited = true)]
	public class EnumConverter : IColumnConverter
	{
		#region P Vars
		protected Type _type;
		#endregion

		#region Constructors
		public EnumConverter(Type type)
		{
			_type = type;
		}
		#endregion

		#region Methods
		public override object Convert(object obj)
		{
			string str = (string)obj;
			if(string.IsNullOrEmpty(str))
				return null;
			else
				return Enum.Parse(_type,str);
		}
		#endregion
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,Inherited = true)]
	public class EnumArrayConverter : IColumnConverter
	{
		#region P Vars
		protected char _delimeter;
		protected Type _type;
		#endregion

		#region Constructors
		public EnumArrayConverter(Type type,char delim='|')
		{
			_type = type;
			_delimeter = delim;
		}
		#endregion

		#region Methods
		public override object Convert(object obj)
		{
			string str = (string)obj;
			if(string.IsNullOrEmpty(str))
				return null;
			else
			{
				var listType = typeof(List<>);
				var constructedListType = listType.MakeGenericType(_type);
				IList instance = (IList)Activator.CreateInstance(constructedListType);

				string[] arr = str.Split(_delimeter);

				for(int index =0; index < arr.Length; index++)
				{
					instance.Add(Enum.Parse(_type,arr[index]));
				}

				return instance;
			}
		}
		#endregion
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,Inherited = true)]
	public class ArrayConverter : IColumnConverter
	{
		#region Public Vars
		public char Delimeter;
		#endregion

		#region Constructors
		public ArrayConverter(char delim)
		{
			Delimeter = delim;
		}
		#endregion

		#region Overriden Methods
		public override object Convert (object obj)
		{
			string str = (string)obj;

			if(string.IsNullOrEmpty(str))
				return null;
			else
				return (obj as string).Split(Delimeter);
		}
		#endregion
	}
}