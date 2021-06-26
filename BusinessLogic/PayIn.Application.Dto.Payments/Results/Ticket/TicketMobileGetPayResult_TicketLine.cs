namespace PayIn.Application.Dto.Payments.Results
{
	public class TicketMobileGetPayResult_TicketLine
	{
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public string Title { get; set; }
		public string Reference { get; set; }
		public decimal Quantity { get; set; }
	}
}
