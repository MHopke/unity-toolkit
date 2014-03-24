using UnityEngine;
using System.Collections;

public class RandomMethods : MonoBehaviour 
{
	public Sprite _texture;

	void Start()
	{
		//_blinkButton.clickEvent += BlinkClick;
	}

	void BlinkClick ()
	{
		UIViewController.PresentUIView("UIScrollView");
	}

	/*void OnGUI()
	{
		GUI.DrawTextureWithTexCoords(new Rect(0, 0, 128, 128),  _texture.texture,_texture.textureRect);
	}*/
}
