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
                if(_toggle.targetGraphic)
                    _toggle.targetGraphic.enabled = display;

                if (_toggle.graphic)
                {
                    //Debug.Log(display + " " + _toggle.isOn);

                    if(_toggle.isOn)
                        _toggle.graphic.enabled = display;
                }
            }

            if (_text)
                _text.enabled = display;
        }
        #endregion

        #region Methods
        protected virtual void HandleVisuals()
        {
            if (_active)
            {
                if (_toggle.graphic && !_toggle.graphic.enabled)
                    _toggle.graphic.enabled = true;
            }
            else
                _toggle.graphic.enabled = false;
        }
        #endregion

        #region Accessors
        public bool IsOn
        {
            get { return _toggle.isOn; }
            set
            {
                if (_toggle)
                {
                    _toggle.isOn = value;

                    HandleVisuals();
                }
            }
        }
        #endregion
    }
}