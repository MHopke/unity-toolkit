using UnityEngine;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class DropDown : ExtendedButton 
    {
        #region Public Vars
        public Transform ListParent;
        #endregion

        #region Protected Vars
        protected bool _dropDown;

        protected List<VisualElement> _listItems;
        #endregion
        
        #region UI Methods
        public void Select()
        {
            _dropDown = ! _dropDown;

            if(ButtonIconImage)
                ButtonIconImage.transform.ScaleY(-1f);

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
        public virtual void InstatiateChild(VisualElement prefab)
        {
            VisualElement element = (VisualElement)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            OnInstatiate(element);
            
            _listItems.Add(element);
            
            (element.transform as RectTransform).SetParent(ListParent, false);
        }
        protected virtual void OnInstatiate(VisualElement element){}
        protected virtual void OnSelect(){}
        #endregion
    }
}