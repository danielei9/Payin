using PayIn.Infrastructure.JustMoney.Enums;
using System.Collections.Generic;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class MainGetAllResult
	{
		public int Id { get; set; }
		public decimal Balance { get; set; }
		public CardStatus Status { get; set; }
	}
}
