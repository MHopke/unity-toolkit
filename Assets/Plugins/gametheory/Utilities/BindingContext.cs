using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Reflection;

namespace gametheory
{
	public interface IBindingContext  
	{
		#region Events
		event System.Action<object,string> propertyChanged;
		#endregion
	}

	public class Binding
	{
		#region Private Vars
		protected Graphic _graphic;
		#endregion

		#region Constructors
		public Binding() {}
		public Binding(Graphic graphic)
		{
			_graphic = graphic;
		}
		#endregion

		#region Methods
		public virtual void PropertyChanged(object obj, PropertyInfo info){}
		#endregion
	}

	public class ImageBinding : Binding
	{
		#region Overridden Methods
		public override void PropertyChanged(object obj, PropertyInfo info)
		{
			base.PropertyChanged(obj,info);

			if(info.PropertyType == typeof(Color))
				(_graphic as Image).color = (Color)info.GetValue(obj,null);
			else if(info.PropertyType == typeof(Sprite))
				(_graphic as Image).sprite = info.GetValue(obj,null) as Sprite;
		}
		#endregion
	}

	public class TextBinding : Binding
	{
		#region Private Vars
		string _formatting;
		#endregion

		#region Constructors
		public TextBinding(Graphic graphic,string formatting="{0}") : base(graphic)
		{
			_formatting = formatting;
		}
		#endregion

		#region Overridden Methods
		public override void PropertyChanged (object obj, PropertyInfo info)
		{
			base.PropertyChanged (obj, info);

			(_graphic as Text).text = string.Format(_formatting,info.GetValue(obj,null));
		}
		#endregion
	}
}