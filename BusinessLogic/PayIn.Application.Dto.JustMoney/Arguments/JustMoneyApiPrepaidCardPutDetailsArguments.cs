using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardPutDetailsArguments : IArgumentsBase
	{
        public IEnumerable<JustMoneyApiPrepaidCardPutDetailsArguments_Cards> Cards { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardPutDetailsArguments(IEnumerable<JustMoneyApiPrepaidCardPutDetailsArguments_Cards> cards)
        {
            Cards = cards;
		}
        #endregion Constructors
    }
}
