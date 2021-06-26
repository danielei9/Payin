using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketGetSystemCardArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int EventId { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		#region Constructors
		public TicketGetSystemCardArguments(string filter, int eventId, XpDate since, XpDate until) 
		{
			Filter = filter ?? "";
			EventId = eventId;
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}