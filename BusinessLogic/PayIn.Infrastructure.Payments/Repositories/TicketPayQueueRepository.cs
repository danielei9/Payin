using PayIn.Application.Dto.Arguments.Ticket;
using Xp.Infrastructure.Azure;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class TicketPayQueueRepository : AzureQueueRepository<TicketPayArguments>
	{
		#region Contructors
		public TicketPayQueueRepository()
			: base("TransferStorageConnectionString", "ticket-pay")
		{
		}
		#endregion Contructors
	}
}
