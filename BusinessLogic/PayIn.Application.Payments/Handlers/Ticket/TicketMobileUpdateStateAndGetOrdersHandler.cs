using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "UpdateStateAndGetOrders")]
	public class TicketMobileUpdateStateAndGetOrdersHandler : IServiceBaseHandler<TicketMobileUpdateStateAndGetOrdersArguments>
	{
		[Dependency] public MobileTicketGetOrdersHandler GetHandler { get; set; }
        [Dependency] public TicketMobileUpdateStateHandler UpdateHandler { get; set; }
		[Dependency] public IEntityRepository<Payment> PaymentRepository { get; set; }
        [Dependency] public IUnitOfWork UnitOfWork { get; set; }
        [Dependency] public IPushService PushService { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketMobileUpdateStateAndGetOrdersArguments arguments)
		{
			var ticket = await UpdateHandler.ExecuteAsync(arguments) as Ticket;

			var tarjets = (await PaymentRepository.GetAsync())
				.Where(x => x.Id == x.Ticket.Id)
				.Select(x => x.UserLogin);

			await UnitOfWork.SaveAsync();

			if ((ticket.Type == TicketType.Order) || (ticket.Type == TicketType.Shipment))
                await PushService.SendNotification(tarjets, NotificationType.OrderChangeState, NotificationState.Actived, "El pedido ha cambiado de estado", "ticket", ticket.Id.ToString(), ticket.Id);

			return await GetHandler.ExecuteAsync(new MobileTicketGetOrdersArguments());
		}
		#endregion ExecuteAsync
	}
}
