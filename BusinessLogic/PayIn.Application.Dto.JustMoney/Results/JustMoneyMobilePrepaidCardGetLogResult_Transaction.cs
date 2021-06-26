using Xp.Common;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class JustMoneyMobilePrepaidCardGetLogResult_Transaction
	{
        public XpDateTime Date { get; set; }
		public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
