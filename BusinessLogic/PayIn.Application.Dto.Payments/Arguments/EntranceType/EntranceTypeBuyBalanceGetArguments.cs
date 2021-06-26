using System.Collections.Generic;
using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class EntranceTypeBuyBalanceGetArguments : IArgumentsBase
    {
		public int CardId { get; set; }

		#region Constructors
		public EntranceTypeBuyBalanceGetArguments(int cardId)
		{
			CardId = cardId;
		}
        #endregion Constructors
    }
}
