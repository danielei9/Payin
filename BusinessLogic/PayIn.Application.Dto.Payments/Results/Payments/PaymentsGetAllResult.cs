using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentsGetAllResult
	{
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public XpDateTime Date { get; set; }
        public long? Uid { get; set; }
        public string UidText { get; set; }
        public int? Seq { get; set; }
		public string TaxName { get; set; }
		public int TicketId { get; set; }
		public decimal Payin { get; set; }
		public decimal TicketAmount { get; set; }	
		//public string Ticket { get; set; }
		//public decimal Total { get; set; }
		public PaymentState State { get; set; }
		//public string StateName { get; set; }
		public int? RefundFromId { get; set; }
		public XpDateTime RefundFromDate { get; set; }
		public int? RefundToId { get; set; }
		public XpDateTime RefundToDate { get; set; }
	}
}
