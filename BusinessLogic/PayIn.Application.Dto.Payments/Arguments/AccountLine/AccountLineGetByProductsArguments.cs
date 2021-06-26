using PayIn.Common;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class AccountLineGetByProductsArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		public int? ProductId { get; set; }

		public int? EventId { get; set; }
		public int? ServiceConcessionId { get; set; }
		public TicketLineType? Type { get; set; }

        #region Constructors
        public AccountLineGetByProductsArguments(string filter, XpDate since, XpDate until, int? productId, int? eventId, int? serviceConcessionId, TicketLineType? type)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;

            ProductId = productId;

			EventId = eventId;
			ServiceConcessionId = serviceConcessionId;
			Type = type;
		}
		#endregion Constructors
	}
}
