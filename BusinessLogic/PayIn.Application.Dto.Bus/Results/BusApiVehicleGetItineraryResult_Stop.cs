using PayIn.Domain.Bus.Enums;
using System;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiVehicleGetItineraryResult_Stop
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
		public TimeSpan Time { get; set; }
		public int RequestsIn { get; set; }
		public int RequestsOut { get; set; }
	}
}
