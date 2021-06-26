using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments.ControlIncident
{
	public partial class ControlIncidentCreateArguments_PlanningCheck
	{
		public int Id { get; set; }
		public IEnumerable<ControlIncidentCreateArguments_Assign> Assigns { get; set; }
	}
}
