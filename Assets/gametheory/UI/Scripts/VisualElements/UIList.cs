using UnityEngine;
using UnityEngine.UI;

using System.Collections;
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

        public Vector2 ResetPos;

        public VisualElement EmptyListItem;
        public Text DefaultListItemText;

		public RectTransform ContentTransform;

        public ScrollRect Scroll;
        public ExtendedScrollbar ScrollBar;

        public List<VisualElement> ListItems;
        #endregion

        #region Protected Vars
        protected int _endOfOriginalItems;

		protected VisualElement _itemPrefab;

		protected IEnumerable _listContext;
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
				VisualElement ele = null;
                for (int i = 0; i < ListItems.Count; i++)
                {
					ele = ListItems[i];
					ele.Init();
					ele.PresentVisuals(false);

					//add listeners to starting items
					AddListeners(ele as ListElement);
                }
            }
            else
                ListItems = new List<VisualElement>();
        }
		protected override void OnCleanUp ()
		{
			/*if(_listContext != null)
			{
				_listContext.itemChanged -= ItemChanged;
				_listContext.cleared -= ListCleared;
			}*/

			VisualElement ele = null;
			for (int i = 0; i < ListItems.Count; i++)
			{
				ele = ListItems[i];
				ele.CleanUp();
				//clear listeners on starting items
				if(i < _endOfOriginalItems)
					ClearListeners(ele as ListElement);
			}

			base.OnCleanUp ();
		}
        protected override void OnActivate ()
		{
			base.OnActivate ();

			VisualElement el = null;
            for(int i = 0; i < ListItems.Count; i++)
            {
				el = ListItems[i];
				//Debug.Log(el);
				if(!el.HiddenByDefault)
					el.Activate();
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
        public virtual void SetupList(VisualElement prefab, IEnumerable data,bool setupContent=true)
        {
            _itemPrefab = prefab;
            SetContext(data);

            int dif = GetItemCount(data);
            //setup extra items
            while (dif > 0)
            {
                AddItem(null);
                dif--;
            }
            // remove extra items
            while(dif < 0)
            {
                RemoveListElement(ListItems[ListItems.Count - 1]);
                dif++;
            }

            if(setupContent)
                SetupContent();
        }

        protected virtual void SetupContent()
        {
            if (_listContext == null)
                return;

            IEnumerator iter = _listContext.GetEnumerator();
            iter.Reset();
            int index = 0;
            while(iter.MoveNext())
            {
                ListItems[index].SetContext(iter.Current);
                index++;
            }

            ResetScrollPos();
        }

        protected virtual int GetItemCount(IEnumerable data)
        {
            if (data == null)
                return 0;
            else
                return data.Count() - ListItems.Count;
        }

        protected void ResetScrollPos()
        {
            if (Scroll)
            {
                if (Scroll.vertical)
                    Scroll.verticalNormalizedPosition = ResetPos.y;
                if (Scroll.horizontal)
                    Scroll.horizontalNormalizedPosition = ResetPos.x;
            }
        }

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
		public virtual void AddItem(object obj)
		{
			DeactivateEmptyItem();

            AddListElement(Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity));
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

		protected void ClearListeners(ListElement element)
		{
			if(element != null)
			{
				element.selected -= ItemSelected;
				element.delete -= ItemDeleted;
			}
		}
		protected void AddListeners(ListElement element)
		{
			if(element != null)
			{
				element.selected += ItemSelected;
				element.delete += ItemDeleted;
			}
		}
		/*protected void AddListeners(ListElement element,object obj)
		{
			if(element != null)
			{
				element.SetContext(obj);
				
				element.selected += ItemSelected;
				element.delete += ItemDeleted;
			}
		}*/


		public void SetItemPrefab(VisualElement element)
		{
			_itemPrefab = element;
		}

		public void SetContext(IEnumerable list)
		{
			_listContext = list;
			/*_listContext.itemChanged += ItemChanged;
			_listContext.cleared += ListCleared;*/
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

    #region IEnumerable Extenstions
    public static class IEnumerableExtensions
    {
        public static int Count(this IEnumerable source)
        {
            int res = 0;

            foreach (var item in source)
                res++;

            return res;
        }
    }
    #endregion
}
