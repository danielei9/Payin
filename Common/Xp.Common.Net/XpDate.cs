using System;
using System.ComponentModel;
using Xp.Common.Net.Converters;

namespace Xp.Common
{
	[TypeConverter(typeof(XpDateConverter))]
	public class XpDate : IComparable<XpDate>
	{
		public DateTime Value { get; private set; }

		#region Constructors
		public XpDate(DateTime value)
		{
			Value = value.Date;
		}
		#endregion Constructors

		#region AddDays
		public XpDate AddDays(double value)
		{
			return Value.AddDays(value);
		}
		#endregion AddDays

		#region Cast to DateTime
		public static implicit operator DateTime?(XpDate d)
		{
			return d == null ? (DateTime?)null : d.Value;
		}
		public static implicit operator DateTime(XpDate d)
		{
			return d.Value;
		}
		#endregion Cast to DateTime

		#region Cast from DateTime
		public static implicit operator XpDate(DateTime datetime)
		{
			return new XpDate(datetime);
		}
		#endregion Cast from DateTime

		#region ToString
		public override string ToString()
		{
			var result = Value.ToString("yyyy-MM-dd");
			return result;
		}
		#endregion ToString

		#region FromString
		public static XpDate FromString(string value)
		{
			XpDate result = null;
			if (value == null || value == "null")
				result = new XpDate(MinValue);
			else
				try
				{
					DateTime dt = DateTime.Parse(value);
					result = Convert.ToDateTime(value);
				}
				catch
				{
				}
			return result;
		}
		#endregion FromString

		#region CompareTo
		public int CompareTo(XpDate other)
		{
			return other == null ? -1 : Value.CompareTo(other.Value);
		}
		#endregion CompareTo

		#region Equals
		public override bool Equals(object obj)
		{
			var value = obj as XpDate;
			return value == null ? false : this.Value.Equals(value.Value);
		}
		#endregion Equals

		#region GetHashCode
		public override int GetHashCode()
		{
			return Value == null ? 0 : Value.GetHashCode();
		}
		#endregion GetHashCode

		#region MaxValue
		public static XpDate MaxValue
		{
			get
			{
				return new XpDate(new DateTime(2999, 12, 31));
			}
		}
		#endregion MaxValue

		#region MinValue
		public static XpDate MinValue
		{
			get
			{
				return new XpDate(new DateTime(1753, 01, 01));
			}
		}
		#endregion MinValue
	}
}
