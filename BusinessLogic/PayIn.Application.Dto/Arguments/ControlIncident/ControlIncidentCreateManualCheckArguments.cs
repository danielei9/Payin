using PayIn.Common;
using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlIncident
{
	public class ControlIncidentCreateManualCheckArguments : IArgumentsBase
	{
		public IncidentType IncidentType { get; set; }
		public String       Observations { get; set; }
		public ControlIncidentCreateManualCheckArguments_IncidentItem Item { get; set; }

		#region Constructors
		public ControlIncidentCreateManualCheckArguments(IncidentType incidentType, String observations, ControlIncidentCreateManualCheckArguments_IncidentItem item)
		{
			IncidentType = incidentType;
			Observations = observations ?? "";
			Item = item;
		}
		#endregion Constructors
	}
}
