using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIListView : UIView 
    {
        #region Public Vars
        public bool _startsWithItems;
        public bool ZeroScrollBar;
        public bool HideScrollBar;

        public RectTransform _listTransform;

        public UIBase _emptyListItem;
        public Text _defaultListItemText;

        public ScrollRect Scroll;
        public UIScrollbar ScrollBar;

        public List<UIBase> _listItems;
        #endregion

        #region Overriden Methods
        protected override void OnInit(bool inCurrentViewController)
        {
            base.OnInit(inCurrentViewController);

            if (_startsWithItems)
            {
                for (int i = 0; i < _listItems.Count; i++)
                {
                    _listItems[i].Init();
                    _listItems[i].PresentVisuals(false);
                }
            }
            else
                _listItems = new List<UIBase>();
        }
        protected override void Activation()
        {
            base.Activation();

            for(int i = 0; i < _listItems.Count; i++)
            {
                if(!_listItems[i]._skipUIViewActivation)
                    _listItems[i].Activate();
            }

            CheckToHideScrollbar();
        }
        protected override void Deactivation()
        {
            base.Deactivation();

            for(int i = 0; i < _listItems.Count; i++)
            {
                _listItems[i].Remove();
            }

            if(ZeroScrollBar)
                Scroll.normalizedPosition = Vector2.zero;
        }
        public override void Exit()
        {
            base.Exit();
            for(int i = 0; i < _listItems.Count; i++)
            {
                _listItems[i].Exit();
            }
        }
        public override void GainedFocus()
        {
            base.GainedFocus();

            for(int i = 0; i < _listItems.Count; i++)
            {
                _listItems[i].GainedFocus();
            }
        }
        public override void LostFocus()
        {
            base.LostFocus();
            for(int i = 0; i < _listItems.Count; i++)
            {
                _listItems[i].LostFocus();
            }
        }
        #endregion

        #region Methods
        public void ActivateEmptyItem()
        {
            if(_emptyListItem)
            {
                _emptyListItem.gameObject.SetActive(true);
                _emptyListItem.Activate();
            }
        }
        public void DeactivateEmptyItem()
        {
            //Debug.Log(_emptyListItem.gameObject.activeSelf);
            if(_emptyListItem && _emptyListItem.gameObject.activeSelf)
            {
                _emptyListItem.gameObject.SetActive(false);
                _emptyListItem.Remove();
            }
        }

        public void AddListElement(UIBase element)
        {
            DeactivateEmptyItem();

            (element.transform as RectTransform).SetParent(_listTransform,false);
            _listItems.Add(element);
            element.Activate();

            CheckToShowScrollBar();
        }
        public void AddListElements(List<UIBase> elements)
        {
            DeactivateEmptyItem();

            for(int i = 0; i < elements.Count; i++)
            {
                (elements[i].transform as RectTransform).SetParent(_listTransform,false);
                _listItems.Add(elements[i]);
                elements[i].Activate();
            }

            CheckToShowScrollBar();
        }
        public void RemoveListElements(int count)
        {
            if(_listItems.Count == 0)
                return;

            int lastItem = _listItems.Count - 1;
            for(int i = 0; i < count; i++)
            {
                lastItem = _listItems.Count - 1;
                
                Destroy(_listItems[lastItem].gameObject);
                
                _listItems.RemoveAt(lastItem);
            }

            CheckToHideScrollbar();
        }
        public void RemoveListElement(UIBase element)
        {
            for(int i = 0; i < _listItems.Count; i++)
            {
                if(_listItems[i] == element)
                {
                    Destroy(_listItems[i].gameObject);
                    _listItems.RemoveAt(i);
                    return;
                }
            }
            CheckToHideScrollbar();
        }
        public void RemoveListElement(int index)
        {
            Destroy(_listItems[index].gameObject);
            _listItems.RemoveAt(index);

            CheckToHideScrollbar();
        }
        public void ClearElements()
        {
            while(_listItems.Count > 0)
            {
                Destroy(_listItems[0].gameObject);
                
                _listItems.RemoveAt(0);
            }

            CheckToHideScrollbar();
        }

        public void SetListItemStates(bool canInteract)
        {
            for (int i = 0; i < _listItems.Count; i++)
            {
                if (canInteract)
                    _listItems[i].Enable();
                else
                    _listItems[i].Disable();
            }
        }

        public void RegisterListChange()
        {
            if (HideScrollBar)
            {
                if (ScrollBar.Size >= 1f)
                    ScrollBar.Hide();
                else
                    ScrollBar.Show();
            }
        }

        void CheckToHideScrollbar()
        {
            if (HideScrollBar && ScrollBar.Size >= 1f)
                ScrollBar.Hide();
        }
        void CheckToShowScrollBar()
        {
            if (HideScrollBar && ScrollBar.Size > 1f)
                ScrollBar.Show();
        }
        #endregion
    }
}
