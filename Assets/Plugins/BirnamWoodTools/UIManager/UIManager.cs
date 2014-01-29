using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour 
{
	#region Constants
	const float PERCENT_FOR_SCALE = 0.01f;
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
	public UIScreen header = null;
	public UIScreen content = null;
	public UIScreen footer = null;

	public List<UIScreen> screens;
	#endregion

	#region Private Variables
	static UIManager instance = null;
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
			ChangeScreen(header, header.section);

		if(content)
			ChangeScreen(content, content.section);

		if(footer)
			ChangeScreen(footer, footer.section);
	}
	#endregion

	#region Screen Methods
	public static void ActivateScreen(string screen)
	{
		if (screen == "") return;

		UIScreen temp = GetScreenWithName(screen);

		if(temp)
			ChangeScreen(temp, temp.section);
	}

	public static void ChangeScreen(UIScreen toScreen, UIScreen.Section section)
	{
		instance.ExitSection(section);

		if(toScreen)
			toScreen.Activate();

		instance.SetSection(toScreen, section);
	}

	void ExitSection(UIScreen.Section section)
	{
		switch(section)
		{
		case UIScreen.Section.HEADER:
			if(UIManager.Header)
				UIManager.Header.FlagForExit();
			break;
		case UIScreen.Section.CONTENT:
			if(UIManager.Content)
				UIManager.Content.FlagForExit();
			break;
		case UIScreen.Section.FOOTER:
			if(UIManager.Footer)
				UIManager.Footer.FlagForExit();
			break;
		}
	}

	void SetSection(UIScreen screen, UIScreen.Section section)
	{
		switch(section)
		{
		case UIScreen.Section.HEADER:
			UIManager.Header = screen;
			break;
		case UIScreen.Section.CONTENT:
			UIManager.Content = screen;
			break;
		case UIScreen.Section.FOOTER:
			UIManager.Footer = screen;
			break;
		}
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

	public static UIScreen GetScreenWithName(string name)
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

	public static UIScreen Header
	{
		get { return instance.header; }
		set { instance.header = value; }
	}
	public static UIScreen Content
	{
		get { return instance.content; }
		set { instance.content = value; }
	}
	public static UIScreen Footer
	{
		get { return instance.footer; }
		set { instance.footer = value; }
	}
	#endregion
}
