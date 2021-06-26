using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "Activate")]
	public class TicketActivateHandler :
		IServiceBaseHandler<TicketActivateArguments>
	{
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public TicketActivateHandler(
			IEntityRepository<Ticket> ticketRepository,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			TicketRepository = ticketRepository;

			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<TicketActivateArguments>.ExecuteAsync(TicketActivateArguments arguments)
		{
			var now = DateTime.Now;
			var Tickets = (await TicketRepository.GetAsync("Concession.Concession.Supplier","PaymentUser"))
			.Where(x => x.Since.Year == now.Year &&
						x.Since.Month == now.Month &&
						x.Since.Day == now.Day &&
						x.PaymentUser != null
			);
			var LastDayTickets = (await TicketRepository.GetAsync("Concession.Concession.Supplier","PaymentUser"))
				.Where(x => (x.Since.Year == now.Year && x.Since.Month == now.Month && x.Since.Day + 1 == now.Day));


			foreach (var ticket in Tickets){
					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.TicketActive,
					message: TicketResources.TicketActive.FormatString(ticket.Amount, ticket.SupplierName, ticket.Until),
					referenceId: ticket.Id,
					referenceClass: "Ticket",
					senderLogin: ticket.Concession.Concession.Supplier.Login,
					receiverLogin: ticket.PaymentUser.Login
					));
					ticket.State = TicketStateType.Active;
			}

			foreach (var ticket in LastDayTickets)
			{
				await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.TicketLastDay,
				message: TicketResources.TicketLastDay.FormatString(ticket.Amount, ticket.SupplierName, ticket.Until.Hour),
				referenceId: ticket.Id,
				referenceClass: "Ticket",
				senderLogin: ticket.Concession.Concession.Supplier.Login,
				receiverLogin: ticket.PaymentUser.Login
				));
			}

			return null;
		}
		#endregion ExecuteAsync
	}	
}