using PayIn.Domain.Bus.Enums;
using System.Collections.Generic;

namespace PayIn.Application.Bus.Services
{
	public class RouteItinerate
	{
		public int Id { get; set; }
		public int? LineId { get; set; }
		public RouteSense CurrentSense { get; set; }
		public RouteStop CurrentStop { get; set; }

		public IEnumerable<RouteStop> Stops { get; set; }
		public IEnumerable<RouteRoute> Routes { get; set; }
		public IEnumerable<RouteRequestStop> RequestStops { get; set; }

		public IEnumerable<RouteStop> LastRoutesGo { get; set; }
		public IEnumerable<RouteStop> LastRoutesBack { get; set; }
		public RouteStop DefaultStop { get; set; }
		public IEnumerable<RouteStop> ReverseStops { get; set; }
	}
}
