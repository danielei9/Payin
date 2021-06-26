using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("TransportOperation", "RefundPayed")]
	[XpAnalytics("TransportOperation", "RefundPayed")]
	public class PublicTransportOperationRefundHandler :
		IServiceBaseHandler<PublicTransportOperationRefundArguments>
	{
		[Dependency] public IEntityRepository<TransportOperation> TransportOperationRepository { get; set; }
        [Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public ServerService ServerService { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(PublicTransportOperationRefundArguments arguments)
		{
            var now = DateTime.UtcNow;

            var operation = (await TransportOperationRepository.GetAsync(arguments.Id, "Price.Title", "Ticket.Payments"));
            if (operation == null)
                throw new ArgumentNullException(nameof(PublicTransportOperationRefundArguments.Id));
            if (operation.OperationType != OperationType.Recharge)
                throw new ApplicationException("Solo se pueden devolver operaciones de recarga");
            if (operation.ConfirmationDate != null)
                throw new ApplicationException("No se puede devolver una operación confirmada");
            if (operation.Ticket.Type != TicketType.NotPayable)
                throw new ApplicationException("Solo se pueden devolver operaciones pagadas externamente");
            if (operation.Ticket.State != TicketStateType.Active)
                throw new ApplicationException("Solo se pueden devolver operaciones con pagos no devueltos");

            // Cancelando ticket
            operation.Ticket.State = TicketStateType.Cancelled;

            // Get Pago
            var rechargePayment = operation.Ticket.Payments
                .FirstOrDefault();

            // Crear devolución
            var payment = new Payment(
                operation.Ticket,
                -1 * operation.Ticket.Amount,
                0,
                now,
                userName: SessionData.Name,
                userLogin: SessionData.Login,
                externalLogin: operation.Ticket.ExternalLogin,
                taxNumber: SessionData.TaxNumber,
                taxName: SessionData.TaxName,
                taxAddress: SessionData.TaxAddress,
                authorizationCode: arguments.AuthorizationCode,
				commerceCode: arguments.CommerceCode,
                refundFrom: rechargePayment
            );
            operation.Ticket.Payments.Add(payment);

            operation.OperationType = OperationType.Refund;
            operation.ConfirmationDate = now;

            #region Confirmar Carga y Devolución
            if (operation.Price != null)
            {
                try
                {
                    operation.ScriptResponse = (await ServerService.Refund(operation, payment, "", now))
                        .ToJson();
                }
                catch (Exception e)
                {
                    operation.Error = e.Message;
                }
            }
            #endregion Confirmar Carga y Devolución	

            return operation;
		}
		#endregion ExecuteAsync
	}
}