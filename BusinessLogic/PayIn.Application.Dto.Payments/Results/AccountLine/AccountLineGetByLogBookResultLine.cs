using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class AccountLineGetByLogBookResultLine
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public bool Liquidated { get; set; }
        public bool Paid { get; set; }
        public string ConcessionName { get; set; }
        public string Uid { get; set; }
        public string UidText { get; set; }
        public AccountLineType Type { get; set; }
        public string TypeName { get; set; }
    }
}
