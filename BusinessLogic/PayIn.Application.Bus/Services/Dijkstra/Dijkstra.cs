using System.Collections.Generic;
using System.Linq;
using System;

namespace PayIn.Application.Bus.Services.Dijkstra
{
	public class Dijkstra
	{
		// Posición y sentido actual del bus
		public string CurrentStopName { get; set; }
		public RouteWayEnum CurrentWay { get; set; }
		
		// Parada por defecto
		public string DefaultStopName { get; set; }

		// Indica inicios y finales de ruta
		public List<Node> LastNodesGo;
		public List<Node> LastNodesBack;

		// Grafos
		public IEnumerable<Route> Routes = new List<Route>();
		public IEnumerable<Route> RoutesGo = new List<Route>();
		public IEnumerable<Route> RoutesBack = new List<Route>();

		// Puntos de cambio de sentido
		public List<RoutingNode> RoutesreverseNodeList = new List<RoutingNode>();

		public Dictionary<string, Node> NodeDict = new Dictionary<string, Node>();
		public Dictionary<string, Node> NodeDictGo = new Dictionary<string, Node>();
		public Dictionary<string, Node> NodeDictBack = new Dictionary<string, Node>();

		public HashSet<string> Unvisited = new HashSet<string>();
		public HashSet<string> UnvisitedGo = new HashSet<string>();
		public HashSet<string> UnvisitedBack = new HashSet<string>();

		public RoutesStopsList RoutesStopsList = new RoutesStopsList();

		public Dijkstra(
			string currentStopName, RouteWayEnum currentWay,
			IEnumerable<Node> nodeDictGo, IEnumerable<Node> nodeDictBack,
			IEnumerable<Route> routes, IEnumerable<Route> routesGo, IEnumerable<Route> routesBack,
			string defaultStopName, IEnumerable<Node> lastNodesGo, IEnumerable<Node> lastNodesBack,
			IEnumerable<RoutingNode> routesreverseNodeList
		)
		{
			this.CurrentStopName = currentStopName.IsNullOrEmpty() ? defaultStopName : currentStopName;
			this.CurrentWay = currentWay;

			this.Routes = routes;
			this.RoutesGo = routesGo;
			this.RoutesBack = routesBack;

			this.NodeDictGo = nodeDictGo
				.ToDictionary(
					x => x.Code,
					x => x
				);
			this.NodeDictBack = nodeDictBack
				.ToDictionary(
					x => x.Code,
					x => x
				);
			this.NodeDict = nodeDictGo
				.Union(nodeDictBack
					.Where(x => !nodeDictGo.Any(y => y.Code == x.Code))
				)
				.Distinct()
				.ToDictionary(
					x => x.Code,
					x => x
				);
			this.RoutesreverseNodeList = routesreverseNodeList.ToList();

			this.DefaultStopName = defaultStopName;
			this.LastNodesGo = lastNodesGo.ToList();
			this.LastNodesBack = lastNodesBack.ToList();

			this.NodeDict[defaultStopName].Value = 0;
			this.NodeDictGo[defaultStopName].Value = 0;
			this.NodeDictBack[defaultStopName].Value = 0;

			var routesStops1 = new RoutesStops(this.CurrentWay);
			var routesStops2 = new RoutesStops(this.CurrentWay == RouteWayEnum.Ida ? RouteWayEnum.Vuelta : RouteWayEnum.Ida);
			var routesStops3 = new RoutesStops(this.CurrentWay);
			var routesStops4 = new RoutesStops(this.CurrentWay == RouteWayEnum.Ida ? RouteWayEnum.Vuelta : RouteWayEnum.Ida);
			var routesStops5 = new RoutesStops(this.CurrentWay);
			var routesStops6 = new RoutesStops(this.CurrentWay == RouteWayEnum.Ida ? RouteWayEnum.Vuelta : RouteWayEnum.Ida);
			var routesStops7 = new RoutesStops(this.CurrentWay);
			var routesStops8 = new RoutesStops(this.CurrentWay == RouteWayEnum.Ida ? RouteWayEnum.Vuelta : RouteWayEnum.Ida);
			//routesStops1.Nodes.Add(this.NodeDict[this.CurrentStop]);

			this.RoutesStopsList = new RoutesStopsList();
			this.RoutesStopsList.Routes.Add(routesStops1);
			this.RoutesStopsList.Routes.Add(routesStops2);
			this.RoutesStopsList.Routes.Add(routesStops3);
			this.RoutesStopsList.Routes.Add(routesStops4);
			this.RoutesStopsList.Routes.Add(routesStops5);
			this.RoutesStopsList.Routes.Add(routesStops6);
			this.RoutesStopsList.Routes.Add(routesStops7);
			this.RoutesStopsList.Routes.Add(routesStops8);
		}
		
