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

		public ExtendedButton NextButton;
		public ExtendedButton PreviousButton;
		#endregion

		#region Private Vars
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

			//set up initial page
			int min = Mathf.Min(ItemsPerPage,data.Count);
			for(int index=0; index < min; index++)
			{
				AddItem(data[index]);
				_itemsOnPage++;
			}

			_pageIndex = 0;

			//calculate the last page
			_lastPage = Mathf.CeilToInt((float)data.Count / (float)ItemsPerPage) - 1;

			AdjustButtons();
			AdjustBars();
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
				}
				else
					element.Deactivate();
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
			//somewhere in the middle so both should be active
			if(_pageIndex >= 0 && _pageIndex <= _lastPage)
			{
				PreviousButton.Enable();
				NextButton.Enable();
			}

			if(_pageIndex == 0)
				PreviousButton.Disable();

			if(_pageIndex == _lastPage)
				NextButton.Disable();
		}

		void AdjustBars()
		{
			if(Scroll.verticalScrollbar)
			{
				if(Scroll.verticalScrollbar.gameObject.activeSelf && _itemsOnPage < ItemsBeforeScroll)
					Scroll.verticalScrollbar.gameObject.SetActive(false);
				else if(!Scroll.verticalScrollbar.gameObject.activeSelf && _itemsOnPage >= ItemsBeforeScroll)
					Scroll.verticalScrollbar.gameObject.SetActive(true);
			}

			if(Scroll.horizontalScrollbar)
			{
				if(Scroll.horizontalScrollbar.gameObject.activeSelf && _itemsOnPage < ItemsBeforeScroll)
					Scroll.horizontalScrollbar.gameObject.SetActive(false);
				else if(!Scroll.horizontalScrollbar.gameObject.activeSelf && _itemsOnPage >= ItemsBeforeScroll)
					Scroll.horizontalScrollbar.gameObject.SetActive(true);
			}
		}
		#endregion
	}
}
