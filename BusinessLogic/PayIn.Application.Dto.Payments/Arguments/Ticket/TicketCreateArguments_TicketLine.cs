namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketCreateArguments_TicketLine
	{
		public string Reference { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public decimal Quantity { get; set; }
	}
}
