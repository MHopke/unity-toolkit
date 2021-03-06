﻿using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace gametheory.UI
{
	public class PagingList : UIList 
	{
        #region Public Vars
        public bool PageButtonsInList;
        [Tooltip("Causes the next/previous buttons to be deactivated rather than disabled")]
        public bool DeactivateButtons;

        public int ItemsPerPage;
        [Tooltip("Used to determine if the scrollbars should hide or not")]
        public int ItemsBeforeScroll;

        //public string PageCount

        public ExtendedText PageCount;
        public ExtendedButton NextButton;
        public ExtendedButton PreviousButton;
        //public bool Deactivate 
        #endregion

        #region Private Vars
        bool _isVertical, _isHorizontal;

        int _pageIndex, _lastPage, _itemsOnPage, _dataCount;
        #endregion

        #region Overridden Methods
        protected override void OnActivate()
        {
            base.OnActivate();
            AdjustButtons();
        }
        protected override void OnInit()
        {
            base.OnInit();

            if (Scroll)
            {
                _isVertical = Scroll.vertical;
                _isHorizontal = Scroll.horizontal;
            }
        }
        public override void SetupList(VisualElement prefab, IEnumerable data, bool setupContent=true)
        {
            /*_itemPrefab = prefab;
            SetContext(data);

            int dif = ItemsPerPage - ListItems.Count;
            if (dif > 0)
            {
                for (int index = 0; index < dif; index++)
                {
                    AddItem(null);
                }
            }*/
            base.SetupList(prefab, data,false);

            _pageIndex = 0;

            //calculate the last page

            if (data != null)
                _dataCount = data.Count();
            else
                _dataCount = 0;

            _lastPage = Mathf.CeilToInt((float)_dataCount / (float)ItemsPerPage) - 1;

            if (PageCount != null)
            {
                if (_lastPage > _pageIndex)
                    PageCount.Activate();
                else
                    PageCount.Deactivate();
            }

            AdjustButtons();

            SetupContent();
        }
        protected override int GetItemCount(IEnumerable data)
        {
            return ItemsPerPage - ListItems.Count;//base.GetItemCount(data);
        }

        public override void AddItem(object obj)
        {
            DeactivateEmptyItem();

            VisualElement element = (VisualElement)GameObject.Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);

            (element.transform as RectTransform).SetParent(ContentTransform, false);
            ListItems.Add(element);

            if (PageButtonsInList)
                element.transform.SetSiblingIndex(ListItems.Count - 1);

            element.Init();

            AddListeners(element as ListElement);

            if (_active)
                element.Activate();
            else
                element.PresentVisuals(false);
        }
        protected override void SetupContent()
        {
            //set to the next dataIndex
            int dataIndex = _pageIndex * ItemsPerPage;
            int index = 0, elementIndex = 0;

            _itemsOnPage = 0;

            VisualElement element = null;
            if (_listContext != null)
            {
                //loop through the entire list of elements
                foreach (var item in _listContext)
                {
                    index++;
                    if (index > dataIndex)
                    {
                        if (dataIndex < _dataCount)
                        {
                            element = ListItems[elementIndex];

                            if (!element.gameObject.activeSelf)
                                element.gameObject.SetActive(true);

                            element.SetContext(item);

                            if (_active)
                                element.Activate();

                            dataIndex++;
                            _itemsOnPage++;
                            elementIndex++;

                            //cut out if we've reached the max page items
                            if (_itemsOnPage >= ItemsPerPage)
                                break;
                        }
                    }
                }
            }

            for (index = elementIndex; index < ListItems.Count; index++)
            {
                element = ListItems[index];
                element.Deactivate();

                if (element.gameObject.activeSelf)
                    element.gameObject.SetActive(false);
            }

            ResetScrollPos();

            AdjustButtons();
            AdjustBars();
        }
        #endregion

        #region UI Methods
        public void NextPage()
        {
            _pageIndex++;
            SetupContent();
        }

        public void PreviousPage()
        {
            _pageIndex--;
            SetupContent();
        }
        #endregion

        #region Methods
        void AdjustButtons()
        {
            if (!_active)
                return;

            //Debug.Log(_dataCount + " " + ItemsPerPage);
            if (_dataCount < ItemsPerPage)
            {
                if (NextButton)
                    NextButton.Deactivate();

                if (PreviousButton)
                    PreviousButton.Deactivate();
            }
            else
            {
                if (NextButton)
                    NextButton.Activate();
                if (PreviousButton)
                    PreviousButton.Activate();
            }

            //somewhere in the middle so both should be active
            if (_pageIndex >= 0 && _pageIndex <= _lastPage)
            {
                if (PreviousButton)
                {
                    if (DeactivateButtons)
                        PreviousButton.Activate();
                    else
                        PreviousButton.Enable();
                }

                if (NextButton)
                {
                    if (DeactivateButtons)
                        NextButton.Activate();
                    else
                        NextButton.Enable();
                }
            }

            if (_pageIndex == 0)
            {
                if (PreviousButton)
                {
                    if (DeactivateButtons)
                        PreviousButton.Deactivate();
                    else
                        PreviousButton.Disable();
                }
            }

            if (_pageIndex == _lastPage)
            {
                if (NextButton)
                {
                    if (DeactivateButtons)
                        NextButton.Deactivate();
                    else
                        NextButton.Disable();
                }
            }

            if (PageCount != null && PageCount.Active)
                PageCount.Text = (_pageIndex + 1) + "/" + (_lastPage + 1);
        }

        void AdjustBars()
        {
            if (_itemsOnPage < ItemsBeforeScroll)
            {
                /*if(Scroll.verticalScrollbar && Scroll.verticalScrollbar.gameObject.activeSelf)
					Scroll.verticalScrollbar.gameObject.SetActive(false);

				if(Scroll.horizontalScrollbar && !Scroll.horizontalScrollbar.gameObject.activeSelf)
					Scroll.horizontalScrollbar.gameObject.SetActive(false);*/

                if (_isVertical && Scroll.vertical)
                    Scroll.vertical = false;

                if (_isHorizontal && Scroll.horizontal)
                    Scroll.horizontal = false;
            }
            else
            {
                /*if(Scroll.verticalScrollbar && !Scroll.verticalScrollbar.gameObject.activeSelf)
					Scroll.verticalScrollbar.gameObject.SetActive(true);

				if(Scroll.horizontalScrollbar && !Scroll.horizontalScrollbar.gameObject.activeSelf)
					Scroll.horizontalScrollbar.gameObject.SetActive(true);*/

                if (_isVertical && !Scroll.vertical)
                    Scroll.vertical = true;

                if (_isHorizontal && !Scroll.horizontal)
                    Scroll.horizontal = true;
            }
        }
        #endregion
    }
}
