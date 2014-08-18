using UnityEngine;
using System.Collections.Generic;

namespace gametheory.UI
{
    public class GUIDropDown : GUIBase 
    {
    	#region Public Vars
    	public int _maxItems = 4;

    	public string[] _choiceStrings;
    	#endregion

    	#region Private Vars
    	bool _choicesDisplayed;

    	int _loopIndex;
    	int _choiceIndex;

    	string _choice;

    	Vector2 _scrollPos;

    	Rect _scrollArea;
    	Rect _defaultRect;

    	List<Rect> _choiceRects;
    	#endregion

    	#region Overridden Methods
    	protected override void OnInit()
    	{
    		base.OnInit();

    		_choiceRects = new List<Rect>();

    		_defaultRect = new Rect(_drawRect.x,_drawRect.y,_drawRect.width,_drawRect.height / _maxItems);

    		//_drawRect.y = _defaultRect.yMax;

    		_startPosition.y = _defaultRect.yMax;

    		SetUpRects();

    		//Debug.Log(_defaultRect.y + " " + _drawRect.y);
    	}

    	public override void Draw()
    	{
    		if(_choicesDisplayed)
    		{
    			_scrollPos = GUI.BeginScrollView(_drawRect, _scrollPos, _scrollArea);

    			for(_loopIndex = 0; _loopIndex < _choiceRects.Count; _loopIndex++)
    			{
    				if(GUI.Button(_choiceRects[_loopIndex], _choiceStrings[_loopIndex],(_loopIndex % 2 == 0) ? "EvenItem" : "OddItem"))
    				{
    					_choicesDisplayed = false;
    					_choice = _choiceStrings[_loopIndex];
    					_choiceIndex = _loopIndex;

    					_scrollPos = _choiceRects[_loopIndex].min;

    					UIViewController.GainFocus();
    					break;
    				}
    			}

    			GUI.EndScrollView();

    			if(GUI.Button(_defaultRect, _choice, "SelectionStyle"))
    			{
    				_choicesDisplayed = false;
    				UIViewController.GainFocus();
    			}
    		}
    		else if(GUI.Button(_defaultRect, _choice, "SelectionStyle"))
    		{
    			_choicesDisplayed = true;
    			UIViewController.LoseFocus();
    		}
    	}
    	#endregion

    	#region Other Methods
    	public void ChangeInfo(string[] info)
    	{
    		/*for(int i = 0; i < _choiceRects.Count; i++)
    		{
    			Destroy(_choiceRects[i]);
    		}*/

    		_choiceRects.Clear();

    		_choiceStrings = info;

    		SetUpRects();
    	}
    	void SetUpRects()
    	{
    		Rect baseRect = new Rect(0, 0, _drawRect.width, _defaultRect.height);

    		for(int i = 0; i < _choiceStrings.Length; i++)
    		{
    			_choiceRects.Add(baseRect);
    			baseRect.y = baseRect.yMax;
    		}

    		_choice = _choiceStrings[0];

    		_scrollArea = new Rect(0, 0, _drawRect.width * 0.93f, baseRect.y);

    		_scrollPos = Vector2.zero;
    	}
    	#endregion

    	#region Accessors
    	public string Choice
    	{
    		get { return _choice; }
    	}
    	public int ChoiceIndex
    	{
    		get { return _choiceIndex; }
    	}
    	#endregion
    }
}
