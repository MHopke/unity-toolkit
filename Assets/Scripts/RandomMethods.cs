using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using gametheory.UI;

public class RandomMethods : MonoBehaviour 
{
	public ButtonComponent _blinkButton;

    public Button _button;
    public Text _text;

	void Start()
	{
		//_blinkButton.clickEvent += BlinkClick;
	}

	void BlinkClick ()
	{
		UIViewController.PresentUIView("UIScrollView");
	}

    public void Clicked()
    {
        _button.gameObject.SetActive(false);
        _text.gameObject.SetActive(true);
        Debug.Log("clicked");
    }
}
