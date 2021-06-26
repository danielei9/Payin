using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments.ControlIncident
{
	public partial class ControlIncidentCreateManualCheckArguments_Assign
	{
		public int Id { get; set; }
		public IEnumerable<ControlIncidentCreateManualCheckArguments_Value> Values { get; set; }
	}
}
