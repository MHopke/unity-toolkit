using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// Causes the button's scale to change when pressed.
    /// </summary>
    public class ABAScale : AnimatedButtonAnimation
    {
        #region Public Vars
        public float _downScale;
        #endregion

        #region Private Vars
        Vector3 _originalScale;
        #endregion

        #region Overriden Methods
        public override void Initialize()
        {
            base.Initialize();

            _originalScale = transform.localScale;
        }
        public override void Animate()
        {
            base.Animate();

            transform.SetXYScale(_originalScale.x * _downScale, _originalScale.y * _downScale);
        }
        public override void Restore()
        {
            base.Restore();


            transform.SetXYScale(_originalScale.x, _originalScale.y);
        }
        #endregion
    }
}