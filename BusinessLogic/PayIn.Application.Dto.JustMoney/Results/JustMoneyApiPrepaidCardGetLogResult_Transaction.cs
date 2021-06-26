using System;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class JustMoneyApiPrepaidCardGetLogResult_Transaction
	{
        public DateTime Date { get; set; }
		public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
