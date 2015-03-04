using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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
        public static event System.Action languageChanged;

        /// <summary>
        /// Occurs when language fails to load.
        /// </summary>
        public static event System.Action languageFailedToLoad;
        #endregion

        #region Constants
        const string LANGUAGE_KEY = "CurrentLanguage";
        #endregion

    	#region Public Variables
    	public string BaseFileName;
    	public Language DefaultLanguage;

        public LanguageToggle _english;
        public LanguageToggle _french;
        public LanguageToggle _spanish;
        public LanguageToggle _italian;
        public LanguageToggle _german;

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
        private XmlReader _reader;
        private FileStream _file;

        DateTimeFormatInfo _dateTimeFormatInfo;
        CultureInfo _cultureInfo;

    	private Dictionary<string, Dictionary<string, string>> _gameText;
    	#endregion

    	#region Unity Methods
    	// Use this for initialization
    	void Awake () 
    	{
            if (Instance == null)
            {
                LanguageToggle.changeLanguage += SetCurrentLanguage;

                Instance = this;

                _gameText = new Dictionary<string, Dictionary<string, string>>();
            }
            else
                Destroy(gameObject);
    	}
        void Start()
        {
            enabled = false;
            Load();
        }
        void OnDestroy()
        {
            LanguageToggle.changeLanguage -= SetCurrentLanguage;
        }
    	#endregion

    	#region Methods
        void Load()
        {
            string lang = PlayerPrefs.GetString(LANGUAGE_KEY, "");

            if (string.IsNullOrEmpty(lang))
                _currentLanguage = DefaultLanguage;

            if (LoadLocalization())
                FinishLanguageChange();

            SetToggle();
        }
        void Save()
        {
            PlayerPrefs.SetString(LANGUAGE_KEY, _currentLanguage.ToString());
            PlayerPrefs.Save();
        }
        void SetToggle()
        {
            switch (_currentLanguage)
            {
                case Language.English:
                    _english.IsOn = true;
                    break;
                case Language.French:
                    _french.IsOn = true;
                    break;
                case Language.German:
                    _german.IsOn = true;
                    break;
                case Language.Italian:
                    _italian.IsOn = true;
                    break;
                case Language.Spanish:
                    _spanish.IsOn = true;
                    break;
                default:
                    _english.IsOn = true;
                    break;
            }
        }

        public bool LoadLocalization()
    	{
            TextAsset asset = (TextAsset)Resources.Load(Path.Combine(BaseFileName, _currentLanguage.ToString()));

            if (asset == null)
            {
                LoadAlert.Instance.Close();
                LoadTimeout();
                return false;
            }

            _gameText = new Dictionary<string, Dictionary<string, string>>();

            StringReader strReader = new StringReader(asset.text);

            using(_reader = XmlReader.Create(strReader))
            {
                _reader.ReadToDescendant("Classes");

                XmlReader subReader = _reader.ReadSubtree();
                while(subReader.ReadToFollowing("Class"))
                {
                    var viewName = subReader.GetAttribute("name");

    				if(!_gameText.ContainsKey(viewName))
    					_gameText.Add(viewName, new Dictionary<string, string>());

                    XmlReader classReader = subReader.ReadSubtree();
                    while (classReader.ReadToFollowing("Variable"))
                    {
                        string content = "", name ="";
                        classReader.MoveToAttribute("name");

                        name = subReader.Value;
                        classReader.MoveToContent();
                        content = subReader.ReadString();
                        classReader.MoveToElement();

                        //Debug.Log(viewName + " " + name + " " + content);

                        _gameText[viewName].Add(name, content);
                    }
                    classReader.Close();
    			}
                subReader.Close();

                //read in other application specific data here
    		}

            return true;
    	}

        public IEnumerator LoadLocalizationCoroutine()
        {
            LoadAlert.Instance.StartLoad(LOADING_LANGUAGE, LoadTimeout, 1.0f, 10.0f, 0.15f);

            TextAsset asset = (TextAsset)Resources.Load(Path.Combine(BaseFileName, _currentLanguage.ToString()));

            if (asset != null)
            {
                _gameText = new Dictionary<string, Dictionary<string, string>>();

                StringReader strReader = new StringReader(asset.text);

                using (_reader = XmlReader.Create(strReader))
                {
                    _reader.ReadToDescendant("Classes");

                    XmlReader subReader = _reader.ReadSubtree();
                    while (subReader.ReadToFollowing("Class"))
                    {
                        var viewName = subReader.GetAttribute("name");

                        if (!_gameText.ContainsKey(viewName))
                            _gameText.Add(viewName, new Dictionary<string, string>());

                        XmlReader classReader = subReader.ReadSubtree();
                        while (classReader.ReadToFollowing("Variable"))
                        {
                            string content = "", name = "";
                            classReader.MoveToAttribute("name");

                            name = subReader.Value;
                            classReader.MoveToContent();
                            content = subReader.ReadString();
                            classReader.MoveToElement();

                            //Debug.Log(viewName + " " + name + " " + content);

                            _gameText[viewName].Add(name, content);
                        }
                        classReader.Close();

                        yield return null;
                    }
                    subReader.Close();

                    //read in other application specific data here
                }

                FinishLanguageChange();

                LoadAlert.Instance.Done();
            }
            else
            {
                LoadAlert.Instance.Done();
                LoadTimeout();
            }
        }

        public Dictionary<string, string> GetVariableText(string key)
        {
            return (_gameText.ContainsKey(key)) ? _gameText[key] : new Dictionary<string,string>();
        }
        void FinishLanguageChange()
        {
            if (languageChanged != null)
                languageChanged();

            Save();

            _cultureInfo = CultureInfo.GetCultureInfo(CultureString);
            _dateTimeFormatInfo = DateTimeFormatInfo.GetInstance(_cultureInfo);
        }
    	#endregion

    	#region Event Hooks
    	void SetCurrentLanguage(Language language)
    	{
            //Debug.Log(language);
            if (language != _currentLanguage)
            {
                _currentLanguage = language;

                StartCoroutine(LoadLocalizationCoroutine());
            }
    	}
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
        #endregion
    }
}
