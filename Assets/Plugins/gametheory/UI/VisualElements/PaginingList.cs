using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

namespace gametheory.UI
{
	public class PaginingList : UIList 
	{
		#region Public Vars
		public int ItemsPerPage;
		[Tooltip("Used to determine if the scrollbars should hide or not")]
		public int ItemsBeforeScroll;

		public bool PageButtonsInList;

		public ExtendedButton NextButton;
		public ExtendedButton PreviousButton;
		#endregion

		#region Private Vars
		bool _isVertical, _isHorizontal;

		int _pageIndex, _lastPage, _itemsOnPage;
		#endregion

		#region UI Methods
		public void NextPage()
		{
			_pageIndex++;
			AdjustPage();
		}

		public void PreviousPage()
		{
			_pageIndex--;
			AdjustPage();
		}
		#endregion

		#region Methods
		public void SetupList(VisualElement prefab, ObservableList<object> data)
		{
			_itemPrefab = prefab;
			SetContext(data);

			_isVertical = Scroll.vertical;
			_isHorizontal = Scroll.horizontal;

			int dif = ItemsPerPage - ListItems.Count;
			if(dif > 0)
			{
				for(int index=0; index < dif; index++)
				{
					AddItem(null);
				}
			}

			_pageIndex = 0;

			//calculate the last page
			_lastPage = Mathf.CeilToInt((float)data.Count / (float)ItemsPerPage) - 1;

			AdjustPage();
			AdjustButtons();
			AdjustBars();
		}

		public void AddItem(object obj)
		{
			DeactivateEmptyItem();

			VisualElement element = (VisualElement)GameObject.Instantiate(_itemPrefab,Vector3.zero,Quaternion.identity);

			(element.transform as RectTransform).SetParent(ContentTransform,false);
			ListItems.Add(element);

			if(PageButtonsInList)
				element.transform.SetSiblingIndex(ListItems.Count - 1);

			element.Init();

			AddListeners(element as ListElement,obj);

			if(_active)
				element.Activate();
			else
				element.PresentVisuals(false);
		}

		void AdjustPage()
		{
			//set to the next dataIndex
			int dataIndex = _pageIndex * ItemsPerPage;

			_itemsOnPage =0;

			//loop through the entire list of elements
			VisualElement element = null;
			for(int index =0; index < ListItems.Count; index++)
			{
				element = ListItems[index];
				//if there is still data
				if(dataIndex < _listContext.Count)
				{
					element.Activate();
					element.SetContext(_listContext[dataIndex]);
					dataIndex++;
					_itemsOnPage++;

					if(PageButtonsInList && !element.gameObject.activeSelf)
						element.gameObject.SetActive(true);
				}
				else
				{
					element.Deactivate();

					if(PageButtonsInList && element.gameObject.activeSelf)
						element.gameObject.SetActive(false);
				}
			}

			if(Scroll.vertical)
				Scroll.verticalNormalizedPosition = 1f;
			if(Scroll.horizontal)
				Scroll.horizontalNormalizedPosition = 1f;

			AdjustButtons();
			AdjustBars();
		}

		void AdjustButtons()
		{
			if(_listContext.Count < ItemsPerPage)
			{
				NextButton.Deactivate();
				PreviousButton.Deactivate();
			}
			else
			{
				NextButton.Activate();
				PreviousButton.Activate();
			}

			//somewhere in the middle so both should be active
			if(_pageIndex >= 0 && _pageIndex <= _lastPage)
			{
				if(PreviousButton)
					PreviousButton.Enable();

				if(NextButton)
					NextButton.Enable();
			}

			if(_pageIndex == 0)
			{
				if(PreviousButton)
					PreviousButton.Disable();
			}

			if(_pageIndex == _lastPage)
			{
				if(NextButton)
					NextButton.Disable();
			}
		}

		void AdjustBars()
		{
			if(_itemsOnPage < ItemsBeforeScroll)
			{
				if(Scroll.verticalScrollbar && Scroll.verticalScrollbar.gameObject.activeSelf)
					Scroll.verticalScrollbar.gameObject.SetActive(false);

				if(Scroll.horizontalScrollbar && !Scroll.horizontalScrollbar.gameObject.activeSelf)
					Scroll.horizontalScrollbar.gameObject.SetActive(false);

				if(_isVertical && Scroll.vertical)
					Scroll.vertical = false;

				if(_isHorizontal && Scroll.horizontal)
					Scroll.horizontal = false;
			}
			else
			{
				if(Scroll.verticalScrollbar && !Scroll.verticalScrollbar.gameObject.activeSelf)
					Scroll.verticalScrollbar.gameObject.SetActive(true);

				if(Scroll.horizontalScrollbar && !Scroll.horizontalScrollbar.gameObject.activeSelf)
					Scroll.horizontalScrollbar.gameObject.SetActive(true);

				if(_isVertical && !Scroll.vertical)
					Scroll.vertical = true;

				if(_isHorizontal && !Scroll.horizontal)
					Scroll.horizontal = true;
			}
		}
		#endregion
	}
}
