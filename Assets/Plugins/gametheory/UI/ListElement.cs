namespace gametheory.UI
{
	public class ListElement : VisualElement
	{
		#region Events
		public event System.Action<object> selected;
		#endregion

		#region Protected Vars
		protected object _obj;
		#endregion

		#region UI Methods
		public void Selected()
		{
			OnSelected();

			if(selected != null)
				selected(_obj);
		}
		#endregion

		#region Virtual Methods
		public virtual void Setup(object obj)
		{
			_obj = obj;
			SetContext(obj);
		}
		protected virtual void OnSelected(){}
		#endregion
	}
}
