using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace gametheory.UI
{
	public class UIGroup : VisualElement 
	{
		#region Public Vars
		public List<VisualElement> Items;
		#endregion

		#region Overridden Methods
		protected override void Disabled()
		{
			base.Disabled();

			for(int index = 0; index < Items.Count; index++)
				Items[index].Disable();
		}
		protected override void Enabled()
		{
			base.Enabled();

			for(int index = 0; index < Items.Count; index++)
				Items[index].Enable();
		}
		public override void OnGainedFocus()
		{
			base.OnGainedFocus();
			for(int index = 0; index < Items.Count; index++)
				Items[index].GainedFocus();
		}
		public override void OnLostFocus()
		{
			base.OnLostFocus();

			for(int index = 0; index < Items.Count; index++)
				Items[index].LostFocus();
		}
		protected override void OnHide()
		{
			base.OnGainedFocus();
			for(int index = 0; index < Items.Count; index++)
				Items[index].Hide();
		}
		protected override void OnShow()
		{
			base.OnLostFocus();

			for(int index = 0; index < Items.Count; index++)
				Items[index].Show();
		}
		protected override void OnActivate()
		{
			base.OnActivate();

			for(int index = 0; index < Items.Count; index++)
			{
				if(!Items[index].HiddenByDefault)
					Items[index].Activate();
			}
		}
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			for(int index = 0; index < Items.Count; index++)
				Items[index].Deactivate();
		}
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			for(int index = 0; index < Items.Count; index++)
				Items[index].CleanUp();
		}
		protected override void OnInit()
		{
			base.OnInit();

			VisualElement ele = null;
			for(int i = 0; i < Items.Count; i++)
			{
				ele = Items[i];
				if (ele != null)
				{
					//Debug.Log(_elements[i].name);

					ele.Init();

					//Disables all renders, etc so that you don't have to manually do it
					if (HiddenByDefault)
					{
						ele.PresentVisuals(false);
					}
				}
			}
		}
		#endregion
	}
}