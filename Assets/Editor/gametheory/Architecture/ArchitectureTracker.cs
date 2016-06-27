using UnityEngine;
using UnityEditor;

using System;
using System.Reflection;
using System.Collections.Generic;

using MiniJSON;

namespace gametheory.Architecture
{
	public class ArchitectureTracker : EditorWindow 
	{
		#region Constants
		const string DESC_KEY = "architectureDescriptions";
		#endregion

		#region Private Vars
		int _index;
		Architecture _arch;
		public List<Architecture> Architectures;
		#endregion

		#region Unity Methods
		// Add menu named "My Window" to the Window menu
		[MenuItem ("gametheory/Architecture Tracker")]
		static void Init () 
		{
			// Get existing open window or if none, make a new one:
			ArchitectureTracker window = (ArchitectureTracker)EditorWindow.GetWindow (typeof (ArchitectureTracker));
			window.Show();
		}

		void OnGUI () 
		{
			if(GUILayout.Button("Setup List"))
				SetupList();

			if(GUILayout.Button("Save Descriptions"))
				SaveDescriptions();

			// "target" can be any class derrived from ScriptableObject 
			// (could be EditorWindow, MonoBehaviour, etc)
			ScriptableObject target = this;
			SerializedObject so = new SerializedObject(target);
			SerializedProperty stringsProperty = so.FindProperty("Architectures");

			if(stringsProperty != null)
			{
				EditorGUILayout.PropertyField(stringsProperty, true);

				so.ApplyModifiedProperties(); // Remember to apply modified properties
			}
		}
		#endregion

		#region Methods
		void SaveDescriptions()
		{
			Dictionary<string,string> dict = new Dictionary<string, string>();
			for(_index = 0; _index <Architectures.Count; _index++)
			{
				_arch = Architectures[_index];

				if(!dict.ContainsKey(_arch.Name))
					dict.Add(_arch.Name,_arch.Description);
			}

			PlayerPrefs.SetString(DESC_KEY,Json.Serialize(dict));
			PlayerPrefs.Save();
		}

		void SetupList()
		{
			Architectures = new List<Architecture>();

			Dictionary<string,List<MonoBehaviour>> behaviors = new Dictionary
				<string, List<MonoBehaviour>>();

			GameObject[] objs = Selection.gameObjects;
			Type type = null;
			GameObject obj = null;
			object[] infoList;
			ArchitectureTag info;
			MonoBehaviour[] components = null;
			MonoBehaviour component = null;
			string key = "";

			List<string> values = new List<string>();

			int index = 0, componentIndex =0, fieldIndex = 0;
			for(index =0; index < objs.Length; index++)
			{
				obj = objs[index];
				components = obj.GetComponents<MonoBehaviour>();
				for(componentIndex = 0; componentIndex  < components.Length; componentIndex++)
				{
					component = components[componentIndex];
					type = component.GetType();

					infoList = type.GetCustomAttributes(typeof(ArchitectureTag),true);

					//Debug.Log(obj.name + " " + infoList.Length);

					//monobehavior has a tag
					if(infoList.Length > 0)
					{
						info = infoList[0] as ArchitectureTag;
						if(behaviors.ContainsKey(info.Tag))
							behaviors[info.Tag].Add(component);
						else
							behaviors.Add(info.Tag,new List<MonoBehaviour>() { component });
					}
				}//end component loop
			}//end object loop

			//load pre-existing descriptions
			Dictionary<string,object> dict = null;
			string json = PlayerPrefs.GetString(DESC_KEY,"");
			if(string.IsNullOrEmpty(json))
				dict = new Dictionary<string, object>();
			else
				dict = Json.Deserialize(json) as Dictionary<string,object>;

			//add classes to each 
			foreach(KeyValuePair<string,List<MonoBehaviour>> pair in behaviors)
			{
				Architecture arch = new Architecture() { Name = pair.Key };
				arch.Behaviors = new List<MonoBehaviour>(pair.Value);

				if(dict.ContainsKey(arch.Name))
					arch.Description = (string)dict[arch.Name];

				Architectures.Add(arch);
			}
		}
		#endregion
	}
}