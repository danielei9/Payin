using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.JustMoney.Results
{
	public class JustMoneyApiPrepaidCardGetAllResultBase : ResultBase<JustMoneyApiPrepaidCardGetAllResult>
	{
		public IEnumerable<JustMoneyApiPrepaidCardGetCardsResult> Cards { get; set; }
	}
}
