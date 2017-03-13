using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
	[RequireComponent(typeof(Text))]
    public class ExtendedText : VisualElement
    {
        #region Public Vars
        public Text Label;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!Label)
                Label = GetComponent<Text>();
        }
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (Label)
                Label.enabled = display;
        }
        #endregion

		#region Methods
		public void SetTextBinding(string propName, string format="{0}")
		{
			SetBinding(propName,new TextBinding(Label,format));
		}
		#endregion

        #region Propeties
        public string Text
        {
            get { return ((Label) ? Label.text : ""); }
            set { if (Label) Label.text = value; }
        }
        #endregion
    }
}