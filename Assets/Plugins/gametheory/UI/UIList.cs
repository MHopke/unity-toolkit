using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIList : VisualElement 
    {
        #region Public Vars
        public bool StartsWithItems;
        public bool ZeroScrollBar;
        public bool HideScrollBar;

        public RectTransform LiistTransform;

        public VisualElement EmptyListItem;
        public Text DefaultListItemText;

        public ScrollRect Scroll;
        public ExtendedScrollbar ScrollBar;

        public List<VisualElement> ListItems;
        #endregion

        #region Overriden Methods
        protected override void OnInit()
        {
            base.OnInit();

            if (StartsWithItems)
            {
                for (int i = 0; i < ListItems.Count; i++)
                {
                    ListItems[i].Init();
                    ListItems[i].PresentVisuals(false);
                }
            }
            else
                ListItems = new List<VisualElement>();
        }
        protected override void OnActivate ()
		{
			base.OnActivate ();

            for(int i = 0; i < ListItems.Count; i++)
            {
                if(!ListItems[i].SkipUIViewActivation)
                    ListItems[i].Present();
            }

            CheckToHideScrollbar();
        }
        protected override void OnDeactivate ()
		{
			base.OnDeactivate ();

            for(int i = 0; i < ListItems.Count; i++)
            {
                ListItems[i].Remove();
            }

            if(ZeroScrollBar)
                Scroll.normalizedPosition = Vector2.zero;
        }
        public override void GainedFocus()
        {
            base.GainedFocus();

            for(int i = 0; i < ListItems.Count; i++)
            {
                ListItems[i].GainedFocus();
            }
        }
        public override void LostFocus()
        {
            base.LostFocus();
            for(int i = 0; i < ListItems.Count; i++)
            {
                ListItems[i].LostFocus();
            }
        }
        #endregion

        #region Methods
        public void ActivateEmptyItem()
        {
            if(EmptyListItem)
            {
                EmptyListItem.gameObject.SetActive(true);
                EmptyListItem.Present();
            }
        }
        public void DeactivateEmptyItem()
        {
            //Debug.Log(_emptyListItem.gameObject.activeSelf);
            if(EmptyListItem && EmptyListItem.gameObject.activeSelf)
            {
                EmptyListItem.gameObject.SetActive(false);
                EmptyListItem.Remove();
            }
        }

        public void AddListElement(VisualElement element)
        {
            DeactivateEmptyItem();

            (element.transform as RectTransform).SetParent(LiistTransform,false);
            ListItems.Add(element);
            element.Present();

            CheckToShowScrollBar();
        }
        public void AddListElements(List<VisualElement> elements)
        {
            DeactivateEmptyItem();

            for(int i = 0; i < elements.Count; i++)
            {
                (elements[i].transform as RectTransform).SetParent(LiistTransform,false);
                ListItems.Add(elements[i]);
                elements[i].Present();
            }

            CheckToShowScrollBar();
        }
        public void RemoveListElements(int count)
        {
            if(ListItems.Count == 0)
                return;

            int lastItem = ListItems.Count - 1;
            for(int i = 0; i < count; i++)
            {
                lastItem = ListItems.Count - 1;
                
                Destroy(ListItems[lastItem].gameObject);
                
                ListItems.RemoveAt(lastItem);
            }

            CheckToHideScrollbar();
        }
        public void RemoveListElement(VisualElement element)
        {
            for(int i = 0; i < ListItems.Count; i++)
            {
                if(ListItems[i] == element)
                {
                    Destroy(ListItems[i].gameObject);
                    ListItems.RemoveAt(i);
                    return;
                }
            }
            CheckToHideScrollbar();
        }
        public void RemoveListElement(int index)
        {
            Destroy(ListItems[index].gameObject);
            ListItems.RemoveAt(index);

            CheckToHideScrollbar();
        }
        public void ClearElements()
        {
            while(ListItems.Count > 0)
            {
                Destroy(ListItems[0].gameObject);
                
                ListItems.RemoveAt(0);
            }

            CheckToHideScrollbar();
        }

        public void SetListItemStates(bool canInteract)
        {
            for (int i = 0; i < ListItems.Count; i++)
            {
                if (canInteract)
                    ListItems[i].Enable();
                else
                    ListItems[i].Disable();
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
