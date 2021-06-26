namespace PayIn.Application.Dto.Payments.Results
{
	public class ProductGetAllByDashboardResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal? Price { get; set; }
		public int LastDay { get; set; }
		public int LastWeek { get; set; }
		public int LastMonth { get; set; }
		public int LastYear { get; set; }
	}
}
