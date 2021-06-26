using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class TicketMobileGetPayResult_Recharge
	{
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public string UserName { get; set; }
		public string PaymentMediaName { get; set; }
		public XpDateTime Date { get; set; }
	}
}
