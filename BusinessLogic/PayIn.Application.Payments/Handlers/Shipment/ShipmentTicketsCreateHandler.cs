using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ShipmentTicketsCreateHandler :
		IServiceBaseHandler<ShipmentTicketsCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<Shipment> ShipmentRepository;	
	    private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IEntityRepository<PaymentUser> PaymentUserRepository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public ShipmentTicketsCreateHandler(
			ISessionData sessionData,
			IUnitOfWork unitOfWork,
			IEntityRepository<Shipment> shipmentRepository,
			IEntityRepository<Ticket> ticketRepository,
			IEntityRepository<PaymentUser> paymentUserRepository,
		    ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (shipmentRepository == null) throw new ArgumentNullException("shipmentRepository");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (paymentUserRepository == null) throw new ArgumentNullException("paymentUserRepository");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

			SessionData = sessionData;
			UnitOfWork = unitOfWork;
			ShipmentRepository = shipmentRepository;
			TicketRepository = ticketRepository;
			PaymentUserRepository = paymentUserRepository;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ShipmentTicketsCreateArguments>.ExecuteAsync(ShipmentTicketsCreateArguments arguments)
		{
			var item = (await ShipmentRepository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.ShipmentId)
				.FirstOrDefault();
			var now = DateTime.Now;

			foreach (var id in arguments.PaymentUserIds)
			{
				var paymentUser = (await PaymentUserRepository.GetAsync("tickets"))
					.Where(x => x.Id == id)
					.FirstOrDefault();
				if (paymentUser.Tickets.Any(x => x.ShipmentId == item.Id && x.State != TicketStateType.Cancelled))
					throw new Exception(ShipmentResources.AddUserException.FormatString(paymentUser.Login));

				if (paymentUser.Tickets.Any(x => x.ShipmentId == item.Id && x.State == TicketStateType.Cancelled))
				{
					var ticketCreated = item.Tickets.Where(x => x.PaymentUser == paymentUser).FirstOrDefault();
					ticketCreated.State = TicketStateType.Preactive;

					if (ticketCreated.Since.Year == now.Year &&
						ticketCreated.Since.Month == now.Month &&
						ticketCreated.Since.Day == now.Day)
					{
						await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
						type: NotificationType.TicketActiveToday,
						message: TicketResources.TicketActiveToday.FormatString(ticketCreated.Amount, ticketCreated.SupplierName, ticketCreated.Since.TimeOfDay, ticketCreated.Until),
						referenceId: ticketCreated.Id,
						referenceClass: "Ticket",
						senderLogin: ticketCreated.Concession.Concession.Supplier.Login,
						receiverLogin: ticketCreated.PaymentUser.Login
						));
						ticketCreated.State = TicketStateType.Active;
					}
					continue;
				}

				var ticket = new Ticket
				{
					Amount = item.Amount,
					Reference = item.Name,
					Since = item.Since,
					Until = item.Until,
					Concession = item.Concession,
					ConcessionId = item.ConcessionId,
					Shipment = item,
					ShipmentId = item.Id,
					PaymentUser = paymentUser,
					PaymentUserId = paymentUser.Id,
					PaymentWorker = null ,
					State = TicketStateType.Preactive,
					Date = now,
					SupplierName = item.Concession.Concession.Supplier.Name,
					TaxName = item.Concession.Concession.Supplier.TaxName,
					TaxNumber = item.Concession.Concession.Supplier.TaxNumber,
					TaxAddress = item.Concession.Concession.Supplier.TaxAddress,
					TextUrl = "",
					PdfUrl = "",
					Type = TicketType.Shipment
				};
				ticket.Lines.Add(new TicketLine
				{
					Title = item.Name,
					Amount = item.Amount,
					Quantity = 1,
					TicketId = ticket.Id,
					Type = TicketLineType.Buy
				});
				await TicketRepository.AddAsync(ticket);
				await UnitOfWork.SaveAsync();

				if (ticket.Since.Year == now.Year &&
				    ticket.Since.Month == now.Month &&
					ticket.Since.Day == now.Day)
				{
					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
						type: NotificationType.TicketActiveToday,
						message: TicketResources.TicketActiveToday.FormatString(ticket.Amount, ticket.SupplierName, ticket.Since.TimeOfDay, ticket.Until), 
						referenceId: ticket.Id,
						referenceClass: "Ticket",
						senderLogin: ticket.Concession.Concession.Supplier.Login,
						receiverLogin: ticket.PaymentUser.Login
					));
					ticket.State = TicketStateType.Active;
				}
			}
			return null;
		}
		#endregion ExecuteAsync
	}
}

