using UnityEngine;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIAlert : MonoBehaviour 
    {
        #region Public Vars
        public bool FindElements;
        public bool SkipStack;
        public CanvasGroup CanvasGroup;
        public List<VisualElement> Elements;
        #endregion

        #region Private Vars
        bool _init;
        bool _open;
        #endregion

        #region Unity Methods
        void Awake()
        {
            enabled = false;
            Init();
        }
        void OnDestroy()
        {
            CleanUp();
        }
        #endregion

        #region Methods
        protected virtual void Init()
        {
            if(_init)
                return;

            if(FindElements)
            {
                VisualElement[] elements = GetComponentsInChildren<VisualElement>();

                Elements = new List<VisualElement>();

                for(int i = 0; i < elements.Length; i++)
                {
                    Elements.Add(elements[i]);
                }
            }

            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Init();
                Elements[i].PresentVisuals(false);
            }
            LoseFocus();

            _init = true;
        }
        protected virtual void CleanUp(){}

        public virtual void Open()
        {
            if(_open)
                return;

            _open = true;

            OnOpen();
        }
        protected virtual void OnOpen()
        {
            for(int i = 0; i < Elements.Count; i++)
            {
                if(!Elements[i].SkipUIViewActivation)
                    Elements[i].Present();
            }
            GainFocus();
            UIAlertController.Instance.PresentAlert(this);
        }

        public void Hide()
        {
            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Remove();
            }
        }
        public void UnHide()
        {
            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Present();
            }
        }

        public virtual void Close()
        {
            if(!_open)
                return;
            
            _open = false;

            OnClose();
        }
        protected virtual void OnClose()
        {
            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Remove();
            }
            LoseFocus();
            UIAlertController.Instance.RemoveAlert();
        }

        public virtual void GainFocus()
        {
            if(CanvasGroup)
            {
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.interactable = true;
            }
            /*for(int i = 0; i < _elements.Length; i++)
            {
                _elements[i].Enable();
            }*/
        }
        public virtual void LoseFocus()
        {
            if(CanvasGroup)
            {
                CanvasGroup.blocksRaycasts = false;
                CanvasGroup.interactable = false;
            }
            /*for(int i = 0; i < _elements.Length; i++)
            {
                _elements[i].Disable();
            }*/
        }
        public void AddElement(VisualElement element)
        {
            Elements.Add(element);
        }
        public void RemoveElement(VisualElement element)
        {
            Elements.Remove(element);
        }
        #endregion
    }
}
