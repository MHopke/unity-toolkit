using UnityEngine;
using System.Collections;
using gametheory.UI;

public class RandomMethods : MonoBehaviour 
{
	public ButtonComponent _blinkButton;

	void Start()
	{
		_blinkButton.clickEvent += BlinkClick;
	}

	void BlinkClick ()
	{
		UIViewController.PresentUIView("UIScrollView");
	}
}
