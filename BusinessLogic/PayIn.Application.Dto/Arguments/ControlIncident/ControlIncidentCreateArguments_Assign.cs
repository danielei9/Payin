using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments.ControlIncident
{
	public partial class ControlIncidentCreateArguments_Assign
	{
		public int Id { get; set; }
		public IEnumerable<ControlIncidentCreateArguments_Value> Values { get; set; }
	}
}
