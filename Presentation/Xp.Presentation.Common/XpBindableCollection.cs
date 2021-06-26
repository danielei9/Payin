using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Xp.Presentation.Common
{
	public class XpBindableCollection<T> : ObservableCollection<T>
	{
		#region AddRange
		public void AddRange(IEnumerable<T> collection)
		{
			foreach (var item in collection)
				this.Add(item);
		}
		#endregion AddRange

		#region ClearAndAddRange
		public void ClearAndAddRange(IEnumerable<T> collection)
		{
			this.Clear();
			foreach (var item in collection)
				this.Add(item);
		}
		#endregion ClearAndAddRange
	}
}
