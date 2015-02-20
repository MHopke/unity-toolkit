using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    public class UIInputField : UIBase
    {
        #region Events
        public event System.Action<string> OnSubmit;
        #endregion

        #region Public Vars
        //public InputField.ContentType _validation;
        public Text _label;
        public InputField _inputField;
        #endregion

		#region Virtual Methods
		protected virtual void OnSubmitData(string value){}
		#endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!_inputField)
                _inputField = GetComponent<InputField>();

            //_inputField.contentType = _validation;
        }
        protected override void OnActivate()
        {
            base.OnActivate();

            if (_inputField)
                _inputField.onEndEdit.AddListener(SubmittedData);
        }
        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            if (_inputField)
                _inputField.onEndEdit.RemoveListener(SubmittedData);
        }
        /*protected override void Disabled()
        {
            base.Disabled();

            if (_inputField)
            {
                _inputField.enabled = false;
                _inputField.interactable = false;
            }
        }
        protected override void Enabled()
        {
            base.Enabled();

            if (_inputField)
            {
                _inputField.enabled = true;
                _inputField.interactable = true;
            }
        }*/
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if(_label)
                _label.enabled = display;

            if (_inputField)
            {
                _inputField.enabled = display;

                if(_inputField.image)
                    _inputField.image.enabled = display;

                if(_inputField.placeholder)
                {
                    //Debug.Log(display + " " + _inputField.text);
                    if (display)
                    {
                        if (_inputField.text == "")
                            _inputField.placeholder.enabled = display;
                    }
                    else
                        _inputField.placeholder.enabled = display;
                }

                _inputField.textComponent.enabled = display;

                _inputField.interactable = display;
            }
        }
        #endregion

        #region InputField Event Listeners
        void SubmittedData(string value)
        {
			OnSubmitData(value);

            if (OnSubmit != null)
                OnSubmit(value);
        }
        #endregion

        #region Accessors
        public string Text
        {
            get { return ((_inputField) ? _inputField.text : ""); }
            set 
            {
                if (_inputField)
                {
                    if (_inputField.text != value)
                    {
                        //_inputField.text.text = value;
                        _inputField.text = value;

                        if(!_active && _inputField.text == "")
                        {
                            if(_inputField.placeholder)
                                _inputField.placeholder.enabled = false;
                        }
                    }
                }
            }
        }
        public string PlaceholderText
        {
            set 
            {
                if (_inputField)
                {
                    if (_inputField.placeholder)
                    {
                        (_inputField.placeholder as Text).text = value;
                    }
                }
            }
        }
        #endregion
    }
}