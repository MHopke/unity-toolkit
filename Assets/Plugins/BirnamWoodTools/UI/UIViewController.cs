using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class represents a collection of UIViews. Only one
/// UIViewController can be active at a time.
/// </summary>
public class UIViewController : MonoBehaviour 
{
	#region Public Variables
	public List<UIView> screens;
	#endregion

	#region Private Variables
	#endregion

	#region Unity Methods
	void Awake()
	{
		defaultHeader = header;
		defaultContent = content;
		defaultFooter = footer;
	}
	#endregion

	#region Activate, Deactivate Methods
	public void Activate(bool useDefaults=false)
	{
		DirectViewChange(defaultHeader, UIView.Section.HEADER);

		DirectViewChange(defaultContent, UIView.Section.CONTENT);

		DirectViewChange(defaultFooter, UIView.Section.FOOTER);

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
	public void ChangeView(UIView view)
	{
		instance.TriggerSectionExit(view.section);

		view.Activate();

		instance.SetSection(view, view.section);
	}
	public void ChangeView(string viewName)
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
	public void ChangeView(UIView view, UIView.Section section)
	{
		if(view == GetUIView(section))
			return;

		instance.TriggerSectionExit(section);

		if(view)
			view.Activate();

		instance.SetSection(view, section);
	}

	void DirectViewChange(UIView view, UIView.Section section)
	{
		if(view)
			view.Activate();

		SetSection(view, section);
	}
		
	public UIBase GetElementFromView(string element, string view)
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
			if(header)
				header.FlagForExit();
			break;
		case UIView.Section.CONTENT:
			if(content)
				content.FlagForExit();
			break;
		case UIView.Section.FOOTER:
			if(footer)
				footer.FlagForExit();
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

	public UIView GetUIView(string name)
	{
		for (int i = 0; i < instance.screens.Count; i++)
		{
			if (instance.screens[i] && instance.screens[i].name == name)
				return instance.screens[i];
		}

		return null;
	}

	public UIView GetUIView(UIView.Section section)
	{
		if(section == UIView.Section.HEADER)
			return header;
		else if(section == UIView.Section.CONTENT)
			return content;
		else if(section == UIView.Section.FOOTER)
			return footer;
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

	public void GainFocus()
	{
		if(header)
			header.GainedFocus();

		if(content)
			content.GainedFocus();

		if(footer)
			footer.GainedFocus();
	}

	public void LoseFocus()
	{
		if(header)
			header.LostFocus();

		if(content)
			content.LostFocus();

		if(footer)
			footer.LostFocus();
	}
	#endregion

	#region Audio Methods
	public void PlayButtonAudio(AudioClip clip=null)
	{
		if(audio && !audio.isPlaying)
		{
			if(clip == null)
				audio.Play();
			else
				audio.PlayOneShot(clip);
		}
	}
	#endregion
}
