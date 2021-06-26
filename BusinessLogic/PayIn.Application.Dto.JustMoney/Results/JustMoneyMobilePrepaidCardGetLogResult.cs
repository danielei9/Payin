using PayIn.Infrastructure.JustMoney.Enums;
using System.Collections.Generic;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class JustMoneyMobilePrepaidCardGetLogResult
    {
		public int Id { get; set; }
		public decimal AvailableBalance { get; set; }
		public CardStatus CardStatus { get; set; }

		public IEnumerable<JustMoneyMobilePrepaidCardGetLogResult_Transaction> Transactions { get; set; } = new List<JustMoneyMobilePrepaidCardGetLogResult_Transaction>();
	}
}
