﻿using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments.ControlIncident
{
	public partial class ControlIncidentCreateArguments_PlanningItem
	{
		public int Id { get; set; }
		public IEnumerable<ControlIncidentCreateArguments_PlanningCheck> Checks { get; set; }
	}
}
