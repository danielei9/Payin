namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileCreateArguments_TicketLine
	{
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public decimal Quantity { get; set; }
	}
}
