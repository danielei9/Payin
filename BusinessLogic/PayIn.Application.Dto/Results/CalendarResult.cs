using System;
using System.Collections.Generic;
using System.Text;

namespace PayIn.Application.Dto.Results
{
   public partial class CalendarResult
    {
		public int	  Id		{ get; set; }
		public string Title     { get; set; }
		public DateTime Start     { get; set; }
		public DateTime End       { get; set; }			
		public string Location  { get; set; }
		public string Info      { get; set; }
		
	    public DayOfWeek FromWeekday { get; set; }
		public string FromWeekdayLabel { get; set; }
		public DayOfWeek UntilWeekday { get; set; }
		public string UntilWeekdayLabel { get; set; }
		public TimeSpan FromHour { get; set; }
		public int ZoneId { get; set; }
		public TimeSpan UntilHour { get; set; }
		public string ZoneName { get; set; }
		public int ConcessionId { get; set; }
		public string ConcessionName { get; set; }
		
		
    }
}
