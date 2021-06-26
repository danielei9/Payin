using System;
using System.ComponentModel;
using Xp.Common.Net.Converters;

namespace Xp.Common
{
	[TypeConverter(typeof(XpTimeConverter))]
	public class XpTime
	{
        #region Value
        private DateTime _Value { get; set; }
		public TimeSpan? Value
		{
			get
			{
				return this._Value
					.ToLocalTime()
					.TimeOfDay;
			}
        }
        #endregion Value

        #region Constructors
        public XpTime(DateTime value)
		{
			if ((value.Kind == DateTimeKind.Unspecified) ||  (value.Kind == DateTimeKind.Local))
				_Value = value.ToUTC();
			else
				_Value = value;
		}
		#endregion Constructors

		#region Cast to DateTime
		public static implicit operator DateTime?(XpTime d)
		{
			return d == null ? (DateTime?)null : d._Value;
		}
		public static implicit operator DateTime(XpTime d)
		{
			return d._Value;
		}
		#endregion Cast to DateTime

		#region Cast from DateTime
		public static implicit operator XpTime(DateTime datetime)
		{
			return new XpTime(datetime);
		}
		#endregion Cast from DateTime

		#region ToString
		public override string ToString()
		{
			if (Value == null)
				return "";
			var date = DateTime.Now.Date.Add(Value.Value);

			var result = date.ToUTC().ToString("HH:mm:ssZ");
			return result;
		}
		#endregion ToString

		#region FromString
		public static XpTime FromString(string value)
		{
			var datetime = Convert.ToDateTime(value);

			return FromDateTime(datetime);
		}
		#endregion FromString

		#region FromDateTime
		public static XpTime FromDateTime(DateTime value)
		{
			var time = value.ToLocalTime().Subtract(DateTime.Now.Date);
			var date = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local).Add(time);
			var final = date.ToUTC();

			return new XpTime(final);
		}
		#endregion FromDateTime

		#region InPeriod
		public bool InPeriod(XpTime since, XpTime until)
		{
			// Gestión de nulos
			if ((since == null) && (until == null))
				return true;
			if (until == null)
				return (since.Value <= Value);
			if (since == null)
				return (until.Value >= Value);

			// Horario normal
			if (since.Value <= until.Value)
				return ((since.Value <= Value) && (until.Value >= Value));

			// Horario cruzado
			return ((since.Value <= Value) || (until.Value >= Value));
		}
		#endregion InPeriod
	}
}
