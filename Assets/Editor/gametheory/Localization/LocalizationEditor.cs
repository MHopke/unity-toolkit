using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace gametheory.Localization
{
    public class LocalizationEditor : MonoBehaviour 
    {
        #region Constants
        const string COMMA = ",";
        const string IDENTIFIER = "Identifier";
        #endregion

        [MenuItem("gametheory/Localization/Generate CSV")]
        static void GenerateCSV()
        {
            //Debug.Log("temp!");

            using(StreamWriter writer = new StreamWriter(
                Path.Combine(Application.dataPath,Path.Combine("Resources","Localizations.csv"))))
            {
                writer.WriteLine(IDENTIFIER + COMMA + "English" + COMMA + 
                                 "French" + COMMA + "German" + COMMA + "Italian" + COMMA + "Spanish");

                GameObject[] objs = Selection.gameObjects;
                Type type = null;
                GameObject obj = null;
                FieldInfo field = null;
                FieldInfo[] fields = null;
                MonoBehaviour[] components = null;
                MonoBehaviour component = null;
                string key = "";

                List<string> values = new List<string>();

                int componentIndex =0, fieldIndex = 0;
                for(int index =0; index < objs.Length; index++)
                {
                    obj = objs[index];
                    components = obj.GetComponents<MonoBehaviour>();
                    for(componentIndex = 0; componentIndex  < components.Length; componentIndex++)
                    {
                        component = components[componentIndex];
                        type = component.GetType();

                        fields = type.GetFields(BindingFlags.Instance|BindingFlags.NonPublic
                                                |BindingFlags.Public|BindingFlags.Static);

                        for (fieldIndex =0; fieldIndex  < fields.Length; fieldIndex++)
                        {
                            field = fields[fieldIndex];
                            if (field.FieldType == typeof(string))
                            {
                                var attrs = (LocalizationKey[])field.GetCustomAttributes
                                    (typeof(LocalizationKey),false);
                                
                                if (attrs.Length > 0)
                                {
                                    //Debug.Log(_text[attrs[0].Key]);
                                    key = attrs[0].Key;
                                    
                                    if(!values.Contains(key))
                                    {
                                        values.Add(key);
                                        writer.WriteLine(key + COMMA + field.GetValue(component) 
                                                         + COMMA + COMMA + COMMA + COMMA);
                                    }
                                }
                            }
                        }
                    }//end component loop
                }//end object loop
            }//end using
        }
    }
}
