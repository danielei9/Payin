using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardEnableDisableArguments : IArgumentsBase
	{
		public IEnumerable<JustMoneyApiPrepaidCardEnableDisableArguments_Card> Cards { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardEnableDisableArguments(IEnumerable<JustMoneyApiPrepaidCardEnableDisableArguments_Card> cards)
		{
			Cards = cards;
		}
		#endregion Constructors
	}
}
