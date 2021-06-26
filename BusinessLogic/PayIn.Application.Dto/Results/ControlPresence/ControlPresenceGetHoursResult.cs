using System;
using System.Collections.Generic;
using System.Text;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlPresence
{
	public class ControlPresenceGetHoursResult
	{
		public XpDate Day             { get; set; }
		public decimal DurationPlanned { get; set; }
		public decimal DurationWorked  { get; set; }
	}
}
