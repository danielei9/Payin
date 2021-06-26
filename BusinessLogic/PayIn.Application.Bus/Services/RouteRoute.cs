using PayIn.Domain.Bus.Enums;
using System.Collections.Generic;

namespace PayIn.Application.Bus.Services
{
	public class RouteRoute
	{
		public RouteSense Sense { get; set; }

		public IEnumerable<RouteLink> Links { get; set; }
	}
}
