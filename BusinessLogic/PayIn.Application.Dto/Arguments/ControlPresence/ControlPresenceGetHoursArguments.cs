using System;
using System.Collections.Generic;
using System.Text;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
    public class ControlPresenceGetHoursArguments : IArgumentsBase
    {
		public int    WorkerId { get; set; }
		public XpDate Since { get; private set; }
		public XpDate Until { get; private set; }

		public ControlPresenceGetHoursArguments(int workerId, XpDate since, XpDate until) 
		{
			WorkerId = workerId;
			Since = since;
			Until = until;
		}
    }
}
