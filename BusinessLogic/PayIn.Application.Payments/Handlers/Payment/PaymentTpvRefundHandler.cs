using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments.Payment;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Payment", "Refund")]
	public class PaymentTpvRefundHandler :
		IServiceBaseHandler<PaymentTpvRefundArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<Payment> Repository;
		private readonly IInternalService InternalService;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		
		#region Contructors
		public PaymentTpvRefundHandler(
			ISessionData sessionData,
			IUnitOfWork unitOfWork,
			IEntityRepository<Payment> repository,
			IInternalService internalService,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (repository == null) throw new ArgumentNullException("repository");
			if (internalService == null) throw new ArgumentNullException("internalservice");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

			SessionData = sessionData;
			UnitOfWork = unitOfWork;
			Repository = repository;
			InternalService = internalService;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Contructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<PaymentTpvRefundArguments>.ExecuteAsync(PaymentTpvRefundArguments arguments)
		{
			var now = DateTime.Now;

			// Check PIN
			var correct = await InternalService.UserCheckPinAsync(arguments.Pin);
			if (correct == false)
				throw new ArgumentException(UserResources.IncorrectPin, "pin");

			// Cargar pago
			var payment = (await Repository.GetAsync("Ticket.Concession.Concession.Supplier"))
				.Where(x => 
					x.Id == arguments.PaymentId &&
					x.State == PaymentState.Active &&
					!x.RefundTo.Any(y => y.State == PaymentState.Active)
				)
				.FirstOrDefault();
			if (payment == null)
				throw new ArgumentNullException("id");

			// Crear devolución
			var refund = new Payment
			{
				AuthorizationCode = "",
				Amount = - payment.Amount,
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
				publicPaymentMediaId: refund.PaymentMediaId.Value,
				publicTicketId: refund.TicketId,
				publicPaymentId: refund.Id,
				order: payment.Order.Value,
				amount: payment.Amount
			);

			// Actualizar datos devolución
			refund.AuthorizationCode = refundResponse.AuthorizationCode;
			refund.CommerceCode = refundResponse.CommerceCode;
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
