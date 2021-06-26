using PayIn.Common;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class AccountLineGetByCreditCardsArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		public int? EventId { get; set; }
		public int? ServiceConcessionId { get; set; }
		public TicketLineType? Type { get; set; }

		#region Constructors
		public AccountLineGetByCreditCardsArguments(string filter, XpDate since, XpDate until, int? eventId, int? serviceConcessionId, TicketLineType? type)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;

			EventId = eventId;
			ServiceConcessionId = serviceConcessionId;
			Type = type;
		}
		#endregion Constructors
	}
}
