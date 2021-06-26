namespace PayIn.Application.Bus.Services.Dijkstra
{
	public class Node
	{
		public string Code { get; private set; }
		public RouteWayEnum Way { get; set; }
		public double Value { get; set; }
		public Node PreviousNode { get; set; }
		public bool IsSelected { get; set; } = false;

		public Node(string code, double value = int.MaxValue, Node previousNode = null)
		{
			this.Code = code;
			this.Value = value;
			this.PreviousNode = previousNode;
		}
	}
}
