namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiSensorGetAllResult_Price
	{
		public int Id { get; set; }
		public decimal? Price { get; set; }
		public decimal? PowerContract { get; set; }
		public decimal PowerContractFactor { get; set; }
		public string PowerContractUnit { get; set; }
		public string Color { get; set; }
	}
}
