using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using gametheory.Utilities;

namespace gametheory.UI
{
	[RequireComponent(typeof(Dropdown))]
    public class ExtendedDropdown : VisualElement
    {
        #region Public Vars
        public Image Arrow;
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

                PresentComponent(Arrow, display);
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

        public void InitializeWithEnums<T>(int selection,params string[] ignoreValues)
        {
            System.Array arr = EnumUtility.GetValues<T>();
            //some stuff here
            Dropdown.options.Clear();

            int sub = 0, select=0;
            bool keep = true;
            string str = "";
            
            for (int index = 0; index < arr.Length; index++)
            {
                str = arr.GetValue(index).ToString();
                keep = true;

                for(sub =0; sub <ignoreValues.Length; sub++)
                {
                    if(ignoreValues[sub] == str)
                    {
                        keep = false;
                        break;
                    }
                }
                
                if (keep)
                {
                    Dropdown.options.Add(new Dropdown.OptionData(str));
                    select = Dropdown.options.Count - 1;
                    if (select == selection)
                        Dropdown.captionText.text = Dropdown.options[select].text;
                }
            }

            Dropdown.value = selection;

            //Dropdown.options[selection].
        }
        #endregion
    }
}