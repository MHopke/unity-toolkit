using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    public class UIToggle : UIBase
    {
        #region Public Vars
        public Toggle _toggle;
        public Text _text;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!_toggle)
                _toggle = GetComponent<Toggle>();

            if (!_text)
                _text = GetComponentInChildren<Text>();
        }
        protected override void Disabled()
        {
            base.Disabled();

            if (_toggle)
                _toggle.interactable = false;
        }
        protected override void Enabled()
        {
            base.Enabled();

            if (_toggle)
                _toggle.interactable = true;
        }
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (_toggle)
            {
                _toggle.targetGraphic.enabled = display;
                _toggle.graphic.enabled = display;
            }

            if (_text)
                _text.enabled = display;
        }
        #endregion

        #region Accessors
        public bool Active
        {
            get { return _toggle.isOn; }
            set { _toggle.isOn = value; }

        }
        #endregion
    }
}