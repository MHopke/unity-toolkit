using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace gametheory
{
	public class ObservableList<T> : Collection<T>
	{
		#region Events
		public event System.Action<int,T> itemChanged;
		public event System.Action cleared;
		#endregion

		#region Overridden Methods
		protected override void InsertItem (int index, T item)
		{
			base.InsertItem (index, item);

			if(itemChanged != null)
				itemChanged(index,item);
		}
		protected override void SetItem (int index, T item)
		{
			base.SetItem (index, item);

			if(itemChanged != null)
				itemChanged(index, item);
		}
		protected override void RemoveItem (int index)
		{
			base.RemoveItem (index);
			if(itemChanged != null)
				itemChanged(index,default(T));
		}
		protected override void ClearItems ()
		{
			base.ClearItems ();

			if(cleared != null)
				cleared();
		}
		#endregion
	}
}