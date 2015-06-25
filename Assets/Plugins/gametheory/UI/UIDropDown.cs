using UnityEngine;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIDropDown : UIButton 
    {
        #region Public Vars
        public Transform ListParent;
        #endregion

        #region Protected Vars
        protected bool _dropDown;

        protected List<UIBase> _listItems;
        #endregion
        
        #region UI Methods
        public void Select()
        {
            _dropDown = ! _dropDown;

            if(_buttonIconImage)
                _buttonIconImage.transform.ScaleY(-1f);

            OnSelect();
        }
        #endregion
        
        #region Methods
        public void Initialize(string subject)
        {
            Text = subject;
        }
        #endregion

        #region Virtual Methods
        public virtual void InstatiateChild(UIBase prefab)
        {
            UIBase element = (UIBase)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            OnInstatiate(element);
            
            _listItems.Add(element);
            
            (element.transform as RectTransform).SetParent(ListParent, false);
        }
        protected virtual void OnInstatiate(UIBase element){}
        protected virtual void OnSelect(){}
        #endregion
    }
}