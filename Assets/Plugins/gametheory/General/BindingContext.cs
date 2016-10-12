﻿using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using gametheory.Utilities;

namespace gametheory
{
	public interface IBindingContext  
	{
		#region Events
		event System.Action<object,string> propertyChanged;
		#endregion
	}

	public class DataContext : IBindingContext
	{
		#region Events
		public event System.Action<object,string> propertyChanged;
		#endregion

		#region Methods
		public void OnPropertyChanged(string propName)
		{
			if(propertyChanged != null)
			{
				propertyChanged(this,propName);
			}
		}
		#endregion
	}

	public class DataBinder : MonoBehaviour
	{
		#region Protected Vars
		protected object _context;
		protected Dictionary<string,Binding> _bindings;
		#endregion

		#region Methods
		public virtual void SetContext(object obj)
		{
			//clear any old information
			if(_context != null)
				ClearContext();

			_context = obj;

			if(obj is IBindingContext)
			{
				IBindingContext context = _context as IBindingContext;
				context.propertyChanged += OnPropertyChanged;
			}
			
			OnContextSet();
		}
		protected virtual void OnContextSet(){}

		public virtual void SetBinding(string propName, Binding binding)
		{
			if(_context == null)
				return;

			if(_bindings == null)
				_bindings = new Dictionary<string, Binding>();

			if(_bindings.ContainsKey(propName))
				_bindings[propName] = binding;
			else
				_bindings.Add(propName,binding);

			//setup the info initially
			OnPropertyChanged(_context,propName);
		}
		protected virtual void OnPropertyChanged(object obj, string propName)
		{
			if(_bindings != null)
			{
				if(_bindings.ContainsKey(propName))
				{
					_bindings[propName].PropertyChanged(obj,obj.GetType().GetProperty(propName));
				}
			}
		}

		public void ClearContext()
		{
			if(_context != null && _context is IBindingContext)
			{
				IBindingContext context = _context as IBindingContext;
				context.propertyChanged -= OnPropertyChanged;
			}

			if(_bindings != null)
				_bindings.Clear();
		}
		protected void SetProperty(string name, object value)
		{
			if(_context != null)
			{
				PropertyInfo info = _context.GetType().GetProperty(name);
				info.SetValue(_context,value,null);
			}
		}
		protected object GetProperty(string name)
		{
			if(_context != null)
			{
				PropertyInfo info = _context.GetType().GetProperty(name);
				return info.GetValue(_context,null);
			}

			return null;
		}
		#endregion
	}

	public class Binding
	{
		#region Private Vars
		protected ICanvasElement _graphic;
		#endregion

		#region Constructors
		public Binding() {}
		public Binding(ICanvasElement graphic)
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
		public TextBinding(ICanvasElement graphic,string formatting="{0}") : base(graphic)
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

	public class SliderBinding : Binding
	{
		#region Constructors
		public SliderBinding(){}
		public SliderBinding(ICanvasElement graphic) : base(graphic){}
		#endregion

		#region Overridden Methods
		public override void PropertyChanged (object obj, PropertyInfo info)
		{
			base.PropertyChanged (obj, info);
			(_graphic as Slider).value = (float)System.Convert.ToDouble(info.GetValue(obj,null));
		}
		#endregion
	}

	public class FuncBinding : Binding
	{
		#region Private Vars
		Action _func;
		#endregion

		#region Constructors
		public FuncBinding(Action callback) : base(null)
		{
			_func = callback;
		}
		#endregion

		#region Overridden Methods
		public override void PropertyChanged (object obj, PropertyInfo info)
		{
			base.PropertyChanged (obj, info);

			if(_func != null)
				_func();
		}
		#endregion
	}

	public class TimeSpanBinding : Binding
	{
		#region Constructors
		public TimeSpanBinding(){}
		public TimeSpanBinding(ICanvasElement graphic) : base(graphic){}
		#endregion

		#region Overridden Methods
		public override void PropertyChanged (object obj, PropertyInfo info)
		{
			base.PropertyChanged (obj, info);
			(_graphic as Text).text = Helper.ConvertTimeSpanToString((TimeSpan)info.GetValue(obj,null));
		}
		#endregion
	}

	public class FillBinding : Binding
	{
		#region Private Vars
		float _max;
		#endregion

		#region Constructors
		public FillBinding(Graphic graphic, float max) : base(graphic)
		{
			_max = max;
		}
		#endregion

		#region Overridden Methods
		public override void PropertyChanged (object obj, PropertyInfo info)
		{
			base.PropertyChanged (obj, info);
			(_graphic as Image).fillAmount = (float)((int)(info.GetValue(obj,null))) / (float)_max;
		}
		#endregion
	}
}