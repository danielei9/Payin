namespace System
{
	public static class DateTimeExtension
	{
		#region ToUTC
		public static DateTime ToUTC(this DateTime that)
		{
			if (that.Kind == DateTimeKind.Unspecified)
				return new DateTime(that.Ticks, DateTimeKind.Utc);
			else
				return that.ToUniversalTime();
		}
		public static DateTime? ToUTC(this DateTime? that)
		{
			return that != null ?
				that.Value.ToUTC() :
				(DateTime?)null;
		}
		#endregion ToUTC

		#region ToLTC
		public static DateTime ToLTC(this DateTime that)
		{
			if (that.Kind == DateTimeKind.Unspecified)
				return new DateTime(that.Ticks, DateTimeKind.Local);
			else
				return that.ToLocalTime();
		}
		public static DateTime? ToLTC(this DateTime? that)
		{
			return that != null ?
				that.Value.ToLTC() :
				(DateTime?)null;
		}
		#endregion ToLTC

		#region ToLocalTime
		public static DateTime? ToLocalTime(this DateTime? that)
		{
			if (that == null)
				return null;
			return that.Value.ToLocalTime();
		}
		#endregion ToLocalTime

		#region FloorHour
		public static DateTime FloorHour(this DateTime that)
		{
			return new DateTime(that.Year, that.Month, that.Day, that.Hour, 0, 0, that.Kind);
		}
		public static DateTime? FloorHour(this DateTime? that)
		{
			if (that == null)
				return null;

			return that.Value.FloorHour();
		}
		#endregion FloorHour

		#region GetQuarter
		public static int GetQuarter(this DateTime that)
		{
			if (that.Minute < 15)
				return 0;
			if (that.Minute < 30)
				return 1;
			if (that.Minute < 45)
				return 2;

			return 3;
		}
		public static int? GetQuarter(this DateTime? that)
		{
			if (that == null)
				return null;

			return that.Value.GetQuarter();
		}
		#endregion GetQuarter

		#region FloorMonth
		public static DateTime FloorMonth(this DateTime that)
		{
			return new DateTime(that.Year, that.Month, 1);
		}
		public static DateTime? FloorMonth(this DateTime? that)
		{
			if (that == null)
				return null;
			return that.Value.FloorMonth();
		}
		#endregion FloorMonth

		#region FloorYear
		public static DateTime FloorYear(this DateTime that)
		{
			return new DateTime(that.Year, 1, 1);
		}
		public static DateTime? FloorYear(this DateTime? that)
		{
			if (that == null)
				return null;
			return that.Value.FloorYear();
		}
		#endregion FloorYear
	}
}
