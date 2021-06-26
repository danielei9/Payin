using PayIn.Domain.Bus.Enums;

namespace PayIn.Application.Bus.Services
{
	public class RouteItinerateStop
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public RouteSense Sense { get; set; }
		public bool IsRequested { get; set; }
		public NodeType Type { get; set; }
		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? GeofenceRadious { get; set; }
		public decimal Value { get; set; }
		public int RequestsIn { get; set; }
		public int RequestsOut { get; set; }
	}
}
