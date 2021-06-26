using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTimeTable
{
	public partial class ServiceTimeTableUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int ConcessionId { get; private set; }
		public int ZoneId { get; set; }
		public DayOfWeek FromWeekday { get; private set; }
		public DayOfWeek UntilWeekday { get; private set; }
		public TimeSpan FromHour { get; private set; }
		public TimeSpan DurationHour { get; private set; }

		#region Constructors
		public ServiceTimeTableUpdateArguments(int id, int concessionId, int zoneId, DayOfWeek fromWeekday, DayOfWeek untilWeekday, TimeSpan fromHour, TimeSpan durationHour)
		{
			Id = id;
			ZoneId = zoneId;
			ConcessionId = concessionId;
			FromWeekday = fromWeekday;
			UntilWeekday = untilWeekday;
			FromHour = fromHour;
			DurationHour = durationHour;
		}
		#endregion Constructors
	}
}
