using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
    public class IntroPage : ExtendedImage
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
            Image.sprite = sprite;
        }
        public void UnloadImage()
        {
            Resources.UnloadAsset(Image.sprite);
        }
        #endregion
    }
}
