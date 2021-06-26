using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTimeTable
{
	public partial class ServiceTimeTableCreateArguments : IArgumentsBase
	{
		public int ZoneId { get; private set; }
		public DayOfWeek FromWeekday { get; private set; }
		public DayOfWeek UntilWeekday { get; private set; }
		public TimeSpan FromHour { get; private set; }
		public TimeSpan DurationHour { get; private set; }

		#region Constructors
		public ServiceTimeTableCreateArguments(int zoneId, DayOfWeek fromWeekday, DayOfWeek untilWeekday, TimeSpan fromHour, TimeSpan durationHour)
		{
			ZoneId = zoneId;
			FromWeekday = fromWeekday;
			UntilWeekday = untilWeekday;
			FromHour = fromHour;
			DurationHour = durationHour;
		}
		#endregion Constructors
	}
}
