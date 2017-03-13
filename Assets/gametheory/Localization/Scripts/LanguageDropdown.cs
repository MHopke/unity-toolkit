using UnityEngine;

using System.Collections.Generic;

using gametheory.UI;

namespace gametheory.Localization
{
	/// <summary>
	/// Language dropdown. During setup include the possible languages in
	/// the toggle's OptionList in the editor
	/// </summary>
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
        protected override void OnActivate()
        {
            base.OnActivate();

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
