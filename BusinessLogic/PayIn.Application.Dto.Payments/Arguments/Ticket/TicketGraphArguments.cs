using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketGraphArguments : IArgumentsBase
    {
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		public TicketGraphArguments(XpDate since, XpDate until) 
		{
			Since = since;
			Until = until;
		}
    }
}
