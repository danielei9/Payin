using System.Collections.Generic;
using System.Linq;
using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketGiveEntrancesArguments : ICreateArgumentsBase<Ticket>
	{
		public int? CardId { get; set; }
		public IEnumerable<TicketGiveEntrancesArguments_EntranceType> EntranceTypes { get; set; }
		
		#region Constructors
		public TicketGiveEntrancesArguments(int? cardId, IEnumerable<TicketGiveEntrancesArguments_EntranceType> entranceTypes)
		{
			CardId = cardId;
			EntranceTypes = entranceTypes
				.Where(x => x.Selected == true)
				.ToList();
		}
		#endregion Constructors
	}
}