using System.Collections.Generic;
using System.Linq;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class TicketBuyManyEntrancesArguments : IArgumentsBase
    {
		public int ConcessionId { get; set; }
		public int															ServiceCardId	{ get; set; }
        public IEnumerable<TicketBuyManyEntrancesArguments_EntranceType>	EntrancesType	{ get; set; }

        #region Constructors
        public TicketBuyManyEntrancesArguments(int concessionId, int serviceCardId, IEnumerable<TicketBuyManyEntrancesArguments_EntranceType> entrancesType)
		{
			ConcessionId = concessionId;
            ServiceCardId = serviceCardId;
			EntrancesType = entrancesType;
		}
		#endregion Constructors
	}
}