using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace gametheory.UI
{
	public class UIGroup : VisualElement 
	{
		#region Public Vars
		public VisualElement[] Items;
		#endregion

		#region Overridden Methods
		public override void PresentVisuals (bool display)
		{
			base.PresentVisuals(display);

			if(!display)
			{
				for(int index = 0; index < Items.Length; index++)
					Items[index].PresentVisuals(display);
			}
		}
		protected override void Disabled()
		{
			base.Disabled();

			for(int index = 0; index < Items.Length; index++)
				Items[index].Disable();
		}
		protected override void Enabled()
		{
			base.Enabled();

			for(int index = 0; index < Items.Length; index++)
				Items[index].Enable();
		}
		public override void OnGainedFocus()
		{
			base.OnGainedFocus();

			for(int index = 0; index < Items.Length; index++)
				Items[index].GainedFocus();
		}
		public override void OnLostFocus()
		{
			base.OnLostFocus();

			for(int index = 0; index < Items.Length; index++)
				Items[index].LostFocus();
		}
		protected override void OnPresent()
		{
			base.OnPresent();

			for(int index = 0; index < Items.Length; index++)
			{
				if(!Items[index].SkipUIViewActivation)
					Items[index].Present();
			}
		}
		protected override void OnRemove()
		{
			base.OnRemove();
			for(int index = 0; index < Items.Length; index++)
				Items[index].Remove();
		}
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			for(int index = 0; index < Items.Length; index++)
				Items[index].CleanUp();
		}
		protected override void OnInit()
		{
			base.OnInit();
			for(int index = 0; index < Items.Length; index++)
				Items[index].Init();
		}
		#endregion
	}
}