using UnityEngine;

namespace gametheory.UI
{
    /// <summary>
    /// Causes the button's sprite to change when pressed.
    /// </summary>
    public class ABASprite : AnimatedButtonAnimation
    {
        #region Public Vars
        public Sprite _pressedSprite;
        #endregion

        #region Private Vars
        Sprite _originalSprite;
        SpriteRenderer _renderer;
        #endregion

        #region Overriden Methods
        public override void Initialize()
        {
            base.Initialize();

            if(!_renderer)
                _renderer = renderer as SpriteRenderer;

            _originalSprite = _renderer.sprite;
        }
        public override void Animate()
        {
            base.Animate();

            _renderer.sprite = _pressedSprite;
        }
        public override void Restore()
        {
            base.Restore();

            _renderer.sprite = _originalSprite;
        }
        #endregion
    }
}