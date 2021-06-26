using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Payment", "Refund")]
	public class PaymentRefundHandler :
		IServiceBaseHandler<PaymentRefundArguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
        [Dependency] public IUnitOfWork UnitOfWork { get; set; }
        [Dependency] public IEntityRepository<Payment> Repository { get; set; }
        [Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
        [Dependency] public IInternalService InternalService { get; set; }
        [Dependency] public ServiceNotificationCreateHandler ServiceNotificationCreate { get; set; }
        [Dependency] public IEntityRepository<Recharge> RechargeRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentRefundArguments arguments)
        {
            var now = DateTime.Now;

			// Check PIN
			var correct = await InternalService.UserCheckPinAsync(arguments.Pin);
			if (correct == false)
				throw new ArgumentException(UserResources.IncorrectPin, "pin");
			if (!SessionData.Roles.Contains(AccountRoles.CommercePayment) && !SessionData.Roles.Contains(AccountRoles.PaymentWorker))
				throw new ArgumentException(PaymentUserResources.RefundNotAllowed);

            // Cargar pago para evitar devoluciones incorrectas
            var paymentTemp = (await Repository.GetAsync())
                .Where(x =>
                    x.Id == arguments.Id
                )
                .Select(x => new
                {
                    x.Amount,
                    EventsStarted = x.Ticket.Lines
                        .Any(y => y.EntranceType.CheckInStart <= DateTime.Now)
                })
                .FirstOrDefault();
            if (paymentTemp == null)
                throw new ArgumentException(PaymentResources.NoPayment);
            if (paymentTemp.Amount <= 0)
                throw new ArgumentException(PaymentResources.NegativeAmount);
            if (paymentTemp.EventsStarted)
                throw new ArgumentException(PaymentResources.EventsStarted);

            var payment = (await Repository.GetAsync("Recharges.PaymentMedia", "Ticket.Concession.Concession.Supplier", "Ticket.Recharges", "Recharges.CampaignLine"))
				.Where(x => 
					x.Id == arguments.Id &&
					x.State == PaymentState.Active &&
					!x.RefundTo.Where(y => y.State == PaymentState.Active).Any() &&
					x.Amount >= 0 //Devolución ya realizada (valor negativo del pago)
				)
				.FirstOrDefault();
			
			if (payment == null)
				throw new ArgumentException(PaymentResources.RefundDenied);

			foreach (var recharge in payment.Recharges) // comprobar si algún monedero no se puede devolver
			{
				if (recharge.PaymentMedia.Type == PaymentMediaType.Purse)
				{
					var res = await InternalService.PaymentMediaGetBalanceToRefundAsync(recharge.PaymentMediaId);
					if (res != null)
					{
						if (res.Balance - recharge.Amount < 0)	
						{
							throw new ArgumentException(TicketResources.RefundInsufficientBalance);
						}
					}
					else
						throw new ArgumentException(PaymentResources.RefundDenied);
				}
			}
			
			#region Devolver recargas asociadas
			foreach (var recharge in payment.Recharges.ToArray()) // comprobar si algún monedero no se puede devolver
			{				
				var refoundRecharge = new Recharge
				{
					Amount = - recharge.Amount,
					Date = now,
					UserName = SessionData.Name,
					UserLogin = SessionData.Login,
					TaxName = payment.Ticket.Concession.Concession.Supplier.TaxName,
					TaxAddress = payment.Ticket.Concession.Concession.Supplier.TaxAddress,
					TaxNumber = payment.Ticket.Concession.Concession.Supplier.TaxNumber,
					PaymentMediaId = recharge.PaymentMedia.Id,
					CampaignLineId = recharge.CampaignLine.Id,
					TicketId = payment.Ticket.Id,
					Payment = payment
				};
				await RechargeRepository.AddAsync(refoundRecharge);

				await InternalService.RechargePaymentMediaAsync(new PaymentMediaRechargeArguments(
						Purse: recharge.PaymentMediaId,
						Quantity: recharge.Amount * -1,
						name: "",
						bankEntity: "",
						number: ""
					));
			}
			#endregion Devolver recargas asociadas

			// Crear devolución del pago
			var refund = new Payment
			{
				AuthorizationCode = "",
				CommerceCode = "",
				Amount = -payment.Amount,
				Date = now.ToUTC(),
				Order = payment.Order,
				Payin = 0,
				PaymentMediaId = payment.PaymentMediaId,
				ErrorCode = "",
				ErrorText = "",
				ErrorPublic = "",
                ExternalLogin = "",
				State = PaymentState.Pending,
				TaxAddress = payment.TaxAddress,
				TaxName = payment.TaxName,
				TaxNumber = payment.TaxNumber,
				TicketId = payment.TicketId,
				UserLogin = payment.UserLogin,
				UserName = payment.UserName,
                Uid = null,
                UidFormat = null,
                Seq = null,
				RefundFrom = payment
			};
			await Repository.AddAsync(refund);
			await UnitOfWork.SaveAsync();

			// Ejecutar devolución
			var refundResponse = await InternalService.PaymentMediaRefundAsync(
				commerceCode: payment.CommerceCode,
				pin: arguments.Pin,
				publicPaymentMediaId: refund.PaymentMediaId,
				publicTicketId: refund.TicketId,
				publicPaymentId: refund.Id,
				order: payment.Order.Value,
				amount: payment.Amount
			);

			// Actualizar datos devolución
			refund.AuthorizationCode = refundResponse.AuthorizationCode ?? "";
			refund.CommerceCode = refundResponse.CommerceCode ?? "";
			refund.Order = refundResponse.OrderId;
			if (refundResponse.IsError)
			{
				refund.ErrorCode = refundResponse.ErrorCode;
				refund.ErrorText = refundResponse.ErrorText;

				refund.State = PaymentState.Error;
				await UnitOfWork.SaveAsync();

				throw new ApplicationException(ServiceNotificationResources.GatewayError.FormatString(
					refundResponse.ErrorCode,
					refundResponse.ErrorText
				));
			}
			else
			{
				refund.State = PaymentState.Active;

				await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.RefundSucceed,
					message: PaymentResources.RefundedPushMessage.FormatString(payment.Amount, payment.Ticket.Concession.Concession.Supplier.Name),
					referenceId: payment.TicketId,
					referenceClass: "Ticket",
					senderLogin: payment.Ticket.Concession.Concession.Supplier.Login,
					receiverLogin: payment.UserLogin
				));
			}

			return payment;
		}
		#endregion ExecuteAsync
	}
}
