using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace gametheory.UI
{
    public class UIList : VisualElement 
    {
		#region Events
		public event System.Action<VisualElement> itemDeleted;
		public event System.Action<VisualElement,object> itemSelected;
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
        protected override void OnActivate ()
		{
			base.OnActivate ();

            for(int i = 0; i < ListItems.Count; i++)
            {
                if(!ListItems[i].HiddenByDefault)
                    ListItems[i].Activate();
            }
        }
        protected override void OnDeactivate ()
		{
			base.OnDeactivate ();

            for(int i = 0; i < ListItems.Count; i++)
            {
                ListItems[i].Deactivate();
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
                EmptyListItem.Activate();
            }
        }
        public void DeactivateEmptyItem()
        {
            //Debug.Log(_emptyListItem.gameObject.activeSelf);
            if(EmptyListItem && EmptyListItem.gameObject.activeSelf)
            {
                EmptyListItem.gameObject.SetActive(false);
                EmptyListItem.Deactivate();
            }
        }

        public void AddListElement(VisualElement element)
        {
            DeactivateEmptyItem();

			(element.transform as RectTransform).SetParent(ContentTransform,false);
            ListItems.Add(element);

            element.Init();

			AddListeners(element as ListElement);

            if(_active)
                element.Activate();
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
			
			AddListeners(element as ListElement,obj);
			
			if(_active)
				element.Activate();
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

            for(int i = 0; i < count; i++)
            {
                lastItem = ListItems.Count - 1;

				ClearListeners(ListItems[lastItem] as ListElement);

                element.CleanUp();

                Destroy(element.gameObject);
                
                ListItems.RemoveAt(lastItem);
            }
        }
        public void RemoveListElement(VisualElement element)
        {
			VisualElement listItem = null;
            for(int i = 0; i < ListItems.Count; i++)
            {
				listItem = ListItems[i];
                if(listItem == element)
                {
					ClearListeners(listItem as ListElement);

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
			ClearListeners(temp);

            Destroy(ListItems[index].gameObject);
            ListItems.RemoveAt(index);
        }
        public void ClearElements()
        {
			VisualElement element = null;
            while(ListItems.Count > _endOfOriginalItems)
            {
				element = ListItems[ListItems.Count - 1];
                
				ClearListeners(element as ListElement);

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

		void ClearListeners(ListElement element)
		{
			if(element != null)
			{
				element.selected -= ItemSelected;
				element.delete -= ItemDeleted;
			}
		}
		void AddListeners(ListElement element)
		{
			if(element != null)
			{
				element.selected += ItemSelected;
				element.delete += ItemDeleted;
			}
		}
		void AddListeners(ListElement element,object obj)
		{
			if(element != null)
			{
				element.Setup(obj);
				
				element.selected += ItemSelected;
				element.delete += ItemDeleted;
			}
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
		void ItemSelected(VisualElement sender, object obj)
		{
			if(itemSelected != null)
				itemSelected(sender, obj);
		}
		void ItemDeleted(VisualElement sender)
		{
			if(itemDeleted != null)
				itemDeleted(sender);
		}
		#endregion
    }
}
