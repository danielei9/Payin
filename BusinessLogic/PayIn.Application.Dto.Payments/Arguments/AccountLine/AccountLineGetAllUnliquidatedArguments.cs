using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class AccountLineGetAllUnliquidatedArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }
		public bool FilterByEvent { get; set; }
		public int? EventId { get; set; }
		public int? ConcessionId { get; set; }

		public AccountLineGetAllUnliquidatedArguments(string filter, XpDate since, XpDate until, bool filterByEvent, int? concessionId, int? eventId)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;
			FilterByEvent = filterByEvent;
			EventId = eventId;
			ConcessionId = concessionId;
		}
	}
}
