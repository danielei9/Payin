using System;
using System.ComponentModel;
using Xp.Common.Net.Converters;

namespace Xp.Common
{
	[TypeConverter(typeof(XpDateTimeConverter))]
	public class XpDateTime
	{
		private DateTime _Value { get; set; }
		public DateTime Value
		{
			get
			{
				return this._Value
					.ToLocalTime();
			}
		}

		#region Date
		public XpDate Date
		{
			get
			{
				return Value.Date;
			}
		}
		#endregion Date

		#region Constructors
		public XpDateTime(DateTime value)
		{
			if ((value.Kind == DateTimeKind.Unspecified) ||  (value.Kind == DateTimeKind.Local))
				_Value = value.ToUTC();
			else
				_Value = value;
		}
		#endregion Constructors

		#region AddDays
		public XpDateTime AddDays(double value)
		{
			return Value.AddDays(value);
		}
		#endregion AddDays

		#region Cast to DateTime
		public static implicit operator DateTime?(XpDateTime d)
		{
			return d?._Value;
		}
		public static implicit operator DateTime(XpDateTime d)
		{
			return d._Value;
		}
		#endregion Cast to DateTime

		#region Cast from DateTime
		public static implicit operator XpDateTime(DateTime datetime)
		{
			if (
				(datetime == MinValue) ||
				(datetime == MaxValue)
			)
				return null;

			return new XpDateTime(datetime);
        }
        #endregion Cast from DateTime

        #region ToString
        public override string ToString()
		{
			var result = Value.ToUTC().ToString("yyyy-MM-ddTHH:mm:ssZ");
			return result;
		}
		#endregion ToString

		#region FromString
		public static XpDateTime FromString(string value)
		{
			var datetime = Convert.ToDateTime(value);
			var final = datetime.ToUTC();

			return new XpDateTime(final);
		}
		#endregion FromString

		#region FromDateTime
		public static XpDateTime FromDateTime(DateTime value)
		{
			return new XpDateTime(value);
		}
		#endregion FromDateTime

		#region MaxValue
		public static XpDateTime MaxValue
		{
			get
			{
				return new XpDateTime(new DateTime(2999, 12, 31, 23, 59, 59));
			}
		}
		#endregion MaxValue

		#region MinValue
		public static XpDateTime MinValue
		{
			get
			{
				return new XpDateTime(new DateTime(1753, 01, 01, 00, 00, 00));
			}
		}
		#endregion MinValue
	}
}
