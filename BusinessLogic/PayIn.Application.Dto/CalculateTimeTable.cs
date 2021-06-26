using System;

namespace PayIn.Application.Dto
{
	public class CalculateTimeTable
	{
		public int				Id						{ get; set; }
		public DayOfWeek	FromWeekday		{ get; set; }
		public DayOfWeek	UntilWeekday	{ get; set; }
		public TimeSpan		FromHour			{ get; set; }
		public TimeSpan		DurationHour	{ get; set; }
	}
}