		// Called for each node in the graph and iterates over its directly
		// connected nodes. The function always handles the node that
		// currently has the highest priority in our queue.
		// So this function checks any directly connected  node and compares
		// the value it currently holds (the shortest path we know to it) is
		// bigger than the distance of the path through the node we're
		// currently checking.
		// If it is, we just found a shorter path to it and we update its
		// 'shortest path' value and also update its previous node to the
		// one we're currently processing.
		// Every directly connected node that we find we also add to the queue
		// (which is sorted by distance), if it's not already in the queue.
		// After we're finished 
		private void CheckNode(PrioQueue queue, string destinationNode)
		{
			// If there are no nodes left to check in our queue, we're done.
			if (queue.Count == 0)
			{
				return;
			}

			foreach (var route in Routes.Where(r => r.From == queue.First.Value.Code))
			{
				// Skip routes to nodes that have already been visited.
				if (!Unvisited.Contains(route.To))
				{
					continue;
				}

				double travelledDistance = NodeDict[queue.First.Value.Code].Value + route.Distance;

				// We only look at nodes we haven't visited yet and we only
				// update the node's values if the distance of the path we're
				// currently checking is shorter than the one we found before.
				if (travelledDistance < NodeDict[route.To].Value)
				{
					NodeDict[route.To].Value = travelledDistance;

					NodeDict[route.To].PreviousNode = NodeDict[queue.First.Value.Code];
				}

				// We don't add the 'to' node to the queue if it has already been
				// visited and we don't allow duplicates.
				if (!queue.HasLetter(route.To))
				{
					queue.AddNodeWithPriority(NodeDict[route.To]);
				}
			}
			Unvisited.Remove(queue.First.Value.Code);
			queue.RemoveFirst();

			CheckNode(queue, destinationNode);
		}
		private void CheckNodeGo(PrioQueue queueGo, string destinationNode)
		{
			// If there are no nodes left to check in our queue, we're done.
			if (queueGo.Count == 0)
				return;

			foreach (var route in RoutesGo.Where(r => r.From == queueGo.First.Value.Code))
			{
				// Skip routes to nodes that have already been visited.
				if (!UnvisitedGo.Contains(route.To))
					continue;

				double travelledDistance = NodeDictGo[queueGo.First.Value.Code].Value + route.Distance;

				// We only look at nodes we haven't visited yet and we only
				// update the node's values if the distance of the path we're
				// currently checking is shorter than the one we found before.
				if (travelledDistance < NodeDictGo[route.To].Value)
				{
					NodeDictGo[route.To].Value = travelledDistance;
					NodeDictGo[route.To].PreviousNode = NodeDictGo[queueGo.First.Value.Code];
				}

				// We don't add the 'to' node to the queue if it has already been
				// visited and we don't allow duplicates.
				if (!queueGo.HasLetter(route.To))
					queueGo.AddNodeWithPriority(NodeDictGo[route.To]);
			}

			UnvisitedGo.Remove(queueGo.First.Value.Code);
			queueGo.RemoveFirst();

			CheckNodeGo(queueGo, destinationNode);
		}
		private void CheckNodeBack(PrioQueue queueBack, string destinationNode)
		{
			// If there are no nodes left to check in our queue, we're done.
			if (queueBack.Count == 0)
			{
				return;
			}

			foreach (var route in RoutesBack.Where(r => r.From == queueBack.First.Value.Code))
			{
				// Skip routes to nodes that have already been visited.
				if (!UnvisitedBack.Contains(route.To))
				{
					continue;
				}

				double travelledDistance = NodeDictBack[queueBack.First.Value.Code].Value + route.Distance;

				// We only look at nodes we haven't visited yet and we only
				// update the node's values if the distance of the path we're
				// currently checking is shorter than the one we found before.
				if (travelledDistance < NodeDictBack[route.To].Value)
				{
					NodeDictBack[route.To].Value = travelledDistance;

					NodeDictBack[route.To].PreviousNode = NodeDictBack[queueBack.First.Value.Code];
				}

				// We don't add the 'to' node to the queue if it has already been
				// visited and we don't allow duplicates.
				if (!queueBack.HasLetter(route.To))
				{
					queueBack.AddNodeWithPriority(NodeDictBack[route.To]);
				}
			}
			UnvisitedBack.Remove(queueBack.First.Value.Code);
			queueBack.RemoveFirst();

			CheckNodeBack(queueBack, destinationNode);
		}
		
