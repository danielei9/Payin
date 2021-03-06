using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class AccountLineGetByCreditCardsResult 
	{
		public int? Id { get; set; }
        public XpDateTime Date { get; set; }
        public TicketLineType Type { get; set; }
        public string TypeName { get; set; }
        public string EventName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMedia { get; set; }
    }
}
