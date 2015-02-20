using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
    public class UIText : UIBase
    {
        #region Public Vars
        public Text _text;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!_text)
                _text = GetComponent<Text>();
        }
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (_text)
                _text.enabled = display;
        }
        #endregion

        #region Accessors
        public string Text
        {
            get { return ((_text) ? _text.text : ""); }
            set { if (_text) _text.text = value; }
        }
        #endregion
    }
}