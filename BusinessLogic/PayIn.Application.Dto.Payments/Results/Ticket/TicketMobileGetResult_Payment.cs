using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class TicketMobileGetResult_Payment
	{
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public string UserName { get; set; }
		public string PaymentMediaName { get; set; }
		public XpDateTime Date { get; set; }
		public PaymentState State { get; set; }
		public bool CanBeReturned { get; set; }
	}
}
