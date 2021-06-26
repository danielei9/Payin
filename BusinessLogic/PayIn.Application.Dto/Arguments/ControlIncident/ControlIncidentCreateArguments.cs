using PayIn.Common;
using System;
using System.Collections.Generic;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlIncident
{
    public class ControlIncidentCreateArguments : IArgumentsBase
    {
		public IncidentType IncidentType { get; set; }
		public String Observations { get; set; }
		public ControlIncidentCreateArguments_IncidentItem Item { get; set; }
		public IEnumerable<ControlIncidentCreateArguments_PlanningItem> PlanningItems { get; private set; }
		public IEnumerable<ControlIncidentCreateArguments_PlanningCheck> PlanningChecks { get; private set; }
		
		#region Constructors
		public ControlIncidentCreateArguments(IncidentType incidentType, String observations, ControlIncidentCreateArguments_IncidentItem item, IEnumerable<ControlIncidentCreateArguments_PlanningItem> planningItems, IEnumerable<ControlIncidentCreateArguments_PlanningCheck> planningChecks)
		{
			IncidentType = incidentType;
			PlanningItems = planningItems;
			PlanningChecks = planningChecks;
			Observations = observations ?? "";
			Item = item;
		}
		#endregion Constructors
    }
}
