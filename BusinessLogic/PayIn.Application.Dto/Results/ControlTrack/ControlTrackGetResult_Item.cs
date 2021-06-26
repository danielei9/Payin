using Xp.Application;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlTrack
{
	public class ControlTrackGetResult_Item : IPosition
	{
		public int Id { get; set; }
		public XpDateTime Date { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Longitude { get; set; }
		public int Quality { get; set; }
		public XpDuration Elapsed { get; set; }
		public decimal? Distance { get; set; }
		public decimal? Velocity { get; set; }
		public decimal? MobileVelocity { get; set; }
		public decimal? Acceleration { get; set; }
	}
}
