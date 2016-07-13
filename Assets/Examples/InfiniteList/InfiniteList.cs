using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using gametheory.UI;

public class InfiniteList : MonoBehaviour 
{
	#region Constants
	const int EXTRA = 1;
	#endregion

	#region Public Vars
	public ScrollRect Scroll;
	public ScrollElement Prefab;
	#endregion

	#region Private Vars
	static bool _adjusting;
	bool _vertical;
	bool _positiveVel;

	float _scrollPerItem;
	float _containerSize;
	float _itemSize;
	float _lastAmount;
	float _amountChanged;

	float _min, _max;

	List<ScrollElement> _elements;
	#endregion

	#region Unity Methods
	void Start()
	{
		_vertical = Scroll.vertical;

		_elements = new List<ScrollElement>();
		StartCoroutine(InitWait());
	}
	#endregion

	#region UI Methods
	public void ScrollChanged(Vector2 pos)
	{
		if(_adjusting || Scroll.velocity.y == 0f)
			return;

		if(_vertical)
			_positiveVel = Scroll.velocity.y > 0f;
		else
			_positiveVel = Scroll.velocity.x > 0f;

		bool contained = false;
		ScrollElement element = null;
		for(int index =0; index < _elements.Count; index++)
		{
			element = _elements[index];
			Vector3[] arr = new Vector3[4];
			//indexs = bottom left, top left, top right, bottom right
			(element.transform as RectTransform).GetWorldCorners(arr);
			//TODO implement correct side checks

			Vector2 screenPos = new Vector2((arr[0].x + arr[2].x)/2f,
				(_positiveVel) ? arr[1].y : arr[0].y);
			if(_vertical)
			{
				contained = (screenPos.y >= _min && screenPos.y <= _max);
			}
			else
				contained = (screenPos.x >= _min && screenPos.x <= _max);
			
			//if(index == 0)
			//{
			//Debug.Log("element - " + index);
				//for(int sub =0; sub < arr.Length; sub++)
				//{
					//Debug.Log(arr[sub]);
				//}
				//Debug.Log(index + " " + screenPos + " " + contained + " " + _min + " "+ _max);
			//}

			if(contained && element.Hidden)
			{
				element.SetHidden(false);
			}
			else if(!contained && !element.Hidden)
			{
				//Scroll.enabled = false;
				_adjusting = true;
				//Debug.ClearDeveloperConsole();
				Debug.Log("set to last: " + element.name + " " + screenPos.y);

				//Debug.Log("before: " + Scroll.verticalNormalizedPosition);

				element.SetHidden(true);
				element.transform.SetAsLastSibling();

				//Debug.Log("after: " + Scroll.verticalNormalizedPosition);

				StartCoroutine(WaitToReset());
				//_adjusting = false;
				return;
			}
		}
	}
	/*public void ScrollChanged(float value)
	{
		_amountChanged = value - _lastAmount;
		Debug.Log(_amountChanged + " needed: " + _scrollPerItem);
		if(_amountChanged > 0f)
		{
			while(_amountChanged >= _scrollPerItem)
			{
				_amountChanged -= _scrollPerItem;
				Debug.Log("index increased");

				_lastAmount = value;
			}
		}
		else if( _amountChanged < 0f)
		{
			while(_amountChanged <= -_scrollPerItem)
			{
				_amountChanged += _scrollPerItem;
				Debug.Log("index decreased");

				_lastAmount = value;
			}
		}
	}*/
	#endregion

	#region Coroutines
	IEnumerator InitWait()
	{
		yield return new WaitForEndOfFrame();

		Vector3[] arr = new Vector3[4];
		//indexs = bottom left, top left, top right, bottom right
		Scroll.viewport.GetWorldCorners(arr);

		for(int index =0; index < arr.Length; index++)
		{
			//Debug.Log(arr[index]);
		}
		if(_vertical)
		{
			_min = arr[1].y;
			_max = arr[0].y;
		}
		else
		{
			_min = arr[0].x;
			_max = arr[2].x;
		}

		int numItems = EXTRA;

		if(_vertical)
		{
			_containerSize = Scroll.viewport.rect.height;
			_itemSize = Prefab.Layout.minHeight;
		}
		else
		{
			_containerSize = Scroll.viewport.rect.width;
			_itemSize = Prefab.Layout.minWidth;
		}

		numItems += Mathf.RoundToInt(_containerSize / _itemSize);

		for(int index =0; index < numItems; index++)
		{
			ScrollElement el = (ScrollElement)Instantiate(Prefab,Vector3.zero,Quaternion.identity);
			el.SetParent(Scroll.content);
			el.name = index.ToString();
			_elements.Add(el);
		}

		_elements[_elements.Count - EXTRA].SetHidden(true);

		_scrollPerItem = 1f / numItems;

		//set so the bar can scroll for all of the items
		Scroll.verticalScrollbar.size = _scrollPerItem * (numItems - EXTRA);

		//set last amount
		_lastAmount = Scroll.verticalScrollbar.value;
	}

	IEnumerator WaitToReset()
	{
		Scroll.verticalNormalizedPosition += (_itemSize / _containerSize);

		yield return new WaitForSeconds(2);

		_adjusting = false;
	}
	#endregion
}
