using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlPlanningCheck
{
	public class ControlPlanningCheckMobileGetResult_Planning
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int EllapsedMinutes { get; set; }
		public IEnumerable<ControlPlanningCheckMobileGetResult_Assign> Assigns { get; set; }
		public int? CheckId { get; set; }
		public int? CheckPointId { get; set; }
		public PresenceType PresenceType { get; set; }
	}
}
