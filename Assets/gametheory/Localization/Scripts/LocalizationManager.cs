using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using gametheory.Utilities;

namespace gametheory.Localization
{
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
		public List<SystemLanguage> SupportedLanguages;
        public static LocalizationManager Instance = null;
    	#endregion

    	#region Protected Variables
		protected SystemLanguage _defaultLanguage;
		protected SystemLanguage _currentLanguage;

        protected DateTimeFormatInfo _dateTimeFormatInfo;
        protected CultureInfo _cultureInfo;

		protected Dictionary<string,string> _stringContent;//LocalizationDictionary _localizations;
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
				_defaultLanguage = Application.systemLanguage;

                Instance = this;

				_stringContent = new Dictionary<string, string>();//_localizations = new LocalizationDictionary();
            }
            else
                Destroy(gameObject);
    	}
    	#endregion

    	#region Methods
		public void Load()
        {
            string lang = PlayerPrefs.GetString(LANGUAGE_KEY, "");

            if (string.IsNullOrEmpty(lang))
                _currentLanguage = _defaultLanguage;
			else
				_currentLanguage = EnumUtility.ParseEnum<SystemLanguage>(lang);

			LoadLocalization();
        }
        void Save()
        {
            PlayerPrefs.SetString(LANGUAGE_KEY, _currentLanguage.ToString());
            PlayerPrefs.Save();
        }

		public Dictionary<string,string> GetCurrentLocalization()
		{
			return _stringContent;//_localizations.GetLocalization(_currentLanguage);
		}

		public string GetSpecificLocalization(string key)
		{
			if(_stringContent.ContainsKey(key))
				return _stringContent[key];
			else
				return key;
		}

        public void LoadLocalization()
        {
			_stringContent = CreateDictionary(FILE_PATH);

			OnLoadLocalizations();

            FinishLanguageChange();
        }

		public static Dictionary<string,string> CreateDictionary(string path)
		{
			Dictionary<string,string> dict = new Dictionary<string,string>();

			CSVMap map = new CSVMap(path);

			int index = 0, langIndex =0;;
			string field = "", langStr = Instance._currentLanguage.ToString();

			for(index = 0; index < map.Contents.Count; index++)
				dict.Add(map.Contents[index][map.Headers[0]],map.Contents[index][langStr]);

			return dict;
		}

        void FinishLanguageChange()
        {
            if (languageChanged != null)
				languageChanged(_stringContent);

            Save();

            _cultureInfo = CultureInfo.GetCultureInfo(CultureString);
            _dateTimeFormatInfo = DateTimeFormatInfo.GetInstance(_cultureInfo);
        }

		public void SetCurrentLanguage(SystemLanguage language)
		{
			//Debug.Log(language);
			if (language != _currentLanguage)
			{
				_currentLanguage = language;

				LoadLocalization();
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
                    case SystemLanguage.English:
                        return "en-US";
                    /*case Language.French:
                        return "fr-FR";
                    case Language.German:
                        return "de-DE";
                    case Language.Italian:
                        return "it-IT";*/
				case SystemLanguage.Spanish:
                        return "es-ES";
				case SystemLanguage.Portuguese:
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
		public SystemLanguage CurrentLanguage
		{
			get { return _currentLanguage; }
		}
        #endregion
    }
}
