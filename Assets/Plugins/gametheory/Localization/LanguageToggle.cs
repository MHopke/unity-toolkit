using UnityEngine;
using gametheory.UI;
using gametheory.Localization;
using System.Collections;

/// <summary>
/// A premade component that can be used as a toggle for
/// selecting a language. Utilizes gametheory's UI system.
/// </summary>
public class LanguageToggle : ExtendedToggle 
{
    #region Events
    public static event System.Action<Language> changeLanguage;
    #endregion

    #region Public Vars
    public Language _language;
    #endregion

    #region UI Methods
    public void Toggle(bool toggled)
    {
        if (_active && toggled && changeLanguage != null )
        {
            changeLanguage(_language);
        }
    }
    #endregion
}
