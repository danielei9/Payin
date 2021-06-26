using System.Collections.Generic;

namespace PayIn.Application.Bus.Services
{
	public class RouteItinerates
	{
		public IList<RouteItinerate> Itinerates { get; set; } = new List<RouteItinerate>();
		//public IEnumerable<RouteItinerateRoute> Routes { get; set; }
		public IEnumerable<RouteItinerateStop> Stops { get; set; }
	}
}
