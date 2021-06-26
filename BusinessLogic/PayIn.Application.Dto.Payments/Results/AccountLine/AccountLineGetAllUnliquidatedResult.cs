using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class AccountLineGetAllUnliquidatedResult 
	{
		public int? Id { get; set; }
		public XpDateTime Date { get; set; }
		public decimal Amount { get; set; }
        public decimal? Commission { get; set; }
        public int TicketConcessionId { get; set; }
        public string TicketConcessionName { get; set; }
		public int? EventId { get; set; }
		public string EventName { get; set; }
		public string Name { get; set; }
        public string Title { get; set; }
		public PaymentState State { get; set; }
	}
}
