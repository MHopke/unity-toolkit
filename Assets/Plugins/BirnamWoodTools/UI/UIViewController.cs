using UnityEngine;
using System.Collections.Generic;

public class UIViewController : MonoBehaviour 
{
	#region Public Variables
	public GUISkin guiSkin;
	
	//Reference to the current screen that is active
	public UIView header = null;
	public UIView content = null;
	public UIView footer = null;

	public List<UIView> screens;
	#endregion

	#region Private Variables
	static UIViewController instance = null;
	#endregion

	#region Unity Methods
	// Use this for initialization
	void Start () 
	{
		Activate();

		#if !UNITY_EDITOR
		if(Skin != null)
		{
			for(int i = 0; i < Skin.customStyles.Length; i++)
				Skin.customStyles[i].fontSize = Mathf.RoundToInt(UINavigationController.AspectRatio.x * (float)Skin.customStyles[i].fontSize);

			//Skin.label.fontSize = Mathf.RoundToInt((UINavigationController.AspectRatio.x * (float)Skin.label.fontSize));
		}
		#endif
	}
	#endregion

	#region Activate, Deactivate Methods
	public void Activate()
	{
		if(header)
			ActivateInitialUI(header, header.section);

		if(content)
			ActivateInitialUI(content, content.section);

		if(footer)
			ActivateInitialUI(footer, footer.section);

		enabled = true;

		instance = this;
	}

	public void Deactivate()
	{
		if(header)
			header.FlagForExit();

		if(content)
			content.FlagForExit();

		if(footer)
			footer.FlagForExit();

		enabled = false;
	}
	#endregion

	#region Control Methods
	public static void ChangeScreen(UIView view, UIView.Section section)
	{
		instance.ExitSection(section);

		if(view)
			view.Activate();

		instance.SetSection(view, section);
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

	void ActivateInitialUI(UIView view, UIView.Section section)
	{
		if(view)
		{
			view.Activate();
			SetSection(view, section);
		}
	}

	void ExitSection(UIView.Section section)
	{
		switch(section)
		{
		case UIView.Section.HEADER:
			if(UIViewController.Header)
				UIViewController.Header.FlagForExit();
			break;
		case UIView.Section.CONTENT:
			if(UIViewController.Content)
				UIViewController.Content.FlagForExit();
			break;
		case UIView.Section.FOOTER:
			if(UIViewController.Footer)
				UIViewController.Footer.FlagForExit();
			break;
		}
	}

	void SetSection(UIView screen, UIView.Section section)
	{
		switch(section)
		{
		case UIView.Section.HEADER:
			header = screen;
			break;
		case UIView.Section.CONTENT:
			content = screen;
			break;
		case UIView.Section.FOOTER:
			footer = screen;
			break;
		}
	}

	public UIView GetScreenWithName(string name)
	{
		for (int i = 0; i < screens.Count; i++)
		{
			if (screens[i] && screens[i].name == name)
				return screens[i];
		}

		return null;
	}

	bool HasUIView(string name)
	{
		for (int i = 0; i < screens.Count; i++)
			if (screens[i] && screens[i].name == name)
				return true;

		return false;
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
