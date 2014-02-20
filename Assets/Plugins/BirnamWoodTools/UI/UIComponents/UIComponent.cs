using UnityEngine;

/// <summary>
/// User interface component.
/// </summary>
public class UIComponent : MonoBehaviour 
{
	#region Protected Variables
	protected UIBase _uiElement;
	#endregion

	#region Methods
	public virtual void Init(UIBase element){_uiElement = element;}
	public virtual void Activate(){}
	public virtual void Deactivate(){enabled = false;}
	#endregion
}
