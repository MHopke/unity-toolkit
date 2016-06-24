using UnityEngine;
using UnityEditor;

using System;
using System.Reflection;
using System.Collections.Generic;

namespace gametheory.Architecture
{
	public class ArchitectureTracker : EditorWindow 
	{
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

			// "target" can be any class derrived from ScriptableObject 
			// (could be EditorWindow, MonoBehaviour, etc)
			ScriptableObject target = this;
			SerializedObject so = new SerializedObject(target);
			SerializedProperty stringsProperty = so.FindProperty("Architectures");

			if(stringsProperty != null)
			{
				EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
				so.ApplyModifiedProperties(); // Remember to apply modified properties
			}
		}
		#endregion

		#region Methods
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

					Debug.Log(obj.name + " " + infoList.Length);

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

			//add classes to each 
			foreach(KeyValuePair<string,List<MonoBehaviour>> pair in behaviors)
			{
				Architecture arch = new Architecture() { Name = pair.Key };
				arch.Behaviors = new List<MonoBehaviour>(pair.Value);
				Architectures.Add(arch);
			}
		}
		#endregion
	}
}