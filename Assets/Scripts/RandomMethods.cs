using UnityEngine;
using System.Collections;

public class RandomMethods : MonoBehaviour 
{
	public Transition transition;
	public UIButton _button;
	public UIButton _scrollButton;
	public UIInputField _input;

	public UIView _screen;

	public Font _font;
	public Renderer _render;

	public string _text;

	void Start()
	{
		//_blinkButton.clickEvent += BlinkClick;
		_button.clickEvent += BlinkClick;
		_scrollButton.clickEvent += DoStuff;
		_screen.transitionInEvent += HandletransitionInEvent;
		_input._textChanged += Handle_textChanged;

		_font.RequestCharactersInTexture("_text");
		_render.material.mainTexture = _font.material.mainTexture;

		CharacterInfo _cInfo = new CharacterInfo();

		float xPos = 1.0f;

		for(int i = 0; i < _text.Length; i++)
		{
			if(_font.GetCharacterInfo(_text[i], out _cInfo))
			{
				GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Quad);

				obj.transform.SetXPosition(xPos);
				xPos += 1.0f;

				obj.name = _text[i].ToString();

				obj.renderer.material = _render.material;

				if(_cInfo.flipped)
				{
					obj.transform.Rotate(0, 0, 90);
					obj.transform.ScaleY(-1f);
				}

				Rect uv = _cInfo.uv;

				obj.renderer.material.mainTextureOffset = new Vector2(uv.x, uv.y);

				//scale is actually a textures "Tiling" variable
				obj.renderer.material.mainTextureScale = new Vector2(uv.width, uv.height);
				obj.transform.Scale(Mathf.Abs(uv.width), Mathf.Abs(uv.height), 1);
			}
		}
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
