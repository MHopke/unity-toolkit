using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    public class ExtendedButton : VisualElement
    {
        #region Public Vars
        public Button Button;

        public Text Label;

        public Image ButtonIconImage;

        public TextColorBlock TextColorBlock;
        #endregion

        #region Private Vars
        Color _originalColor;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!Button)
                Button = GetComponent<Button>();

            /*if (_button)
                _button.interactable = false;*/

            //Debug.Log(name + " : " + _button.interactable);

            if (!Label)
                Label = GetComponentInChildren<Text>();

        }
        protected override void Disabled()
        {
            base.Disabled();

            //Debug.Log(name + " : " + _button.interactable);

            if (Button)
            {
                Button.interactable = false;

                if (ButtonIconImage)
                    ButtonIconImage.color = Button.colors.disabledColor;
            }

            if(Label && TextColorBlock)
                Label.color = TextColorBlock.ColorBlock.disabledColor;
        }

        protected override void Enabled()
        {
            base.Enabled();

            //Debug.Log(name + " : " + _button.interactable);

            if (Button)
            {
                Button.interactable = true;

                if (ButtonIconImage)
                    ButtonIconImage.color = Button.colors.normalColor;
            }

            if(Label && TextColorBlock)
                Label.color = TextColorBlock.ColorBlock.normalColor;
        }

        public override void LostFocus()
        {
            base.LostFocus();

            if(Button)
                _previousEnabledState = Button.interactable;

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

            if (Button)
            {
                if(Button.image)
                    Button.image.enabled = display;
            }

            if (Label)
                Label.enabled = display;
        }
        #endregion

        #region Accessors
        public bool Interactable
        {
            get { return (Button) ? Button.interactable : false; }
        }

        public string Text
        {
            get { return (Label) ? Label.text : ""; }
            set {
                if (Label)
                    Label.text = value;
            }
        }
        #endregion
    }
}