		private (List<Node>, double) ReturnlistNodes(string startNode, string destNode)
		{

			if (NodeDict.ContainsKey(destNode))
			{
				List<Node> pathList = new List<Node> { NodeDict[destNode] };

				Node currentNode = NodeDict[destNode];
				while (currentNode != NodeDict[startNode] && currentNode.PreviousNode != null)
				{
					pathList.Add(currentNode.PreviousNode);
					currentNode = currentNode.PreviousNode;
				}

				pathList.Reverse();

				//Frank: Vamos a añadir en la ruta del Bus los nuevos puntos de paso.

				return (pathList, NodeDict[destNode].Value);
			}
			else
			{
				List<Node> pathList = new List<Node>();
				return (pathList, -1);
			}


		}
		private (List<Node>, double) ReturnlistNodesGo(string startNode, string destNode)
		{
			if (NodeDictGo.ContainsKey(destNode))
			{
				List<Node> pathList = new List<Node> { NodeDictGo[destNode] };

				Node currentNode = NodeDictGo[destNode];
				while (currentNode != NodeDictGo[startNode] && currentNode.PreviousNode != null)
				{
					pathList.Add(currentNode.PreviousNode);
					currentNode = currentNode.PreviousNode;
				}

				pathList.Reverse();

				//Frank: Vamos a añadir en la ruta del Bus los nuevos puntos de paso.

				return (pathList, NodeDictGo[destNode].Value);
			}
			else
			{
				List<Node> pathList = new List<Node>();
				return (pathList, -1);
			}


		}
		private (List<Node>, double) ReturnlistNodesBack(string startNode, string destNode)
		{
			if (NodeDictBack.ContainsKey(destNode))
			{
				List<Node> pathList = new List<Node> { NodeDictBack[destNode] };

				Node currentNode = NodeDictBack[destNode];
				while (currentNode != NodeDictBack[startNode] && currentNode.PreviousNode != null)
				{
					pathList.Add(currentNode.PreviousNode);
					currentNode = currentNode.PreviousNode;
				}

				pathList.Reverse();

				//Frank: Vamos a añadir en la ruta del Bus los nuevos puntos de paso.

				return (pathList, NodeDictBack[destNode].Value);
			}
			else
			{
				List<Node> pathList = new List<Node>();
				return (pathList, -1);
			}

		}
		
