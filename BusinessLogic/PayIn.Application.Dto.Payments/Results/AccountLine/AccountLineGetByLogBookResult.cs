using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class AccountLineGetByLogBookResult 
	{
		public int? Id { get; set; }
        public string TypeName { get; set; }
        public XpDateTime Date { get; set; }
        public string EventName { get; set; }
        public string ConcessionName { get; set; }
        public string LiquidationConcessionName { get; set; }

        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }

        public IEnumerable<AccountLineGetByLogBookResultLine> Lines { get; set; }
    }
}
