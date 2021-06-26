using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class AccountLineGetByEntranceTypesResult 
	{
		public int? Id { get; set; }
        public XpDateTime Date { get; set; }
        public TicketLineType Type { get; set; }
        public string TypeName { get; set; }
        public string EventName { get; set; }
        public decimal Amount { get; set; }
        public string ConcessionName { get; set; }
        public string EntranceTypeName { get; set; }
    }
}
