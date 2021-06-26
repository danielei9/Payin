using System;
using System.Collections.Generic;
using System.Text;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlPresence
{
	public class ControlPresenceGetHoursResultBase : ResultBase<ControlPresenceGetHoursResult>
	{
		public string WorkerName { get; set; }
	}
}
