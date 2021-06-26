using PayIn.Domain.Bus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PayIn.Application.Bus.Services
{
	public static class RouteService
	{
		#region CreateItinerary
		public static RouteItinerates CreateItinerary(this RouteItinerates itinerary,
			RouteStop currentStop, RouteSense currentSense,
			List<RouteLink> linksGo, List<RouteLink> linksBack,
			IEnumerable<RouteStop> stops, RouteStop defaultStop,
			IEnumerable<RouteRequestStop> requestStops,
			IEnumerable<RouteStop> lastRoutesGo, IEnumerable<RouteStop> lastRoutesBack,
			IEnumerable<RouteStop> reverseStops
		)
		{
			try
			{
				// Nodes
				var go = (
					linksGo.Select(x => x.FromCode)
				)
				.Union(
					linksGo.Select(x => x.ToCode)
				)
				.Distinct()
				.Select(x => new Dijkstra.Node(x));
				var back = (
					linksBack.Select(x => x.FromCode)
				)
				.Union(
					linksBack.Select(x => x.ToCode)
				)
				.Distinct()
				.Select(x => new Dijkstra.Node(x));

				// Links
				var routesGo = linksGo.Select(x => new Dijkstra.Route(
					x.FromCode,
					x.ToCode,
					(double)(x.Weight ?? 1),
					x.Value
				));
				var routesBack = linksBack.Select(x => new Dijkstra.Route(
					x.FromCode,
					x.ToCode,
					(double)(x.Weight ?? 1),
					x.Value
				));
				var routes = routesGo.Union(routesBack);

				// Dijkstra
				var dijkstra = new Dijkstra.Dijkstra(
					currentStop?.Code ?? "",
					currentSense == RouteSense.Go ? Dijkstra.RouteWayEnum.Ida :
					currentSense == RouteSense.Back ? Dijkstra.RouteWayEnum.Vuelta :
						Dijkstra.RouteWayEnum.None,
					go,
					back,
					routes,
					routesGo,
					routesBack,
					defaultStop?.Code ?? "",
					lastRoutesBack.Select(x => new Dijkstra.Node(x.Code)),
					lastRoutesGo.Select(x => new Dijkstra.Node(x.Code)),
					reverseStops.Select(x => new Dijkstra.RoutingNode(x.Code, Dijkstra.NodeType.PointReverse, ""))
				);

				foreach (var requestStop in requestStops)
				{
					var stopList = new List<string>();
					if (!requestStop.FromVisited)
						stopList.Add(requestStop.FromCode);
					stopList.Add(requestStop.ToCode);

					dijkstra.AddNode(currentStop?.Code ?? defaultStop?.Code, dijkstra.RoutesStopsList, 0, stopList);
				}
				var nodes = dijkstra.GetFullItinerary();
				
				itinerary.Stops = nodes
					.Select(x => new RouteItinerateStop
					{
						Id = stops.Where(y => y.Code == x.Code).FirstOrDefault()?.Id ?? 0,
						Code = x.Code,
						Sense =
							x.Way == Dijkstra.RouteWayEnum.Ida ? RouteSense.Go :
							x.Way == Dijkstra.RouteWayEnum.Vuelta ? RouteSense.Back :
							x.Way == Dijkstra.RouteWayEnum.None ? RouteSense.None :
								RouteSense.Both,
						Name = stops.Where(y => y.Code == x.Code).FirstOrDefault()?.Name ?? "",
						IsRequested = x.IsSelected,
						Type = stops.Where(y => y.Code == x.Code).FirstOrDefault()?.Type ?? NodeType.None,
						Longitude = stops.Where(y => y.Code == x.Code).FirstOrDefault()?.Longitude,
						Latitude = stops.Where(y => y.Code == x.Code).FirstOrDefault()?.Latitude,
						GeofenceRadious = stops.Where(y => y.Code == x.Code).FirstOrDefault()?.GeofenceRadious,
						RequestsIn = stops.Where(y => y.Code == x.Code).FirstOrDefault()?.RequestsIn ?? 0,
						RequestsOut = stops.Where(y => y.Code == x.Code).FirstOrDefault()?.RequestsOut ?? 0
					})
					.ToList(); // Necsario para poder modificar sus valores

				// Calculate times
				var sum = 0m;
				var previousNode = "";
				foreach (var stop in itinerary.Stops)
				{
					if (previousNode.IsNullOrEmpty())
						previousNode = stop.Code;
					else
					{
						var value = routes
							.Where(x =>
								x.From == previousNode &&
								x.To == stop.Code
							)
							.FirstOrDefault();
						previousNode = value?.To ?? "";

						sum = sum + (value.Value ?? 0m);
					}
					stop.Value = sum;
				}

				return itinerary;
			}
			catch(Exception e)
			{
				return null;
			}
		}
		#endregion CreateItinerary
	}
}
