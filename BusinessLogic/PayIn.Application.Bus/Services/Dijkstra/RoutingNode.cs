namespace PayIn.Application.Bus.Services.Dijkstra
{
	public class RoutingNode
	{
		//public NodeEnum NodeName { get; private set; }
		public string NodeName { get; set; }

		public string Nodelongname { get; private set; }
		public NodeType Type { get; private set; }

		public RoutingNode(string nodeName, NodeType type, string nodelongname)
		{
			this.NodeName = nodeName;
			this.Type = type;
			this.Nodelongname = nodelongname;
		}
	}
}
