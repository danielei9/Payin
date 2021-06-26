using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentsGetAllByLiquidationResult
	{
		public int              Id                   { get; set; }
		public decimal          Amount               { get; set; }
		public decimal          Commission           { get; set; }
		public XpDateTime       Date                 { get; set; }
		public string           TaxName              { get; set; }
		public decimal          TicketAmount         { get; set; }
		public PaymentState     State                { get; set; }
		public int              TicketId             { get; set; }
        public string           UidText              { get; set; }
        public int?             Seq                  { get; set; }
        public long?            Uid                  { get; set; }
        public string           EventName            { get; set; }
        public string           TicketConcessionName { get; set; }
        public string           Title                { get; set; }
    }
}
