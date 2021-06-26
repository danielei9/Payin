using PayIn.Domain.Bus.Enums;

namespace PayIn.Application.Bus.Services
{
	public class RouteStop
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public NodeType Type { get; set; }

		public int RequestsIn { get; set; }
		public int RequestsOut { get; set; }

		public bool IsDefaultStop { get; set; }
		public bool IsLastStopsGo { get; set; }
		public bool IsLastStopsBack { get; set; }
		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? GeofenceRadious { get; set; }
	}
}