		public void AddNode(string firstNodeName, RoutesStopsList routesStopsList, int routesStopsIndex, IEnumerable<string> nodeNames, int index = 0)
		{
			if (!nodeNames.Any())
				return;
			
			// Get Route
			var routesStops = routesStopsList.Routes[routesStopsIndex];

			var routes = (routesStops.Way == RouteWayEnum.Ida) ?
				this.RoutesGo :
				this.RoutesBack;
			var nodeDict = (routesStops.Way == RouteWayEnum.Ida) ?
				this.NodeDictGo :
				this.NodeDictBack;
			var lastNode = (
				(routesStops.Way == RouteWayEnum.Ida) ?
				this.LastNodesBack :
				this.LastNodesGo
			).FirstOrDefault();

			// Get Node
			var nodeName = nodeNames.FirstOrDefault();
			if (!nodeDict.ContainsKey(nodeName)) // El nodo no está en el grafo
			{
				// Es necesario hacer el giro
				AddNode(lastNode.Code, routesStopsList, routesStopsIndex + 1, nodeNames, 0);
				return;
			}
			var node = nodeDict[nodeName];
			//var firstNode = nodeDict[firstNodeName];

			if (routesStops.Nodes.Count() == index) // La coleccion está vacía o estamos al final
			{
				(var listFrom, var distanceFrom) = ExecuteDijkstra(routes, nodeDict, firstNodeName, nodeName);
				if (distanceFrom == double.MaxValue) // El nodo no e alcanzable desde el punto inicial
				{
					// Es necesario hacer el giro
					AddNode(lastNode.Code, routesStopsList, routesStopsIndex + 1, nodeNames, 1);
					return;
				}
				else
				{
					// Añadir a la misma colección
					routesStops.Nodes.Add(node);
					nodeNames = nodeNames.Skip(1);
					AddNode(node.Code, routesStopsList, routesStopsIndex, nodeNames, 1);
					return;
				}
			}

			for (int i = index; i < routesStops.Nodes.Count(); i++)
			{
				var nodeFrom = index == 0 ? nodeDict[firstNodeName] : routesStops.Nodes[i-1];
				var nodeTo = routesStops.Nodes[i];

				(var listFrom, var distanceFrom) = ExecuteDijkstra(this.RoutesGo, this.NodeDictGo, nodeFrom?.Code ?? "", nodeName);
				if (distanceFrom == 0) // Ya está en la colección
					return;
				else if (distanceFrom == double.MaxValue) // No se puede ir desde From
				{
					// Es necesario hacer el giro
					AddNode(lastNode.Code, routesStopsList, routesStopsIndex + 1, nodeNames, 0);
					return;
				}
				else if (nodeTo == null) // Está entre el From y el fin de ruta
				{
					// Añadir a la misma colección
					routesStops.Nodes.Add(node);
					nodeNames = nodeNames.Skip(1);
					AddNode(node.Code, routesStopsList, routesStopsIndex, nodeNames, i + 1);
					return;
				}
				
				(var listTo, var distanceTo) = ExecuteDijkstra(this.RoutesGo, this.NodeDictGo, nodeName, nodeTo?.Code ?? "");
				if (distanceTo == 0) // Ya está en la colección
					return;
				else if (distanceTo == double.MaxValue) // No se puede ir hasta To
				{
					// Abanza lista
					AddNode(nodeTo?.Code ?? "", routesStopsList, routesStopsIndex, nodeNames, i + 1);
					return;
				}
				else // Está entre los dos puntos
				{
					// Añadir a la colección
					routesStops.Nodes.Insert(i, node);
					nodeNames = nodeNames.Skip(1);
					AddNode(node.Code, routesStopsList, routesStopsIndex, nodeNames, i + 1);
					return;
				}
			}
			
			{
				(var listFrom, var distanceFrom) = ExecuteDijkstra(routes, nodeDict, nodeName, lastNode.Code);
				if (distanceFrom == double.MaxValue) // El nodo no e alcanzable desde el punto inicial
				{
					// Es necesario hacer el giro
					AddNode(lastNode.Code, routesStopsList, routesStopsIndex + 1, nodeNames, 0);
					return;
				}
				else
				{
					// Añadir a la misma colección
					routesStops.Nodes.Add(node);
					nodeNames = nodeNames.Skip(1);
					AddNode(node.Code, routesStopsList, routesStopsIndex, nodeNames, 1);
					return;
				}
			}
		}
		private (bool, int, int) existStopNodeinRoutes(Node nodeToFind, RoutesStopsList updatedrouteStops)
		{
			foreach (RoutesStops temproutesStops in updatedrouteStops.Routes)
			{
				foreach (Node tempNodesearch in temproutesStops.Nodes)
				{
					if (tempNodesearch.Code == nodeToFind.Code)
					{
						return (true, updatedrouteStops.Routes.IndexOf(temproutesStops), temproutesStops.Nodes.IndexOf(tempNodesearch));
					}
				}

			}

			return (false, 0, 0);
		}
		public List<Node> GetFullItinerary()
		{
			var result = new List<Node>();

			// Add initial stop
			var previousStopName = this.CurrentStopName;

			// Add all routes
			foreach (var routeStops in this.RoutesStopsList.Routes)
			{
				foreach (var routeStop in routeStops.Nodes)
				{
					(var list, var distance) = ExecuteDijkstra(this.Routes, this.NodeDict, previousStopName, routeStop.Code);
					previousStopName = routeStop.Code;

					// Marcar lo que es una parada de ruta
					if (list.Any())
						list.Last().IsSelected = true;

					// Put sense
					foreach (var item in list)
						item.Way = routeStops.Way;

					// Concat lists
					result = ConcatStops(result, list);
				}
			}

			// Add default stop
			{
				(var list, var distance) = ExecuteDijkstra(this.Routes, this.NodeDict, previousStopName, this.DefaultStopName);

				// Put sense
				foreach (var item in list)
					item.Way = RouteWayEnum.Vuelta;

				// Concat lists
				result = ConcatStops(result, list);
			}

			return result;
		}
		public List<Node> ConcatStops(IEnumerable<Node> list1, IEnumerable<Node> list2)
		{
			// Eliminar el fin y inicio de rutas consecutivas si es el mismo punto
			if (list1.Any() && list2.Any() && (list1.Last().Code == list2.First().Code))
				list2 = list2.Skip(1);

			var result = new List<Node>();
			result.AddRange(list1);
			result.AddRange(list2);
			return result;
		}

