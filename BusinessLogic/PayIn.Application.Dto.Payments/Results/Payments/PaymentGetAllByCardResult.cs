using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentGetAllByCardResult
	{
		public int Id { get; set; }
		public int TicketId { get; set; }
		public XpDateTime Date { get; set; }
		public string ConcessionName { get; set; }
		public decimal TicketAmount { get; set; }
		public decimal PaymentAmount { get; set; }
	}
}
