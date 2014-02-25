using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Representation of a collection of UIViews. This class controls which UIViews
/// are currently active.
/// </summary>
public class UIViewController : MonoBehaviour 
{
	#region Public Variables	
	//References to current UIViews that are active
	public UIView header = null;
	public UIView content = null;
	public UIView footer = null;

	public List<UIView> screens;
	#endregion

	#region Private Variables
	//Default UIViews to activate when a UIViewController is restored
	//from being the previous controller.
	UIView defaultHeader;
	UIView defaultContent;
	UIView defaultFooter;

	static UIViewController instance = null;
	#endregion

	#region Unity Methods
	void Start()
	{
		defaultHeader = header;
		defaultContent = content;
		defaultFooter = footer;
	}
	#endregion

	#region Activate, Deactivate Methods
	void ActivateUI(UIView view, UIView.Section section)
	{
		view.Activate();
		SetSection(view, section);
	}

	public void Activate(bool useDefaults=false)
	{
		if(useDefaults)
		{
			if(defaultHeader)
				ActivateUI(defaultHeader, header.section);

			if(defaultContent)
				ActivateUI(defaultContent, content.section);

			if(defaultFooter)
				ActivateUI(defaultFooter, footer.section);
		} else
		{
			if(header)
				ActivateUI(header, header.section);

			if(content)
				ActivateUI(content, content.section);

			if(footer)
				ActivateUI(footer, footer.section);
		}

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
	}
	#endregion

	#region Methods
	public static void ChangeView(UIView view)
	{
		instance.TriggerSectionExit(view.section);

		view.Activate();

		instance.SetSection(view, view.section);
	}
	public static void ChangeView(string viewName)
	{
		UIView view = GetUIView(viewName);

		if(view)
		{
			instance.TriggerSectionExit(view.section);

			view.Activate();

			instance.SetSection(view, view.section);
		}
	}
	/// <summary>
	/// Changes the view. Allows for transitions to empty UIViews.
	/// </summary>
	/// <param name="view">View.</param>
	/// <param name="section">Section.</param>
	public static void ChangeView(UIView view, UIView.Section section)
	{
		if(view == GetUIView(section))
			return;

		instance.TriggerSectionExit(section);

		if(view)
			view.Activate();

		instance.SetSection(view, section);
	}
		
	public static UIBase GetElementFromView(string element, string view)
	{
		for(int i = 0; i < instance.screens.Count; i++)
		{
			if(instance.screens[i] && view == instance.screens[i].name)
				return instance.screens[i].RetrieveUIElement(element);
		}

		return null;
	}

	void TriggerSectionExit(UIView.Section section)
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

	public static UIView GetUIView(string name)
	{
		for (int i = 0; i < instance.screens.Count; i++)
		{
			if (instance.screens[i] && instance.screens[i].name == name)
				return instance.screens[i];
		}

		return null;
	}

	public static UIView GetUIView(UIView.Section section)
	{
		if(section == UIView.Section.HEADER)
			return Header;
		else if(section == UIView.Section.CONTENT)
			return Content;
		else if(section == UIView.Section.FOOTER)
			return Footer;
		else
			return null;
	}

	bool HasUIView(string name)
	{
		for (int i = 0; i < screens.Count; i++)
			if (screens[i] && screens[i].name == name)
				return true;

		return false;
	}

	public static void GainFocus()
	{
		if(Header)
			Header.GainedFocus();

		if(Content)
			Content.GainedFocus();

		if(Footer)
			Footer.GainedFocus();
	}

	public static void LoseFocus()
	{
		if(Header)
			Header.LostFocus();

		if(Content)
			Content.LostFocus();

		if(Footer)
			Footer.LostFocus();
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
