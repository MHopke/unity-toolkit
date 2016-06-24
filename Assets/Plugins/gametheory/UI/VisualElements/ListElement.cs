namespace gametheory.UI
{
	public class ListElement : VisualElement
	{
		#region Events
		public event System.Action<VisualElement,object> selected;
		public event System.Action<VisualElement> delete;
		#endregion

		#region Protected Vars
		protected object _obj;
		#endregion

		#region UI Methods
		public void Selected()
		{
			OnSelected();
		}
		public void Delete()
		{
			OnDelete();
		}
		#endregion

		#region Virtual Methods
		public virtual void Setup(object obj)
		{
			_obj = obj;
			SetContext(obj);
		}
		protected virtual void OnSelected()
		{
			if(selected != null)
				selected(this,_obj);
		}
		protected virtual void OnDelete()
		{
			if(delete != null)
				delete(this);
		}
		#endregion

		#region Properties
		public object Object
		{
			get { return _obj; }
		}
		#endregion
	}
}
