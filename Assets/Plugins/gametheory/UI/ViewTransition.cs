using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    [System.Serializable]
    public class ViewTransition 
    {
        #region Public Vars
        public UIView CurrentView;
        public UIView NextView;
        public string ViewOutAnimation;
        public string ViewInAnimation;
        #endregion
    }
}
