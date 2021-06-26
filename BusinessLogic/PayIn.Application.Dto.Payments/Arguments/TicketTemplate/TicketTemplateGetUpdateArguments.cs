using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.TicketTemplate
{
	public class TicketTemplateGetUpdateArguments : IArgumentsBase
	{
		public int TicketId { get; set; }

		#region Constructors
		public TicketTemplateGetUpdateArguments(int ticketId)
		{
			TicketId = ticketId;
		}
		#endregion Constructors
	}
}
