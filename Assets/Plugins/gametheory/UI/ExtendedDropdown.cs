using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace gametheory.UI
{
	[RequireComponent(typeof(Dropdown))]
    public class ExtendedDropdown : VisualElement
    {
        #region Public Vars
		public Dropdown Dropdown;
        #endregion
        
		#region Overridden Methods
		public override void PresentVisuals (bool display)
		{
			base.PresentVisuals (display);

			if(Dropdown)
			{
				Dropdown.enabled = display;

				if(Dropdown.image)
					Dropdown.image.enabled = display;

				if(Dropdown.captionText)
					Dropdown.captionText.enabled = display;

				if(Dropdown.captionImage)
					Dropdown.captionImage.enabled = display;
			}
		}
		protected override void Disabled ()
		{
			base.Disabled ();

			if(Dropdown)
				Dropdown.interactable = false;
		}
		protected override void Enabled ()
		{
			base.Enabled ();

			if(Dropdown)
				Dropdown.interactable = true;
		}
		#endregion

        #region Methods
        public void Initialize(List<string> items, int selection)
        {
			//some stuff here
			Dropdown.options = new List<Dropdown.OptionData>();

			for(int index = 0; index < items.Count; index++)
				Dropdown.options.Add(new Dropdown.OptionData(items[index]));

			Dropdown.value = selection;
			Dropdown.captionText.text = items[selection];

			//Dropdown.options[selection].
        }
        #endregion
    }
}