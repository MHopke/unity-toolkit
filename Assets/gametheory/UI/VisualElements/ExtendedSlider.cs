using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace gametheory.UI
{
	[RequireComponent(typeof(Slider))]
    public class ExtendedSlider : VisualElement
    {
        #region Public Vars
        public bool CanInteract;

        public Slider Slider;

        public Image FillImage;
        public Image BackgroundImage;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (!Slider)
                Slider = GetComponent<Slider>();

            if (!BackgroundImage)
                BackgroundImage = GetComponent<Image>();
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

            if (Slider)
            {
                if(Slider.image)
                    Slider.image.enabled = display;

                if (BackgroundImage)
                    BackgroundImage.enabled = display;

                if (Slider.targetGraphic)
                    Slider.targetGraphic.enabled = display;

                if (FillImage)
                    FillImage.enabled = display;
            }
        }
        public override void OnLostFocus()
        {
            base.OnLostFocus();
            _previousEnabledState = Slider.interactable;

            Disabled();
        }
        public override void OnGainedFocus()
        {
            base.OnGainedFocus();

            if (_previousEnabledState)
                Enabled();
        }
        protected override void Disabled()
        {
            base.Disabled();

            if (Slider)
            {
                Slider.interactable = false;
            }
        }
        protected override void Enabled()
        {
            base.Enabled();

            if (Slider)
            {
                Slider.interactable = true;
            }
                
        }
        #endregion

        #region Accessors
        public float Value
        {
            get { return (Slider) ? Slider.value : 0f; }
            set { if (Slider) Slider.value = value; }
        }
        public float Max
        {
            get { return (Slider) ? Slider.maxValue : 0f; }
            set { if (Slider) Slider.maxValue = value; }
        }
        public float Min
        {
            get { return (Slider) ? Slider.minValue : 0f; }
            set { if (Slider) Slider.minValue = value; }
        }
        #endregion
    }
}
