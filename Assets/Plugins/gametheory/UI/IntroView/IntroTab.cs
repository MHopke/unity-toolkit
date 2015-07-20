using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    public class IntroTab : ExtendedImage
    {
        #region Methods
        public void SwitchStatus(bool status)
        {
            if(status)
                OnActive();
            else
                OnInactive();
        }
        #endregion
        
        #region Virtual Methods
        protected virtual void OnActive(){}
        protected virtual void OnInactive(){}
        #endregion
    }
}
