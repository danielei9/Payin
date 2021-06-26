using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class TicketGetAllResult
	{
		public XpDateTime Date { get; set; }
		public int Id { get; set; }
        public long? Uid { get; set; }
        public string UidsText { get; set; }
        public string Title { get; set; }
		public decimal Amount { get; set; }
		public decimal PayedAmount { get; set; }
		public TicketStateType State { get; set; }
		public bool HasShipment { get; set; }
		public int? TemplateId { get; set; }
		public bool HasText { get; set; }
		public string EventName { get; set; }
        public string ConcessionName { get; set; }
		public string PaymentUserName { get; set; }
	}
}
