using Xp.Application;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlTrack
{
	public class ControlTrackGetAllResult_Item : IPosition
	{
		public int Id { get; set; }
		public XpDateTime Date { get; set; }
		public string Image { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Longitude { get; set; }
		public int Quality { get; set; }
		public XpDuration Elapsed { get; set; }
		public decimal? Distance { get; set; }
		public decimal? Velocity { get; set; }
		public decimal? Acceleration { get; set; }
		public int FormsCount { get; set; }
		public int? CheckId { get; set; }
	}
}
