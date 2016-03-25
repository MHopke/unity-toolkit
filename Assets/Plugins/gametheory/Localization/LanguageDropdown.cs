using UnityEngine;

using System.Collections.Generic;

using gametheory.UI;

namespace gametheory.Localization
{
    public class LanguageDropdown : ExtendedDropdown 
    {
        #region Private Vars
        List<Language> _languages;
        #endregion

        #region Overridden Methods
        protected override void OnInit()
        {
            base.OnInit();

            _languages = new List<Language>();
            var enums = EnumUtility.GetValues<Language>();

            for(int index = 0; index < enums.Length; index++)
            {
                _languages.Add((Language)enums.GetValue(index));
            }
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
