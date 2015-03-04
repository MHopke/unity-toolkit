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
        public MonoBehaviour _targetObject;
    	#endregion 

    	#region Private Variables
        System.Type _targetType;

    	Dictionary<string, string> _text;
    	#endregion

    	#region Unity Methods
        void Awake()
        {
            LocalizationDictionary.languageChanged += AssignText;

            _targetType = _targetObject.GetType();
        }
        void OnDestroy()
        {
            LocalizationDictionary.languageChanged -= AssignText;
        }
    	#endregion

    	#region Methods
        void AssignText()
        {
            _text = LocalizationDictionary.Instance.GetVariableText(_targetType.Name);

            foreach (var prop in _targetType.GetFields())
            {
                if (prop.FieldType == typeof(string))
                {
                    var attrs = (LocalizationKey[])prop.GetCustomAttributes
                        (typeof(LocalizationKey),false);

                    if (attrs.Length > 0 && _text.ContainsKey(attrs[0].Key))
                    {
                        //Debug.Log(_text[attrs[0].Key]);
                        prop.SetValue(_targetObject, _text[attrs[0].Key]);
                    }
                }
            }

            //Tell the target object that it's localized data has changed
            _targetObject.SendMessage("LanguageChanged", SendMessageOptions.DontRequireReceiver);
        }
    	#endregion
    }
}
