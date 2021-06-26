using System.Collections.Generic;
using System.Linq;
using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketSellArguments : ICreateArgumentsBase<Ticket>
	{
		public int? CardId { get; set; }
		public IEnumerable<TicketSellArguments_EntranceType> EntranceTypes { get; set; }

		#region Constructors
		public TicketSellArguments(int? cardId, IEnumerable<TicketSellArguments_EntranceType> entranceTypes)
		{
			CardId = cardId;
			EntranceTypes = entranceTypes
				.Where(x => x.Selected == true)
				.ToList();
		}
		#endregion Constructors

	}
}