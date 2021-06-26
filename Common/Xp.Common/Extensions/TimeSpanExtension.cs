
namespace System
{
	public static class TimeSpanExtension
	{
		#region TruncateHour
		public static TimeSpan TruncateHour(this TimeSpan time)
		{
			return TimeSpan.FromHours(time.Hours);
		}
		#endregion TruncateHour

		#region TruncateMinute
		public static TimeSpan TruncateMinute(this TimeSpan time)
		{
			return TimeSpan.FromMinutes(time.Hours * 60 + time.Minutes);
		}
		#endregion TruncateMinute

		#region TruncateSecond
		public static TimeSpan TruncateSecond(this TimeSpan time)
		{
			return TimeSpan.FromSeconds(time.Hours * 3600 + time.Minutes * 60);
		}
		#endregion TruncateSecond
	}
}
