using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("Ticket", "Update")]
	public class TicketUpdateHandler :
		IServiceBaseHandler<TicketUpdateArguments>
	{
		private readonly IEntityRepository<Ticket> Repository;
		
		#region Contructors
		public TicketUpdateHandler(
			IEntityRepository<PayIn.Domain.Payments.Ticket> repository
		)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

		}
		#endregion Contructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TicketUpdateArguments>.ExecuteAsync(TicketUpdateArguments arguments)
		{
			var ticket = await Repository.GetAsync(arguments.Id, "Concession.Concession");
			if (ticket == null)
				throw new ArgumentNullException("id");
			if (ticket.Concession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(TicketResources.UpdateNonActiveConcessionException, "id");
			
			ticket.Date = arguments.Date.Value.ToUTC();
			ticket.Reference = arguments.Reference;
			ticket.Amount = arguments.Amount;
			ticket.State = arguments.State;

			return ticket;
		}
		#endregion ExecuteAsync
	}
}
