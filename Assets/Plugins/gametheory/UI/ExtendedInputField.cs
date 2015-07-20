using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    public class ExtendedInputField : VisualElement
    {
        #region Events
        public event System.Action<string> OnSubmit;
        #endregion

        #region Public Vars
        //public InputField.ContentType _validation;
        public Text Label;
        public InputField InputField;
        #endregion

		#region Virtual Methods
		protected virtual void OnSubmitData(string value){}
		#endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!InputField)
                InputField = GetComponent<InputField>();

            //_inputField.contentType = _validation;
        }
        protected override void OnActivate()
        {
            base.OnActivate();

            if (InputField)
                InputField.onEndEdit.AddListener(SubmittedData);
        }
        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            if (InputField)
                InputField.onEndEdit.RemoveListener(SubmittedData);
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

            if(Label)
                Label.enabled = display;

            if (InputField)
            {
                InputField.enabled = display;

                if(InputField.image)
                    InputField.image.enabled = display;

                if(InputField.placeholder)
                {
                    //Debug.Log(display + " " + _inputField.text);
                    if (display)
                    {
                        if (InputField.text == "")
                            InputField.placeholder.enabled = display;
                    }
                    else
                        InputField.placeholder.enabled = display;
                }

                InputField.textComponent.enabled = display;

                InputField.interactable = display;
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
            get { return ((InputField) ? InputField.text : ""); }
            set 
            {
                if (InputField)
                {
                    if (InputField.text != value)
                    {
                        //_inputField.text.text = value;
                        InputField.text = value;

                        if(!_active && InputField.text == "")
                        {
                            if(InputField.placeholder)
                                InputField.placeholder.enabled = false;
                        }
                    }
                }
            }
        }
        public string PlaceholderText
        {
            set 
            {
                if (InputField)
                {
                    if (InputField.placeholder)
                    {
                        (InputField.placeholder as Text).text = value;
                    }
                }
            }
        }
        #endregion
    }
}