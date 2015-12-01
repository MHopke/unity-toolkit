using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;

namespace gametheory.Localization
{
    /// <summary>
    /// Loads the localization data for the target component.
    /// </summary>
    public class LocalizationComponent : MonoBehaviour 
    {
    	#region Public Variables
        public MonoBehaviour TargetObject;
    	#endregion 

    	#region Private Variables
        System.Type _targetType;
    	#endregion

    	#region Unity Methods
        void Awake()
        {
            LocalizationDictionary.languageChanged += AssignText;

            _targetType = TargetObject.GetType();
        }
        void OnDestroy()
        {
            LocalizationDictionary.languageChanged -= AssignText;
        }
    	#endregion

    	#region Event Listeners
        void AssignText(Dictionary<string,string> content)
        {
			foreach (var prop in _targetType.GetFields(BindingFlags.Instance|BindingFlags.NonPublic
				|BindingFlags.Public|BindingFlags.Static))
            {
                if (prop.FieldType == typeof(string))
                {
                    var attrs = (LocalizationKey[])prop.GetCustomAttributes
                        (typeof(LocalizationKey),false);

                    if (attrs.Length > 0)
                    {
                        //Debug.Log(_text[attrs[0].Key]);
						if(content.ContainsKey(attrs[0].Key))
                        	prop.SetValue(TargetObject, content[attrs[0].Key]);
                    }
                }
            }

            //Tell the target object that it's localized data has changed
            TargetObject.SendMessage("LanguageChanged", SendMessageOptions.DontRequireReceiver);
        }
    	#endregion
    }
}
