using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int EventId { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		#region Constructors
		public TicketGetAllArguments(string filter, int eventId, XpDate since, XpDate until) 
		{
			Filter = filter ?? "";
			EventId = eventId;
			Since = since ?? new XpDate(XpDate.MinValue);
			Until = until ?? new XpDate(XpDate.MaxValue);
		}
		#endregion Constructors
	}
}