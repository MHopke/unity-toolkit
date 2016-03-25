using UnityEngine;

using System.Collections.Generic;

using gametheory.UI;

namespace gametheory.Localization
{
    public class LanguageDropdown : ExtendedDropdown 
    {
        #region Private Vars
        List<SystemLanguage> _languages;
        #endregion

        #region Overridden Methods
        protected override void OnInit()
        {
            base.OnInit();

			_languages = LocalizationManager.Instance.SupportedLanguages;
        }
        protected override void OnPresent()
        {
            base.OnPresent();

			Dropdown.value = (int)LocalizationManager.Instance.CurrentLanguage;
        }
        #endregion

        #region UI Methods
        public void LanguageDropdownChanged(int value)
        {
			LocalizationManager.Instance.SetCurrentLanguage(_languages[value]);
        }
        #endregion
    }
}
