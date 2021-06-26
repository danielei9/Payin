using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Xp.Common
{
	public class Bindable // XAVI: INotifyPropertyChanged
	{
		#region Set
		public bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(storage, value))
				return false;

			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}
		#endregion Set
		
		#region PropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion PropertyChanged
	}
}
