using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class TicketGraphResult
	{
		public decimal TicketAmount { get; set; }
		public decimal ChargedAmount { get; set; }
		public decimal RefundedAmount { get; set; }
		public XpDate Day { get; set; }
	}
}
