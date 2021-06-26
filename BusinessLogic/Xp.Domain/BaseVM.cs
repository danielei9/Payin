using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Xp.Domain
{
	public class BaseVM : INotifyPropertyChanged
	{
		#region PropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			var handler = PropertyChanged;
			if (handler != null && name != null)
				handler(this, new PropertyChangedEventArgs(name));
		}
		#endregion PropertyChanged

		#region Set
		protected void Set<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return;

			backingStore = value;
			OnPropertyChanged(propertyName);
		}
		#endregion Set
	}
}
