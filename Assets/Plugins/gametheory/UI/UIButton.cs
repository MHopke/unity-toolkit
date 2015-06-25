using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    public class UIButton : UIBase
    {
        #region Public Vars
        public Button _button;

        public Text _text;

        public Image _buttonIconImage;

        public TextColorBlock _textColorBlock;
        #endregion

        #region Private Vars
        Color _originalColor;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!_button)
                _button = GetComponent<Button>();

            /*if (_button)
                _button.interactable = false;*/

            //Debug.Log(name + " : " + _button.interactable);

            if (!_text)
                _text = GetComponentInChildren<Text>();

        }
        protected override void Disabled()
        {
            base.Disabled();

            //Debug.Log(name + " : " + _button.interactable);

            if (_button)
            {
                _button.interactable = false;

                if (_buttonIconImage)
                    _buttonIconImage.color = _button.colors.disabledColor;
            }

            if(_text && _textColorBlock)
                _text.color = _textColorBlock._colorBlock.disabledColor;
        }

        protected override void Enabled()
        {
            base.Enabled();

            //Debug.Log(name + " : " + _button.interactable);

            if (_button)
            {
                _button.interactable = true;

                if (_buttonIconImage)
                    _buttonIconImage.color = _button.colors.normalColor;
            }

            if(_text && _textColorBlock)
                _text.color = _textColorBlock._colorBlock.normalColor;
        }

        public override void LostFocus()
        {
            base.LostFocus();

            if(_button)
                _previousEnabledState = _button.interactable;

            //Debug.Log(name + " : " + _previousEnabledState);

            Disable();
        }
        public override void GainedFocus()
        {
            base.GainedFocus();

            //Debug.Log(name + " : " + _previousEnabledState);

            if (_previousEnabledState)
                Enable();
        }

        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (_button)
            {
                if(_button.image)
                    _button.image.enabled = display;
            }

            if (_text)
                _text.enabled = display;
        }
        #endregion

        #region Accessors
        public bool Interactable
        {
            get { return (_button) ? _button.interactable : false; }
        }

        public string Text
        {
            get { return (_text) ? _text.text : ""; }
            set {
                if (_text)
                    _text.text = value;
            }
        }
        #endregion
    }
}
