using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
	[RequireComponent(typeof(Button))]
    public class ExtendedButton : VisualElement
    {
		#region Events
		public event System.Action<VisualElement> selected;
		#endregion

        #region Public Vars
        public Button Button;

        public Text Label;

        public Image ButtonIconImage;

        public TextColorBlock TextColorBlock;
        #endregion

        #region Private Vars
		bool _iconWasEnabled;
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

            //Debug.Log(name + " : " + Button.interactable);

            if (Button)
            {
                Button.interactable = true;

                if (ButtonIconImage)
                    ButtonIconImage.color = Button.colors.normalColor;
            }

            if(Label && TextColorBlock)
                Label.color = TextColorBlock.ColorBlock.normalColor;
        }

        public override void OnLostFocus()
        {
            base.OnLostFocus();

            if(Button)
                _previousEnabledState = Button.interactable;

            //Debug.Log(name + " : " + _previousEnabledState);

            Disable();
        }
        public override void OnGainedFocus()
        {
            base.OnGainedFocus();

            //Debug.Log(name + " : " + _previousEnabledState);

            if (_previousEnabledState)
                Enable();
        }

		protected override void OnShow ()
		{
			base.OnShow ();

			if(ButtonIconImage)
				ButtonIconImage.enabled = _iconWasEnabled;
		}
		protected override void OnHide ()
		{
			base.OnHide ();

			if(ButtonIconImage)
				_iconWasEnabled = ButtonIconImage.enabled;
		}

        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (Button)
            {
                Button.enabled = display;

                if(Button.image)
                    Button.image.enabled = display;
            }

            if(ButtonIconImage)
			{
                ButtonIconImage.enabled = display;
				_iconWasEnabled = display;
			}

            if (Label)
                Label.enabled = display;
        }
        #endregion

		#region UI Methods
		public void Selected()
		{
			OnSelected();
		}
		#endregion

		#region Virtual Methods
		protected virtual void OnSelected()
		{
			if(selected != null)
				selected(this);
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
