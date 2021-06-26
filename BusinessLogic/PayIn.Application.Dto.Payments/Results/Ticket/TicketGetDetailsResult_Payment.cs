using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class TicketGetDetailsResult_Payment
	{
		public int Id { get; set; }
		public XpDateTime Date { get; set; }
		public PaymentState State { get; set; }
		public string StateAlias { get; set; }
		public string TaxName { get; set; }
		public string NumberHash { get; set; }
		public decimal Amount { get; set; }
		public decimal Payin { get; set; }
		public decimal Total { get; set; }
		public int? RefundFromId { get; set; }
		public XpDateTime RefundFromDate { get; set; }
		public int? RefundToId { get; set; }
		public XpDateTime RefundToDate { get; set; }
	}
}
