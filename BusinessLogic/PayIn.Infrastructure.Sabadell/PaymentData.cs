using PayIn.Common;

namespace PayIn.Infrastructure.Sabadell
{
	public class PaymentData
	{
		public int? PaymentMediaId { get; set; }
		public int? PublicPaymentMediaId { get; set; }
		public int PublicTicketId { get; set; }
		public int PublicPaymentId { get; set; }
		public PaymentMediaCreateType PaymentMediaCreateType { get; set; }
	}
}
