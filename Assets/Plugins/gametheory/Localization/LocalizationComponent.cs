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
		#region Events
		public event System.Action componentLocalized;
		#endregion

		#region Public Variables
		public bool SetOnAwake;
        public MonoBehaviour TargetObject;
    	#endregion 

    	#region Private Variables
        System.Type _targetType;
    	#endregion

    	#region Unity Methods
        void Awake()
        {
            LocalizationManager.languageChanged += AssignText;

            _targetType = TargetObject.GetType();

			if(SetOnAwake)
				PullData();
        }
        void OnDestroy()
        {
            LocalizationManager.languageChanged -= AssignText;
        }
    	#endregion

		#region Methods
		public void PullData()
		{
			AssignText(LocalizationManager.Instance.GetCurrentLocalization());
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

			//if(componentLocalized != null)
			//	componentLocalized();
			//Tell the target object that it's localized data has changed
			bool wasInactive = false;
			if(!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
				wasInactive = true;
			}

			TargetObject.SendMessage("LanguageChanged", SendMessageOptions.DontRequireReceiver);

			if(wasInactive)
				gameObject.SetActive(false);
        }
    	#endregion
    }
}
