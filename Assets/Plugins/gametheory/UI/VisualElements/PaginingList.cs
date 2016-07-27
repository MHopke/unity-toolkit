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

		public ExtendedButton NextButton;
		public ExtendedButton PreviousButton;
		#endregion

		#region Private Vars
		int _pageIndex, _lastPage;
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
			}

			_pageIndex = 0;

			//calculate the last page
			_lastPage = Mathf.CeilToInt((float)data.Count / (float)ItemsPerPage) - 1;

			AdjustButtons();
		}

		void AdjustPage()
		{
			//set to the next dataIndex
			int dataIndex = _pageIndex * ItemsPerPage;

			//loop through the entire list of elements
			for(int index =0; index < ListItems.Count; index++)
			{
				//if there is still data
				if(dataIndex < _listContext.Count)
				{
					(ListItems[index]).SetContext(_listContext[dataIndex]);
					dataIndex++;
				}
				else
					ListItems[index].Deactivate();
			}

			AdjustButtons();
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
		#endregion
	}
}
