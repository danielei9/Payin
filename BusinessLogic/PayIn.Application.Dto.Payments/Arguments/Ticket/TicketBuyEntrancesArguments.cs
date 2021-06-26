using System.Collections.Generic;
using System.Linq;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class TicketBuyEntrancesArguments : IArgumentsBase
    {
		public int?                                                  CardId            { get; set; }
		public long?                                                 Uid           { get; set; }
        public IEnumerable<TicketBuyEntrancesArguments_EntranceType> EntranceTypes { get; set; }

        #region Constructors
        public TicketBuyEntrancesArguments(int? cardId, long? uid, IEnumerable<TicketBuyEntrancesArguments_EntranceType> entranceTypes)
		{
            CardId = cardId;
			Uid = uid;
            EntranceTypes = entranceTypes
                .Where(x => x.Selected == true)
                .ToList();
		}
		#endregion Constructors
	}
}