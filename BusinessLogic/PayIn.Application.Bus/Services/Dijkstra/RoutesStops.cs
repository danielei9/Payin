using System;
using System.Collections.Generic;

namespace PayIn.Application.Bus.Services.Dijkstra
{
	public class RoutesStops
	{
		public RouteWayEnum Way { get; set; }
		public List<Node> Nodes { get; set; }
		public string idRoute;

		#region Constructors
		public RoutesStops(RouteWayEnum way)
		{
			this.Way = way;
			this.Nodes = new List<Node>();

			this.idRoute = DateTime.Today.ToString();
		}
		#endregion Constructors
	}
}
