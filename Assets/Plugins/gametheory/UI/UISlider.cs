using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace gametheory.UI
{
    public class UISlider : UIBase
    {
        #region Public Vars
        public bool _canInteract;
        public Slider _slider;
        public Image _fillImage;
        public Image _backgroundImage;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!_slider)
                _slider = GetComponent<Slider>();

            if (!_backgroundImage)
                _backgroundImage = GetComponent<Image>();
        }
        /*protected override void Disabled()
        {
            base.Disabled();

            if (_canInteract && _slider)
                _slider.interactable = false;
        }
        protected override void Enabled()
        {
            base.Enabled();

            if (_canInteract && _slider)
                _slider.interactable = true;
        }*/
        public override void PresentVisuals(bool display)
        {
            base.PresentVisuals(display);

            if (_slider)
            {
                if(_slider.image)
                    _slider.image.enabled = display;

                if (_backgroundImage)
                    _backgroundImage.enabled = display;

                if (_slider.targetGraphic)
                    _slider.targetGraphic.enabled = display;

                if (_fillImage)
                    _fillImage.enabled = display;
            }
        }
        public override void LostFocus()
        {
            base.LostFocus();
            _previousEnabledState = _slider.interactable;

            Disabled();
        }
        public override void GainedFocus()
        {
            base.GainedFocus();

            if (_previousEnabledState)
                Enabled();
        }
        protected override void Disabled()
        {
            base.Disabled();

            if (_slider)
            {
                _slider.interactable = false;
            }
        }
        protected override void Enabled()
        {
            base.Enabled();

            if (_slider)
            {
                _slider.interactable = true;
            }
                
        }
        #endregion

        #region Accessors
        public float Value
        {
            get { return (_slider) ? _slider.value : 0f; }
            set { if (_slider) _slider.value = value; }
        }
        public float Max
        {
            get { return (_slider) ? _slider.maxValue : 0f; }
            set { if (_slider) _slider.maxValue = value; }
        }
        public float Min
        {
            get { return (_slider) ? _slider.minValue : 0f; }
            set { if (_slider) _slider.minValue = value; }
        }
        #endregion
    }
}
