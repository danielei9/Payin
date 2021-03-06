using PayIn.Common;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class AccountLineGetByAccountsArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		public int? ServiceCardId { get; set; }
		public int? EventId { get; set; }
		public int? ServiceConcessionId { get; set; }
		public TicketLineType? Type { get; set; }

		#region Constructors
		public AccountLineGetByAccountsArguments(string filter, XpDate since, XpDate until, int? serviceCardId, int? eventId, int? serviceConcessionId, TicketLineType? type)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;

            ServiceCardId = serviceCardId;

			EventId = eventId;
			ServiceConcessionId = serviceConcessionId;
			Type = type;
		}
        #endregion Constructors
    }
}
