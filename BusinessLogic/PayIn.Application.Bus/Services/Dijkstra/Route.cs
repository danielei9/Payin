namespace PayIn.Application.Bus.Services.Dijkstra
{
	public class Route
	{
		public string From { get; private set; }
		public string To { get; private set; }
		public double Distance { get; private set; }
		public decimal? Value { get; private set; }

		public Route(string from, string to, double distance, decimal? value)
		{
			this.From = from;
			this.To = to;
			this.Distance = distance;
			this.Value = value;
		}
	}
}
