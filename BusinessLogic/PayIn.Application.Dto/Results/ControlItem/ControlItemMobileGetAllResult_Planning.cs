using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlItem
{
	public class ControlItemMobileGetAllResult_Planning
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int EllapsedMinutes { get; set; }
		public IEnumerable<ControlItemMobileGetAllResult_Assign> Assigns { get; set; }
		public int? CheckId { get; set; }
		public int? CheckPointId { get; set; }
		public PresenceType PresenceType { get; set; }
	}
}
