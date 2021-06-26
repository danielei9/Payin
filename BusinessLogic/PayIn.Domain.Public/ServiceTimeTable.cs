using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceTimeTable : IEntity
	{
		                          public int       Id           { get; set; }
		                          public DayOfWeek FromWeekday  { get; set; }
		                          public DayOfWeek UntilWeekday { get; set; }
		[DataType(DataType.Time)] public TimeSpan  FromHour     { get; set; }
		[DataType(DataType.Time)] public TimeSpan  DurationHour { get; set; }

		#region Zone
		public int ZoneId { get; set; }
		public ServiceZone Zone { get; set; }
		#endregion Zone
	}
}
