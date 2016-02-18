using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using gametheory.Utilities;

namespace gametheory.Localization
{
    public enum Language
    {
        None = -1,
    	English = 0,
    	French,
    	Spanish,
    	Italian,
    	German,
    }

    /// <summary>
    /// Manages language settings and loads new localizations when neccessary.
    /// </summary>
    public class LocalizationDictionary : MonoBehaviour 
    {
        #region Events
        /// <summary>
        /// Occurs when the language changed. Triggers LocalizationComponents to 
        /// get new text data.
        /// </summary>
        public static event System.Action<Dictionary<string,string>> languageChanged;

        /// <summary>
        /// Occurs when language fails to load.
        /// </summary>
        public static event System.Action languageFailedToLoad;
        #endregion

        #region Constants
        const string LANGUAGE_KEY = "CurrentLanguage";
        #endregion

    	#region Public Variables
    	public string BaseFileName="Localizations";
    	public Language DefaultLanguage;

        public static LocalizationDictionary Instance = null;
    	#endregion

    	#region Private Variables
        [LocalizationKey("LoadingLanguage")]
        string LOADING_LANGUAGE = "Loading Language";
        [LocalizationKey("LanguageErrorHeader")]
        string LANGUAGE_ERROR_HEADER = "Oops";
        [LocalizationKey("LanguageErrorBody")]
        string LANGUAGE_ERROR_BODY = "Unable to load the chosen language.";

        private Language _currentLanguage = Language.None;

        DateTimeFormatInfo _dateTimeFormatInfo;
        CultureInfo _cultureInfo;

    	private Dictionary<Language, Dictionary<string, string>> _localizations;
    	#endregion

    	#region Unity Methods
    	// Use this for initialization
    	void Awake () 
    	{
            if (Instance == null)
            {
                LanguageToggle.changeLanguage += SetCurrentLanguage;

                Instance = this;

                _localizations = new Dictionary<Language, Dictionary<string, string>>();
            }
            else
                Destroy(gameObject);
    	}
        void OnDestroy()
        {
            LanguageToggle.changeLanguage -= SetCurrentLanguage;
        }
    	#endregion

    	#region Methods
		public void Load()
        {
            string lang = PlayerPrefs.GetString(LANGUAGE_KEY, "");

            if (string.IsNullOrEmpty(lang))
                _currentLanguage = DefaultLanguage;
			else
				_currentLanguage = EnumUtility.ParseEnum<Language>(lang);

			StartCoroutine(LoadLocalizationCoroutine());
        }
        void Save()
        {
            PlayerPrefs.SetString(LANGUAGE_KEY, _currentLanguage.ToString());
            PlayerPrefs.Save();
        }

		public Dictionary<string,string> GetCurrentLocalization()
		{
			return _localizations[_currentLanguage];
		}

		public string GetSpecificLocalization(string key)
		{
			Dictionary<string,string> localization = _localizations[_currentLanguage];
			if(localization.ContainsKey(key))
				return localization[key];
			else
				return key;
		}

        public IEnumerator LoadLocalizationCoroutine()
        {
			_localizations = new Dictionary<Language, Dictionary<string, string>>();
			
			CSVMap map = new CSVMap(BaseFileName);
			
			int sub = 0;
			string field = "";

			for(sub = 1; sub < map.Headers.Count; sub++)
		    {
				field = map.Headers[sub];
				if(!string.IsNullOrEmpty(field))
					_localizations.Add(EnumUtility.ParseEnum<Language>(field),new Dictionary<string, string>());
			}
			yield return null;

			for(int index = 0; index < map.Contents.Count; index++)
			{
				for(sub = 1; sub < map.Headers.Count; sub++)
				{
					field = map.Headers[sub];
					if(!string.IsNullOrEmpty(field))
						_localizations[EnumUtility.ParseEnum<Language>(field)].
							Add(map.Contents[index][map.Headers[0]],map.Contents[index][map.Headers[sub]]);
				}
				yield return null;
			}

            FinishLanguageChange();

            LoadAlert.Instance.Done();
        }

        void FinishLanguageChange()
        {
            if (languageChanged != null)
                languageChanged(_localizations[_currentLanguage]);

            Save();

            _cultureInfo = CultureInfo.GetCultureInfo(CultureString);
            _dateTimeFormatInfo = DateTimeFormatInfo.GetInstance(_cultureInfo);
        }

		public void SetCurrentLanguage(Language language)
		{
			//Debug.Log(language);
			if (language != _currentLanguage)
			{
				_currentLanguage = language;

				FinishLanguageChange();
			}
		}
    	#endregion

    	#region Event Hooks
        void LoadTimeout()
        {
            DefaultAlert.Present(LANGUAGE_ERROR_HEADER, LANGUAGE_ERROR_BODY, null, null);

            if (languageFailedToLoad != null)
                languageFailedToLoad();
        }
    	#endregion

        #region Accessors
        public string CultureString
        {
            get {
                switch (_currentLanguage)
                {
                    case Language.English:
                        return "en-US";
                    case Language.French:
                        return "fr-FR";
                    case Language.German:
                        return "de-DE";
                    case Language.Italian:
                        return "it-IT";
                    case Language.Spanish:
                        return "es-ES";
                    default:
                        return "en-US";
                }
            }
        }
        public DateTimeFormatInfo DateInfo
        {
            get { return _dateTimeFormatInfo; }
        }
        public CultureInfo CulturalInfo
        {
            get { return _cultureInfo; }
        }
		public Language CurrentLanguage
		{
			get { return _currentLanguage; }
		}
        #endregion
    }
}
