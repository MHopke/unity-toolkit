using UnityEngine;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIAlert : MonoBehaviour 
    {
        #region Public Vars
        public bool _findElements;
        public bool _skipStack;
        public CanvasGroup _canvasGroup;
        public List<UIBase> _elements;
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

            if(_findElements)
            {
                UIBase[] elements = GetComponentsInChildren<UIBase>();

                _elements = new List<UIBase>();

                for(int i = 0; i < elements.Length; i++)
                {
                    _elements.Add(elements[i]);
                }
            }

            for(int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Init();
                _elements[i].PresentVisuals(false);
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
            for(int i = 0; i < _elements.Count; i++)
            {
                if(!_elements[i]._skipUIViewActivation)
                    _elements[i].Activate();
            }
            GainFocus();
            UIAlertController.Instance.PresentAlert(this);
        }

        public void Hide()
        {
            for(int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Deactivate();
            }
        }
        public void UnHide()
        {
            for(int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Activate();
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
            for(int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Deactivate();
            }
            LoseFocus();
            UIAlertController.Instance.RemoveAlert();
        }

        public virtual void GainFocus()
        {
            if(_canvasGroup)
            {
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
            }
            /*for(int i = 0; i < _elements.Length; i++)
            {
                _elements[i].Enable();
            }*/
        }
        public virtual void LoseFocus()
        {
            if(_canvasGroup)
            {
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
            }
            /*for(int i = 0; i < _elements.Length; i++)
            {
                _elements[i].Disable();
            }*/
        }
        public void AddElement(UIBase element)
        {
            _elements.Add(element);
        }
        public void RemoveElement(UIBase element)
        {
            _elements.Remove(element);
        }
        #endregion
    }
}
