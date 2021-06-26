namespace PayIn.Application.Dto.Bus.Results
{
	public class BusMobileStopGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Radius { get; set; }
	}
}
