using UnityEngine;
using System.Collections;

using gametheory;
using gametheory.UI;

public class PagingView : UIView 
{
	#region Public Vars
	public int NumberOfItems;
	public SamplePageItem ItemPrefab;
	public PaginingList PageList;
	#endregion

	#region Overridden Methods
	protected override void OnActivate ()
	{
		base.OnActivate ();

		ObservableList<object> data = new ObservableList<object>();
		for(int index =0; index < NumberOfItems; index++)
			data.Add(new SamplePageItemData() { Name = "item: " + index });

		PageList.SetupList(ItemPrefab,data);
	}
	#endregion
}