		// Algoritmo de Dijkstra
		private (List<Node>, double) ExecuteDijkstra(IEnumerable<Route> routes, Dictionary<string, Node> nodeDict, string startNode, IEnumerable<Node> endNodes)
		{
			foreach (var node in endNodes)
			{
				(var nodes, var distance) = ExecuteDijkstra(routes, nodeDict, startNode, node.Code);
				if (distance > 0)
					return (nodes, distance);
			}

			return (new List<Node>(), -1);
		}
		private (List<Node>, double) ExecuteDijkstra(IEnumerable<Route> routes, Dictionary<string, Node> nodeDict, string startNode, string endNode)
		{
			if (startNode == "")
				return (new List<Node>(), double.MaxValue);
			if (endNode == "")
				return (new List<Node>(), double.MaxValue);
			if (!nodeDict.ContainsKey(startNode))
				return (new List<Node>(), double.MaxValue);
			if (!nodeDict.ContainsKey(endNode))
				return (new List<Node>(), double.MaxValue);

			// Initialize values
			foreach (var node in nodeDict)
			{
				node.Value.Value = double.MaxValue;
				node.Value.PreviousNode = null;
			}
			nodeDict[startNode].Value = 0;

			// Create Queue
			var queue = new PrioQueue();
			queue.AddNodeWithPriority(nodeDict[startNode]);

			// Execute algorithm
			var nodes = CheckNode(routes, queue, endNode, nodeDict);
			var result = ReturnListNodes(nodes, startNode, endNode);
			return result;
		}
		private HashSet<string> InitGraph(IEnumerable<Route> routes)
		{
			HashSet<string> unvisited = new HashSet<string>();

			foreach (var route in routes)
			{
				unvisited.Add(route.From);
				unvisited.Add(route.To);

				//RoutesGo.Add(new Route(route.From, route.To, route.Distance));
			}

			return unvisited;
		}
		private Dictionary<string, Node> CheckNode(IEnumerable<Route> routes, PrioQueue queue, string destinationNode, Dictionary<string, Node> nodeDict, HashSet<string> unvisited = null)
		{
			if (nodeDict == null)
				nodeDict = new Dictionary<string, Node>();

			if (unvisited == null)
				unvisited = InitGraph(routes);

			// If there are no nodes left to check in our queue, we're done.
			if (queue.Count == 0)
				return nodeDict;

			foreach (var route in routes.Where(r => r.From == queue.First.Value.Code))
			{
				// Skip routes to nodes that have already been visited.
				if (!unvisited.Contains(route.To))
					continue;

				double travelledDistance = nodeDict[queue.First.Value.Code].Value + route.Distance;

				// We only look at nodes we haven't visited yet and we only
				// update the node's values if the distance of the path we're
				// currently checking is shorter than the one we found before.
				if (travelledDistance < nodeDict[route.To].Value)
				{
					nodeDict[route.To].Value = travelledDistance;
					nodeDict[route.To].PreviousNode = nodeDict[queue.First.Value.Code];
				}

				// We don't add the 'to' node to the queue if it has already been
				// visited and we don't allow duplicates.
				if (!queue.HasLetter(route.To))
					queue.AddNodeWithPriority(NodeDict[route.To]);
			}

			unvisited.Remove(queue.First.Value.Code);
			queue.RemoveFirst();

			return CheckNode(routes, queue, destinationNode, nodeDict, unvisited);
		}
		private (List<Node>, double) ReturnListNodes(Dictionary<string, Node> nodeDict, string startNode, string destNode)
		{
			if (!nodeDict.ContainsKey(destNode))
				return (new List<Node>(), -1);
			
			var pathList = new List<Node> { nodeDict[destNode] };

			var currentNode = nodeDict[destNode];
			//if (currentNode.Value == double.MaxValue)
			//	return (new List<Node>(), -1);
			while (currentNode != nodeDict[startNode] && currentNode.PreviousNode != null)
			{
				pathList.Add(currentNode.PreviousNode);
				currentNode = currentNode.PreviousNode;
			}
			pathList.Reverse();

			return (pathList, nodeDict[destNode].Value);
		}
	}
}
