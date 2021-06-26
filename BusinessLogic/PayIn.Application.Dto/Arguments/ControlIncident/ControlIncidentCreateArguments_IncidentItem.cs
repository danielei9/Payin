using Xp.Common;

namespace PayIn.Application.Dto.Arguments.ControlIncident
{
	public class ControlIncidentCreateArguments_IncidentItem
	{
		public XpDateTime Date { get; set; }
		public byte[] Image { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Longitude { get; set; }
		public int ItemId { get; set; }
	}
}
