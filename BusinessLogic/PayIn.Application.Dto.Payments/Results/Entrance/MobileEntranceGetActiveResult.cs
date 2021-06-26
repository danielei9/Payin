namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileEntranceGetActiveResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public long Code { get; set; }
		public string EntranceTypeName { get; set; }
		public string EventName { get; set; }
		public decimal? Amount { get; set; }
	}
}
