using UnityEngine;
using System.Collections;

namespace gametheory.UI
{
	/// <summary>
	/// Sub-class of UIView that should be used for any views that are presented "over" 
	/// the app's primary content; for example: an input window, or confirmation prompt.
	/// </summary>
	public class UIAlert : UIView 
	{
		#region Public Vars
		[Tooltip("Whether or not the view hides " +
			"Typically used for alert stacking.")]
		public bool Hides=true;
		#endregion

		#region Methods
		public void Hide()
		{
			//gameObject.SetActive(false);
			//LostFocus();
			if(Hides)
				OnHide();
		}
		public void Show()
		{
			//gameObject.SetActive(true);
			//GainedFocus();
			OnShow();
		}
		#endregion

		#region Virtual Methods
		protected virtual void OnHide()
		{
			if(Elements != null)
			{
				VisualElement element = null;
				for(int i = 0; i < Elements.Count; i++)
				{
					element = Elements[i];
					if (element)
					{
						element.Hide();
					}
				}
			}
		}
		protected virtual void OnShow()
		{
			if(Elements != null)
			{
				VisualElement element = null;
				for(int i = 0; i < Elements.Count; i++)
				{
					element = Elements[i];
					if (element)
					{
						element.Show();
					}
				}
			}
		}
		#endregion
	}
}
