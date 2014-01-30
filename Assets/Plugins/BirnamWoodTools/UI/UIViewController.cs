using UnityEngine;
using System.Collections.Generic;

public class UIViewController : MonoBehaviour 
{
	#region Constants
	const float PERCENT_FOR_SCALE = 0.01f;
	const string SCREEN_PATH = "Screens/";
	#endregion

	#region Public Variables
	//Hold the x and y resolutions that the screens were
	//originally designed at. 
	public Vector2 DESIGNED_RESOLUTION;

	//This is used to rescale graphics if the resolution
	//is different than the Designed Resolution
	public static Vector2 AspectRatio;

	public GUISkin guiSkin;
	
	//Reference to the current screen that is active
	public UIView header = null;
	public UIView content = null;
	public UIView footer = null;

	public List<UIView> screens;
	#endregion

	#region Private Variables
	UIView previousHeader;
	UIView previousContent;
	UIView previousFooter;
	static UIViewController instance = null;
	#endregion

	#region Unity Methods
	void Awake()
	{
		if(AspectRatio == Vector2.zero)
			AspectRatio = new Vector2((float)Screen.width / DESIGNED_RESOLUTION.x,
				(float)Screen.height / DESIGNED_RESOLUTION.y);

		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		if(header)
			ActivateInitialUI(header, header.section);

		if(content)
			ActivateInitialUI(content, content.section);

		if(footer)
			ActivateInitialUI(footer, footer.section);
	}
	#endregion

	#region Screen Methods
	void ActivateInitialUI(UIView view, UIView.Section section)
	{
		if(view)
		{
			view.Activate();
			instance.SetSection(view, section);
		}
	}

	public static void ChangeScreen(string screen, UIView.Section section)
	{
		instance.ExitSection(section);

		UIView toScreen = instance.GetView(screen,section);

		if(toScreen)
			toScreen.Activate();

		instance.SetSection(toScreen, section);
	}

	void ExitSection(UIView.Section section)
	{
		switch(section)
		{
		case UIView.Section.HEADER:
			if(UIViewController.Header)
			{
				if(instance.previousHeader)
					Destroy(instance.previousHeader);

				instance.previousHeader = UIViewController.Header;

				UIViewController.Header.FlagForExit();
			}
			break;
		case UIView.Section.CONTENT:
			if(UIViewController.Content)
			{
				if(instance.previousContent)
					Destroy(instance.previousContent);
				instance.previousContent = UIViewController.Content;

				UIViewController.Content.FlagForExit();
			}
			break;
		case UIView.Section.FOOTER:
			if(UIViewController.Footer)
			{
				if(instance.previousFooter)
					Destroy(instance.previousFooter);
				instance.previousFooter = UIViewController.Footer;

				UIViewController.Footer.FlagForExit();
			}
			break;
		}
	}

	void SetSection(UIView screen, UIView.Section section)
	{
		switch(section)
		{
		case UIView.Section.HEADER:
			UIViewController.Header = screen;
			break;
		case UIView.Section.CONTENT:
			UIViewController.Content = screen;
			break;
		case UIView.Section.FOOTER:
			UIViewController.Footer = screen;
			break;
		}
	}

	UIView GetView(string viewName, UIView.Section section)
	{
		switch(section)
		{
		case UIView.Section.HEADER:
			if(previousHeader.name == viewName)
				return previousHeader;
			else
				return LoadNewView(viewName);
		case UIView.Section.CONTENT:
			if(previousContent.name == viewName)
				return previousContent;
			else
				return LoadNewView(viewName);
		case UIView.Section.FOOTER:
			if(previousFooter.name == viewName)
				return previousFooter;
			else
				return LoadNewView(viewName);
		}

		return null;
	}

	UIView LoadNewView(string viewName)
	{
		if(viewName == "")
			return null;

		GameObject obj = (GameObject)Resources.Load(SCREEN_PATH + viewName);

		return obj.GetComponent<UIView>();
	}

	public static void EnableButtons()
	{
		if(Header)
			Header.EnableButtons();

		if(Content)
			Content.EnableButtons();

		if(Footer)
			Footer.EnableButtons();
	}

	public static void DisableButtons()
	{
		if(Header)
			Header.DisableButtons();

		if(Content)
			Content.DisableButtons();

		if(Footer)
			Footer.DisableButtons();
	}

	/*public static void AddElementToScreen(BaseDrawable drawable, string screen)
	{
		UIScreen temp = GetScreenWithName(screen);

		if(temp != null)
			temp.AddExternalElement(drawable);
	}

	public static BaseDrawable GetElementFromScreen(string screen, string elementName)
	{
		UIScreen temp = GetScreenWithName(screen);

		if(temp != null)
			return temp.GetElement(elementName);
		else
			return null;
	}*/

	public static UIView GetScreenWithName(string name)
	{
		for (int i = 0; i < instance.screens.Count; i++)
		{
			if (instance.screens[i] && instance.screens[i].name == name)
				return instance.screens[i];
		}

		return null;
	}
	#endregion

	#region Scale Methods
	/// <summary>
	/// Determines if the UI should be scaled.
	/// </summary>
	/// <returns><c>true</c>, if the Aspect Ratio is greater than the required amount <c>false</c> otherwise.</returns>
	public static bool ShouldScaleDimensions()
	{
		return !(Mathf.Abs(1.0f - AspectRatio.x) < PERCENT_FOR_SCALE || Mathf.Abs(1.0f - AspectRatio.y) < PERCENT_FOR_SCALE);
	}
	#endregion

	#region Audio Methods
	public static void PlayButtonAudio(AudioClip clip=null)
	{
		if(instance.audio && !instance.audio.isPlaying)
		{
			if(clip == null)
				instance.audio.Play();
			else
				instance.audio.PlayOneShot(clip);
		}
	}
	#endregion

	#region Accessors
	public static GUISkin Skin
	{
		get { return instance.guiSkin; }
	}

	public static UIView Header
	{
		get { return instance.header; }
		set { instance.header = value; }
	}
	public static UIView Content
	{
		get { return instance.content; }
		set { instance.content = value; }
	}
	public static UIView Footer
	{
		get { return instance.footer; }
		set { instance.footer = value; }
	}
	#endregion
}
