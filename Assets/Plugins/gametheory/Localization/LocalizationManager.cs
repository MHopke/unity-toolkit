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
    	English = 0,
		Spanish,
		Portugese,
    }

    /// <summary>
    /// Manages language settings and loads new localizations when neccessary.
    /// </summary>
    public class LocalizationManager : MonoBehaviour 
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
		const string FILE_PATH = "Localizations - Localizations";
        #endregion

    	#region Public Variables
    	public Language DefaultLanguage;

        public static LocalizationManager Instance = null;
    	#endregion

    	#region Protected Variables
		protected Language _currentLanguage = Language.English;

        protected DateTimeFormatInfo _dateTimeFormatInfo;
        protected CultureInfo _cultureInfo;

		protected LocalizationDictionary _localizations;
    	#endregion

		#region Localization Strings
		[LocalizationKey("LoadingLanguage")]
		string LOADING_LANGUAGE = "Loading Language";
		[LocalizationKey("LanguageErrorHeader")]
		string LANGUAGE_ERROR_HEADER = "Oops";
		[LocalizationKey("LanguageErrorBody")]
		string LANGUAGE_ERROR_BODY = "Unable to load the chosen language.";
		#endregion

    	#region Unity Methods
    	// Use this for initialization
    	void Awake () 
    	{
            if (Instance == null)
            {
                LanguageToggle.changeLanguage += SetCurrentLanguage;

                Instance = this;

				ConvertSystemLanguage(Application.systemLanguage);

				_localizations = new LocalizationDictionary();
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
		void ConvertSystemLanguage(SystemLanguage lang)
		{
			switch(lang)
			{
			case SystemLanguage.English:
				DefaultLanguage = Language.English;
				break;
			case SystemLanguage.Spanish:
				DefaultLanguage = Language.Spanish;
				break;
			case SystemLanguage.Portuguese:
				DefaultLanguage = Language.Portugese;
				break;
			default:
				DefaultLanguage = Language.English;
				break;
			}
		}

		public void Load()
        {
            string lang = PlayerPrefs.GetString(LANGUAGE_KEY, "");

            if (string.IsNullOrEmpty(lang))
                _currentLanguage = DefaultLanguage;
			else
				_currentLanguage = EnumUtility.ParseEnum<Language>(lang);

			LoadLocalizations();
        }
        void Save()
        {
            PlayerPrefs.SetString(LANGUAGE_KEY, _currentLanguage.ToString());
            PlayerPrefs.Save();
        }

		public Dictionary<string,string> GetCurrentLocalization()
		{
			return _localizations.GetLocalization(_currentLanguage);
		}

		public string GetSpecificLocalization(string key)
		{
			Dictionary<string,string> localization = _localizations.GetLocalization(_currentLanguage);
			if(localization.ContainsKey(key))
				return localization[key];
			else
				return key;
		}

        public void LoadLocalizations()
        {
			_localizations = new LocalizationDictionary();
			
			CSVMap map = new CSVMap(FILE_PATH);
			
			int sub = 0;
			string field = "";

			for(sub = 1; sub < map.Headers.Count; sub++)
		    {
				field = map.Headers[sub];
				if(!string.IsNullOrEmpty(field))
					_localizations.AddLanguage(EnumUtility.ParseEnum<Language>(field),
						new Dictionary<string, string>());
			}

			for(int index = 0; index < map.Contents.Count; index++)
			{
				for(sub = 1; sub < map.Headers.Count; sub++)
				{
					field = map.Headers[sub];
					if(!string.IsNullOrEmpty(field))
						_localizations.AddKeyToLanguage(EnumUtility.ParseEnum<Language>(field),
							map.Contents[index][map.Headers[0]],map.Contents[index][map.Headers[sub]]);
				}
			}

			OnLoadLocalizations();

            FinishLanguageChange();
        }

		public static LocalizationDictionary CreateLocalizationDictionary(string path)
		{
			LocalizationDictionary localizations = new LocalizationDictionary();

			CSVMap map = new CSVMap(path);

			int sub = 0;
			string field = "";

			for(sub = 1; sub < map.Headers.Count; sub++)
			{
				field = map.Headers[sub];
				if(!string.IsNullOrEmpty(field))
					localizations.AddLanguage(EnumUtility.ParseEnum<Language>(field),
						new Dictionary<string, string>());
			}

			for(int index = 0; index < map.Contents.Count; index++)
			{
				for(sub = 1; sub < map.Headers.Count; sub++)
				{
					field = map.Headers[sub];
					if(!string.IsNullOrEmpty(field))
						localizations.AddKeyToLanguage(EnumUtility.ParseEnum<Language>(field),
						map.Contents[index][map.Headers[0]],map.Contents[index][map.Headers[sub]]);
				}
			}

			return localizations;
		}

        void FinishLanguageChange()
        {
            if (languageChanged != null)
				languageChanged(_localizations.GetLocalization(_currentLanguage));

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

		#region Virtual Methods
		protected virtual void OnLoadLocalizations(){}
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
                    /*case Language.French:
                        return "fr-FR";
                    case Language.German:
                        return "de-DE";
                    case Language.Italian:
                        return "it-IT";*/
                    case Language.Spanish:
                        return "es-ES";
				case Language.Portugese:
					return "pt-BR";
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

	public class LocalizationDictionary
	{
		#region Private Vars
		Dictionary<Language,Dictionary<string,string>> _dict;
		#endregion

		#region Constructors
		public LocalizationDictionary()
		{
			_dict = new Dictionary<Language, Dictionary<string, string>>();
		}
		#endregion

		#region Methods
		public void AddLanguage(Language key, Dictionary<string,string> values)
		{
			_dict.Add(key,values);
		}
		public void AddKeyToLanguage(Language lang, string key, string value)
		{
			_dict[lang].Add(key,value);
		}
		public void AddNewKey(Language lang, string key, string value, bool otherLanguages=false)
		{
			foreach(KeyValuePair<Language,Dictionary<string,string>> pair in _dict)
			{
				if(pair.Key == lang)
					pair.Value.Add(key,value);
				else
					pair.Value.Add(key,(otherLanguages) ? value :"");
			}
		}
		public void UpdateKey(Language lang, string key, string value, bool keepOtherValues=false)
		{
			foreach(KeyValuePair<Language,Dictionary<string,string>> pair in _dict)
			{
				if(pair.Key == lang)
					pair.Value[key] = value;
				else if(!keepOtherValues)
					pair.Value[key] = "";
			}
		}
		public Dictionary<string,string> GetLocalization(Language lang)
		{
			if(_dict.ContainsKey(lang))
				return _dict[lang];
			else
				return null;
		}
		public string GetValue(Language lang, string key)
		{
			if(!_dict.ContainsKey(lang))
				return "";
			else if(!_dict[lang].ContainsKey(key))
				return "";
			else
				return _dict[lang][key];
		}
		#endregion
	}
}
