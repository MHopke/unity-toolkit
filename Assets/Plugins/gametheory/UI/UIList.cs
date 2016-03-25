using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIList : VisualElement 
    {
		#region Events
		public event System.Action<object> itemSelected;
		#endregion

        #region Public Vars
        public bool StartsWithItems;
        public bool ZeroScrollBar;
        public bool HideScrollBar;

        public VisualElement EmptyListItem;
        public Text DefaultListItemText;

		public RectTransform ContentTransform;

        public ScrollRect Scroll;
        public ExtendedScrollbar ScrollBar;

        public List<VisualElement> ListItems;
        #endregion

        #region Private Vars
        int _endOfOriginalItems;

		VisualElement _itemPrefab;

		ObservableList<object> _listContext;
        #endregion

        #region Overriden Methods
		public override void PresentVisuals (bool display)
		{
			base.PresentVisuals (display);

			if(Scroll)
				Scroll.enabled = display;
		}
        protected override void OnInit()
        {
            base.OnInit();

			if(ContentTransform == null && Scroll != null)
				ContentTransform = Scroll.content;

            if (StartsWithItems)
            {
                _endOfOriginalItems = ListItems.Count;
                for (int i = 0; i < ListItems.Count; i++)
                {
                    ListItems[i].Init();
                    ListItems[i].PresentVisuals(false);
                }
            }
            else
                ListItems = new List<VisualElement>();
        }
		protected override void OnCleanUp ()
		{
			if(_listContext != null)
			{
				_listContext.itemChanged -= ItemChanged;
				_listContext.cleared -= ListCleared;
			}

			base.OnCleanUp ();
		}
        protected override void OnPresent ()
		{
			base.OnPresent ();

            for(int i = 0; i < ListItems.Count; i++)
            {
                if(!ListItems[i].HiddenByDefault)
                    ListItems[i].Present();
            }
        }
        protected override void OnRemove ()
		{
			base.OnRemove ();

            for(int i = 0; i < ListItems.Count; i++)
            {
                ListItems[i].Remove();
            }

            if(ZeroScrollBar)
                Scroll.normalizedPosition = Vector2.zero;
        }
        public override void OnGainedFocus()
        {
            base.OnGainedFocus();

            for(int i = 0; i < ListItems.Count; i++)
            {
                ListItems[i].OnGainedFocus();
            }
        }
        public override void OnLostFocus()
        {
            base.OnLostFocus();
            for(int i = 0; i < ListItems.Count; i++)
            {
                ListItems[i].OnLostFocus();
            }
        }
		protected override void OnHide ()
		{
			base.OnHide ();
			for(int i = 0; i < ListItems.Count; i++)
			{
				ListItems[i].Hide();
			}
		}
		protected override void OnShow ()
		{
			base.OnShow ();
			for(int i = 0; i < ListItems.Count; i++)
			{
				ListItems[i].Show();
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

			(element.transform as RectTransform).SetParent(ContentTransform,false);
            ListItems.Add(element);

            element.Init();

			ListElement temp = element as ListElement;
			if(temp != null)
				temp.selected += ItemSelected;

            if(_active)
                element.Present();
            else
                element.PresentVisuals(false);
        }
		public void AddListElementToTop(VisualElement element)
		{
			AddListElement(element);
			element.transform.SetAsFirstSibling();
		}
		public void AddItem(object obj)
		{
			DeactivateEmptyItem();

			VisualElement element = (VisualElement)GameObject.Instantiate(_itemPrefab,Vector3.zero,Quaternion.identity);

			(element.transform as RectTransform).SetParent(ContentTransform,false);
			ListItems.Add(element);
			
			element.Init();
			
			ListElement temp = element as ListElement;
			if(temp != null)
			{
				temp.Setup(obj);
				temp.selected += ItemSelected;
			}
			
			if(_active)
				element.Present();
			else
				element.PresentVisuals(false);
		}
        public void AddListElements(List<VisualElement> elements)
        {
            DeactivateEmptyItem();

            for(int i = 0; i < elements.Count; i++)
            {
				AddListElement(elements[i]);
            }
        }
        public void RemoveListElements(int count)
        {
            if(ListItems.Count == 0)
                return;

            int lastItem = ListItems.Count - 1;
			VisualElement element = null;
			ListElement temp = null;

            for(int i = 0; i < count; i++)
            {
                lastItem = ListItems.Count - 1;

				temp = ListItems[lastItem] as ListElement;
				if(temp != null)
					temp.selected -= ItemSelected;

                element.CleanUp();

                Destroy(element.gameObject);
                
                ListItems.RemoveAt(lastItem);
            }
        }
        public void RemoveListElement(VisualElement element)
        {
			VisualElement listItem = null;
			ListElement temp = null;
            for(int i = 0; i < ListItems.Count; i++)
            {
				listItem = ListItems[i];
                if(listItem == element)
                {
					temp = listItem as ListElement;
					if(temp != null)
						temp.selected -= ItemSelected;

                    listItem.CleanUp();
                    Destroy(listItem.gameObject);
                    ListItems.RemoveAt(i);
                    return;
                }
            }
        }
        public void RemoveListElement(int index)
        {
			ListElement temp = ListItems[index] as ListElement;
			if(temp != null)
				temp.selected -= ItemSelected;

            Destroy(ListItems[index].gameObject);
            ListItems.RemoveAt(index);
        }
        public void ClearElements()
        {
			VisualElement element = null;
			ListElement temp = null;
            while(ListItems.Count > _endOfOriginalItems)
            {
				element = ListItems[ListItems.Count - 1];
                
				temp = element as ListElement;
				if(temp != null)
					temp.selected -= ItemSelected;

				element.CleanUp();

                Destroy(element.gameObject);
                
                ListItems.RemoveAt(ListItems.Count - 1);
            }
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

		void ItemSelected(object obj)
		{
			if(itemSelected != null)
				itemSelected(obj);
		}

		public void SetItemPrefab(VisualElement element)
		{
			_itemPrefab = element;
		}

		public void SetContext(ObservableList<object> list)
		{
			_listContext = list;
			_listContext.itemChanged += ItemChanged;
			_listContext.cleared += ListCleared;
		}
        #endregion

		#region Event Listeners
		void ItemChanged(int index, object obj)
		{
			if(obj == null)
				RemoveListElement(index);
			else if(ListItems.Count < index)
				AddItem(obj);
		}
		void ListCleared()
		{
			ClearElements();
		}
		#endregion
    }
}
