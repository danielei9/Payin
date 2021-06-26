using System;
using System.ComponentModel;
using Xp.Common.Net.Converters;

namespace Xp.Common
{
	[TypeConverter(typeof(XpDurationConverter))]
	public class XpDuration
	{
		#region Value
		public DateTime _Value { get; set; }
		public TimeSpan Value
		{
			get
			{
				return _Value
					//.ToLocalTime()
					.TimeOfDay;
			}
		}
		#endregion Value

		#region TotalMinutes
		public double TotalMinutes
		{
			get
			{
				return Value.TotalMinutes;
			}
		}
		#endregion TotalMinutes

		#region Constructors
		public XpDuration(DateTime value)
		{
			_Value = value;
		}
		#endregion Constructors

		#region Cast to DateTime
		public static implicit operator DateTime?(XpDuration d)
		{
			return d == null ? (DateTime?) null : d._Value;
		}
		public static implicit operator DateTime(XpDuration d)
		{
			return d._Value;
		}
		#endregion Cast to DateTime

		#region Cast from DateTime
		public static implicit operator XpDuration(DateTime datetime)
		{
			return new XpDuration(datetime);
		}
		#endregion Cast from DateTime

		#region Cast from TimeSpan
		public static implicit operator XpDuration(TimeSpan time)
		{
			return new XpDuration(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).Add(time));
		}
		#endregion Cast from TimeSpan

		#region ToString
		public override string ToString()
		{
			var result = _Value.ToString("HH:mm:ss");
			return result;
		}
		#endregion ToString

		#region FromString
		public static XpDuration FromString(string value)
		{
			var datetime = Convert.ToDateTime(value);
			var time = datetime.Subtract(DateTime.Now.Date);
			var result = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).Add(time);

			return new XpDuration(result);
		}
		#endregion FromString
	}
}
