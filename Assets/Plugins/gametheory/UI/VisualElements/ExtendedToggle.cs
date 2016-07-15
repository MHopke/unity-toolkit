using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
	[RequireComponent(typeof(Toggle))]
    public class ExtendedToggle : VisualElement
    {
		#region Events
		public event System.Action<VisualElement,bool> toggled;
		#endregion

        #region Public Vars
        public Toggle Toggle;
        public Text Text;
		public Image Icon;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!Toggle)
                Toggle = GetComponent<Toggle>();

            if (!Text)
                Text = GetComponentInChildren<Text>();
        }
        protected override void Disabled()
        {
            base.Disabled();

            if (Toggle)
                Toggle.interactable = false;
        }
        protected override void Enabled()
        {
            base.Enabled();

            if (Toggle)
                Toggle.interactable = true;
        }
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (Toggle)
            {
                if(Toggle.targetGraphic)
                    Toggle.targetGraphic.enabled = display;

                if (Toggle.graphic)
                {
                    //Debug.Log(display + " " + _toggle.isOn);

                    if(Toggle.isOn)
                        Toggle.graphic.enabled = display;
                }
            }

            if (Text)
                Text.enabled = display;
        }
        #endregion

		#region UI Method
		public void ToggleStatus(bool toggled)
		{
			OnToggle(toggled);
		}
		#endregion

        #region Methods
        protected virtual void HandleVisuals()
        {
            if (_active)
            {
                if (Toggle.graphic && !Toggle.graphic.enabled)
                    Toggle.graphic.enabled = true;
            }
            else
                Toggle.graphic.enabled = false;
        }
        #endregion

		#region Virtual Methods
		protected virtual void OnToggle(bool status)
		{
			if(toggled != null)
				toggled(this,status);
		}
		#endregion

        #region Accessors
        public bool IsOn
        {
            get { return Toggle.isOn; }
            set
            {
                if (Toggle)
                {
                    Toggle.isOn = value;

                    HandleVisuals();
                }
            }
        }
        #endregion
    }
}