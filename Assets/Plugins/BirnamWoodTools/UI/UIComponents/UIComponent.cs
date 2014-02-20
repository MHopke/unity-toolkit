using UnityEngine;

public class UIComponent : MonoBehaviour 
{
	#region Protected Variables
	protected UIBase _uiElement;
	#endregion

	#region Methods
	public virtual void Init()
	{
		_uiElement = GetComponent<UIBase>();
	}
	public virtual void Activate(){}
	public virtual void Deactivate(){enabled = false;}
	#endregion
}
