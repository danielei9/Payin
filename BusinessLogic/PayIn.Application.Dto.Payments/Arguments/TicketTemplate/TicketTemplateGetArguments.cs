using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.TicketTemplate
{
	public class TicketTemplateGetArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int TicketId { get; set; }

		#region Constructors
		public TicketTemplateGetArguments(int id, int ticketId)
		{
			Id = id;
			TicketId = ticketId;
		}
		#endregion Constructors
	}
}
