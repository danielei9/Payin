using System;
using Xp.Common;

namespace PayIn.Presentation.Common
{
	public class GlobalVariables : Bindable
	{
		#region ParkingDeadline
		private DateTime? _ParkingDeadline;
		public DateTime? ParkingDeadline
		{
			get
			{
				return _ParkingDeadline;
			}
			set
			{
				base.Set(ref _ParkingDeadline, value);
			}
		}
		#endregion ParkingDeadline

		#region Contructors
		public GlobalVariables()
		{
		}
		#endregion Contructors
	}
}
