using UnityEngine;

namespace gametheory.UI
{
    public class AnimatedButtonAnimation : MonoBehaviour 
    {
        #region Enums
        public enum Type {NONE = 0, SPRITE, HIGHLIGHT, ALPHA, SCALE, DOWN_SHIFT }
        #endregion

        #region Methods
        public virtual void Initialize(){}

        public virtual void Animate(){}

        public virtual void Restore(){}
        #endregion
    }
}