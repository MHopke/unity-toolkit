using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    /// <summary>
    /// Causes the button to animate 
    /// </summary>
    public class ABATrigger : AnimatedButtonAnimation
    {
        #region Public Vars
        public string _animationDownKey;
        public string _animationUpKey;

        public Animator _animator;
        #endregion

        #region Overriden Methods
        public override void Initialize()
        {
            base.Initialize();

            if(!_animator)
                _animator = GetComponent<Animator>();
        }

        public override void Animate()
        {
            base.Animate();

            _animator.SetTrigger(_animationDownKey);
        }
        public override void Restore()
        {
            base.Restore();

            _animator.SetTrigger(_animationUpKey);
        }
        #endregion
    }
}