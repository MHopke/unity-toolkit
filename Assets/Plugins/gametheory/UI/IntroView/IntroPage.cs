using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    public class IntroPage : UIImage
    {
        #region Public Vars
        public string Path;
        #endregion
        
        #region Methods
        public void LoadImage()
        {
            SetImage(Resources.Load<Sprite>(Path));
        }
        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }
        public void UnloadImage()
        {
            Resources.UnloadAsset(_image.sprite);
        }
        #endregion
    }
}
