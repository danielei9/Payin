using System.Collections.Generic;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class JustMoneyApiPrepaidCardGetLogResult
    {
		public int Id { get; set; }
		public string Alias { get; set; }

        public decimal Balance { get; set; }
        public string BalanceString { get; set; }

        public IEnumerable<JustMoneyApiPrepaidCardGetLogResult_Transaction> Transactions { get; set; } = new List<JustMoneyApiPrepaidCardGetLogResult_Transaction>();
	}
}
