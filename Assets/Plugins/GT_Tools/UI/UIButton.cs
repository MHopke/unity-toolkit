using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    public class UIButton : UIBase
    {
        #region Public Vars
        public Button _button;
        public Text _text;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!_button)
                _button = GetComponent<Button>();

            if (!_text)
                _text = GetComponentInChildren<Text>();
        }
        protected override void Disabled()
        {
            base.Disabled();

            if (_button)
                _button.interactable = false;
        }

        protected override void Enabled()
        {
            base.Enabled();

            if (_button)
                _button.interactable = true;
        }
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (_button)
                _button.image.enabled = display;

            if (_text)
                _text.enabled = display;
        }
        #endregion
    }
}
