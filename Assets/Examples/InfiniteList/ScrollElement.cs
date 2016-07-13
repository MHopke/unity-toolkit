using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using gametheory.UI;

public class ScrollElement : ListElement 
{
	#region Public Vars
	public LayoutElement Layout;
	#endregion

	#region Protected Vars
	bool _hidden;
	#endregion

	#region Unity Methods
	void OnBecameVisible()
	{
		Debug.Log(name + " visible");
	}

	void OnBecameInvisible()
	{
		Debug.Log(name + " invisible");
	}
	#endregion

	#region Methods
	public void SetHidden(bool status)
	{
		_hidden = status;
	}
	#endregion

	#region Properties
	public bool Hidden
	{
		get { return _hidden; }
	}
	#endregion
}
