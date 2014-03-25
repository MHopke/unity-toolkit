using UnityEngine;
using System.Collections;

public class RandomMethods : MonoBehaviour 
{
	public Transition transition;
	public UIButton _button;
	public UIButton _scrollButton;
	public UIInputField _input;

	public UIView _screen;

	void Start()
	{
		//_blinkButton.clickEvent += BlinkClick;
		_button.clickEvent += BlinkClick;
		_scrollButton.clickEvent += DoStuff;
		_screen.transitionInEvent += HandletransitionInEvent;
		_input._textChanged += Handle_textChanged;
		//UINavigationController.CurrentController.GetUIView("UIScreen").Activate(transition);
	}

	void Handle_textChanged (string obj)
	{
		Debug.Log(obj);
	}

	void HandletransitionInEvent ()
	{
		UIViewController.RemoveUIView("UIScreen");
	}

	void DoStuff()
	{
		UIViewController.PresentUIView("UIScreen");
		UIViewController.RemoveUIViewWithTransition("AUIScrollView",new Transition(new Vector2(-400,0),1f));
	}

	void HandleclickEvent ()
	{
		UIViewController.PresentUIView("UIScreen");
		UIViewController.RemoveUIViewWithTransition("AUIScrollView",new Transition(new Vector2(-400,0),1f));
		/*_screen.enabled = false;
		_screen.enabled = true;*/
	}

	void BlinkClick ()
	{
		UIViewController.PresentUIViewWithTransition("AUIScrollView",transition);
		//UIViewController.RemoveUIView("UIScreen");
		//UIViewController.PresentUIView("UIScrollView");
	}

	/*void OnGUI()
	{
		GUI.DrawTextureWithTexCoords(new Rect(0, 0, 128, 128),  _texture.texture,_texture.textureRect);
	}*/
}
