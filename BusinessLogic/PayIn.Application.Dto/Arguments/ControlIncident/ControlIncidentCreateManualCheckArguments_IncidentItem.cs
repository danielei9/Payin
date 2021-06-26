using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Arguments.ControlIncident
{
	public partial class ControlIncidentCreateManualCheckArguments_IncidentItem
	{
		public XpDateTime Date { get; set; }
		public byte[] Image { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Longitude { get; set; }
		public int Id { get; set; }
		public int? CheckPointId { get; set; }
		public int? CheckId { get; set; }
		public IEnumerable<ControlIncidentCreateManualCheckArguments_Assign> Assigns { get; set; }
	}
}
