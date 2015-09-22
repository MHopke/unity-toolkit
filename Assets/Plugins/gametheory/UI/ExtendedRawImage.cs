using UnityEngine;
using UnityEngine.UI;

namespace gametheory.UI
{
	[RequireComponent(typeof(RawImage))]
	public class ExtendedRawImage : VisualElement 
	{
		#region Public Vars
		public RawImage Image;
		#endregion
		
		#region Overriden Methods
		protected override void OnInit()
		{
			base.OnInit();
			
			if (!Image)
				Image = GetComponent<RawImage>();
		}
		public override void PresentVisuals(bool display)
		{
			base.PresentVisuals(display);
			
			if(Image)
				Image.enabled = display;
		}
		#endregion
	}
}
