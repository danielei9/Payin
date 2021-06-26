namespace PayIn.Application.Bus.Services
{
	public class RouteRequestStop
	{
		public int Id { get; set; }
		public bool FromVisited { get; set; }
		public string FromCode { get; set; }
		public string ToCode { get; set; }
	}
}
