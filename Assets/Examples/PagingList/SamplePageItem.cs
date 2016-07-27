using UnityEngine;
using UnityEngine.UI;

using gametheory.UI;

public class SamplePageItem : ListElement 
{
	#region Public Vars
	public Text Label;
	#endregion

	#region Overridden Methods
	public override void PresentVisuals (bool display)
	{
		base.PresentVisuals (display);

		if(Label)
			Label.enabled = display;
	}

	protected override void OnContextSet ()
	{
		base.OnContextSet ();

		SamplePageItemData data = _context as SamplePageItemData;

		if(data != null)
			Label.text = data.Name;
	}
	#endregion

}

//sample data
public class SamplePageItemData
{
	#region Public Vars
	public string Name;
	#endregion

	#region Constructors
	public SamplePageItemData(){}
	#endregion
}