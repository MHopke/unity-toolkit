using UnityEngine;
using UnityEngine.UI;
using gametheory.UI;
using System.Collections;

/// <summary>
/// A premade component that allows the user to adjust the key/value pairs
/// that will be stored in the localization xml. Utilizes gametheory's UI
/// system. 
/// </summary>
public class KeyValueUI : ExtendedInputField
{
	#region Private Variables
	private event System.Action<string, string, string> onSubmitData;
	private string _className;
	#endregion

	#region Methods
    public void Initialize(string className, string label,string text, System.Action<string, string, string> submitEvent)
	{
		Label.text = label;
        Text = text;
		_className = className;
		onSubmitData += submitEvent;
	}
	#endregion

	#region Overridden Methods
	protected override void OnDeactivate()
	{
		base.OnDeactivate();

		onSubmitData = null;
	}

	protected override void OnSubmitData(string value)
	{
		if(onSubmitData != null)
			onSubmitData(_className, Label.text, value);
	}
	#endregion
}
