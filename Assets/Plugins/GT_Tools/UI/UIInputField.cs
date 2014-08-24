﻿using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    public class UIInputField : UIBase
    {
        #region Public Vars
        public InputField _inputField;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!_inputField)
                _inputField = GetComponent<InputField>();
        }
        protected override void Disabled()
        {
            base.Disabled();

            if (_inputField)
                _inputField.interactable = false;
        }
        protected override void Enabled()
        {
            base.Enabled();

            if (_inputField)
                _inputField.interactable = true;
        }
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (_inputField)
            {
                _inputField.image.enabled = display;
                _inputField.text.enabled = display;
            }
        }
        #endregion
    }
}