using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Infrastructure.Sabadell;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentMedia", "CreateWebCardSabadell")]
	public class SabadellPaymentMediaCreateWebCardHandler :
		IServiceBaseHandler<SabadellPaymentMediaCreateWebCardArguments>
	{
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public IEntityRepository<PaymentMedia> Repository { get; set; }
		[Dependency] public IEntityRepository<Payment> PaymentRepository { get; set; }
		[Dependency] public IInternalService InternalService { get; set; }
		[Dependency] public ServiceNotificationCreateHandler ServiceNotificationCreate { get; set; }
		[Dependency] public EmailService EmailService { get; set; }
		[Dependency] public IApiCallbackService ApiCallbackService { get; set; }
        [Dependency] public MobileTicketPayV3Handler MobileTicketPayV3Handler { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(SabadellPaymentMediaCreateWebCardArguments arguments)
		{
			var now = DateTime.Now;

			Payment payment = null;
			try
			{
				var datos = arguments.Ds_MerchantParameters.FromBase64().ToUtf8();

				#region Control de error en operación
				var internalArguments = SabadellGatewayFunctions.GetWebPaymentResponse(
					arguments.Ds_MerchantParameters,
					arguments.Ds_SignatureVersion,
					arguments.Ds_Signature
				);
				if (internalArguments.IsError)
					throw new ApplicationException("Error: " + internalArguments.ErrorPublic);

				await InternalService.PaymentMediaCreateWebCardSabadellAsync(internalArguments);
				#endregion Control de error en operación

				#region Actualizar datos pago
				payment = (await PaymentRepository.GetAsync("Ticket.Concession","Ticket.Lines.Entrances.EntranceType.Event", "Ticket.Lines.Purse"))
					.Where(x =>
						x.Id == internalArguments.PublicPaymentId &&
						x.State == PaymentState.Pending
					)
					.FirstOrDefault();
				payment.AuthorizationCode = internalArguments.AuthorizationCode;
				payment.CommerceCode = internalArguments.CommerceCode;
				#endregion Actualizar datos pago

				PaymentMedia paymentMedia = null;

                #region Actualización datos tarjeta
                if (internalArguments.PaymentMediaCreateType != PaymentMediaCreateType.WebTicketPay)
                {
                    paymentMedia = (await Repository.GetAsync())
                        .Where(x =>
                            x.Id == internalArguments.PublicPaymentMediaId &&
                            x.State == PaymentMediaState.Pending
                        )
                        .FirstOrDefault();
                    if (paymentMedia == null)
                        throw new ApplicationException("Payment Media {0} doesn't exist or isn't confirmation pending".FormatString(internalArguments.PublicPaymentMediaId));
                    paymentMedia.NumberHash = "{0} {1} {2} {3}".FormatString(
                        internalArguments.CardNumberHash.Substring(0, 4),
                        internalArguments.CardNumberHash.Substring(4, 4),
                        internalArguments.CardNumberHash.Substring(8, 4),
                        internalArguments.CardNumberHash.Substring(12)
                    );
                    paymentMedia.ExpirationMonth = internalArguments.ExpirationMonth;
                    paymentMedia.ExpirationYear = internalArguments.ExpirationYear;
                }
				#endregion Actualización datos tarjeta

				if (internalArguments.IsError)
				{
					payment.ErrorCode = internalArguments.ErrorCode;
					payment.ErrorText = internalArguments.ErrorText;
					payment.ErrorPublic = internalArguments.ErrorPublic;

					payment.State = PaymentState.Error;
                    if (internalArguments.PaymentMediaCreateType != PaymentMediaCreateType.WebTicketPay)
                        paymentMedia.State = PaymentMediaState.Error;

					#region Notification push (Error confirmación pago)
					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
						type: NotificationType.PaymentError,
						message: PaymentResources.PaymentMediaCreationPaymentErrorPushMessage.FormatString(
							payment.Amount,
							ServiceNotificationResources.GatewayError.FormatString(
								internalArguments.ErrorCode,
								internalArguments.ErrorText
							)
						),
						referenceId: payment.TicketId,
						referenceClass: "Ticket",
						senderLogin: "info@pay-in.es",
						receiverLogin: payment.UserLogin
					));
					#endregion Notification push (Error confirmación pago)

					#region Notification push (Error creación de tarjeta)
					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
						type: NotificationType.PaymentMediaCreateError,
						message: PaymentMediaResources.PaymentMediaCreateExceptionPayment,
						referenceId: internalArguments.PublicPaymentMediaId,
						referenceClass: "PaymentMedia",
						senderLogin: "info@pay-in.es",
						receiverLogin: payment.UserLogin
					));
					await UnitOfWork.SaveAsync();
					#endregion Notification push (Error creación de tarjeta)

					#region Notification api (Error creación de tarjeta)
					if (!payment.Ticket.Concession.OnPaymentMediaErrorCreatedUrl.IsNullOrEmpty())
						await SendPaymentMediaCreationErrorNotificationAsync(
							payment.Ticket.Concession.OnPaymentMediaCreatedUrl,
							payment,
							now,
							payment.ErrorCode,
							payment.ErrorText
						);
					#endregion Notification api (Error creación de tarjeta)

					return null;
				}

				payment.State = PaymentState.Active;

				#region Notification push (Confirmación pago)
				await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.PaymentSucceed,
					message: PaymentResources.PaymentMediaCreationPaymentPushMessage,
					referenceId: payment.TicketId,
					referenceClass: "Ticket",
					senderLogin: "info@pay-in.es",
					receiverLogin: payment.UserLogin
				));
				#endregion Notification push (Confirmación pago)

				if (internalArguments.PaymentMediaCreateType == PaymentMediaCreateType.WebPaymentMediaCreate)
				{
					#region Crear devolución
					var refund = new Payment
					{
						AuthorizationCode = "",
						CommerceCode = "",
						Amount = -payment.Amount,
						Date = now.ToUTC(),
                        ExternalLogin = "",
						Order = payment.Order,
						Payin = 0,
						PaymentMediaId = internalArguments.PublicPaymentMediaId,
						ErrorCode = "",
						ErrorText = "",
						ErrorPublic = "",
						State = PaymentState.Pending,
						TaxAddress = payment.TaxAddress,
						TaxName = payment.TaxName,
						TaxNumber = payment.TaxNumber,
						Ticket = payment.Ticket,
						UserLogin = payment.UserLogin,
						UserName = payment.UserName,
                        Uid = null,
                        UidFormat = null,
                        Seq = null,
						RefundFrom = payment
					};
					await PaymentRepository.AddAsync(refund);
					await UnitOfWork.SaveAsync();
					#endregion Crear devolución

					#region Ejecutar devolución
					var refundResponse = await InternalService.PaymentMediaCreateWebCardRefundSabadellAsync(
						commerceCode: payment.CommerceCode,
						publicPaymentMediaId: internalArguments.PublicPaymentMediaId,
						publicTicketId: internalArguments.PublicTicketId,
						publicPaymentId: refund.Id,
						amount: internalArguments.Amount,
						currency: internalArguments.Currency,
						orderId: internalArguments.OrderId,
						terminal: internalArguments.Terminal
					);
					#endregion Ejecutar devolución

					#region Actualizar datos devolución
					refund.AuthorizationCode = refundResponse.AuthorizationCode;
					refund.CommerceCode = refundResponse.CommerceCode;
					refund.Order = refundResponse.OrderId;
					#endregion Actualizar datos devolución

					if (refundResponse.IsError)
					{
						refund.ErrorCode = refundResponse.ErrorCode;
						refund.ErrorText = refundResponse.ErrorText;
						refund.ErrorPublic = refundResponse.ErrorPublic;

						refund.State = PaymentState.Error;
                        if (internalArguments.PaymentMediaCreateType != PaymentMediaCreateType.WebTicketPay)
                            paymentMedia.State = PaymentMediaState.Error;

						#region Notification push (Error devolución pago)
						await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
							type: NotificationType.RefundError,
							message: PaymentResources.PaymentMediaCreationRefundErrorPushMessage.FormatString(
								payment.Amount,
								ServiceNotificationResources.GatewayError.FormatString(
									refundResponse.ErrorCode,
									refundResponse.ErrorText
								)
							),
							referenceId: refund.TicketId,
							referenceClass: "Ticket",
							senderLogin: "info@pay-in.es",
							receiverLogin: refund.UserLogin
						));
						#endregion Notification push (Error devolución pago)

						#region Notification push (Error creación de tarjeta)
						await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
							type: NotificationType.PaymentMediaCreateError,
							message: PaymentMediaResources.PaymentMediaCreateExceptionRefund,
							referenceId: internalArguments.PublicPaymentMediaId,
							referenceClass: "PaymentMedia",
							senderLogin: "info@pay-in.es",
							receiverLogin: payment.UserLogin
						));
						#endregion Notification push (Error creación de tarjeta)

						#region Notification api (Error creación de tarjeta)
						if (!payment.Ticket.Concession.OnPaymentMediaErrorCreatedUrl.IsNullOrEmpty())
							await SendPaymentMediaCreationErrorNotificationAsync(
								payment.Ticket.Concession.OnPaymentMediaCreatedUrl,
								payment,
								now,
								payment.ErrorCode,
								payment.ErrorText
							);
						#endregion Notification api (Error creación de tarjeta)

						return null;
					}

					refund.State = PaymentState.Active;

					#region Notification push (Devolución pago)
					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
						type: NotificationType.RefundSucceed,
						message: PaymentResources.PaymentMediaCreationRefundPushMessage,
						referenceId: refund.TicketId,
						referenceClass: "Ticket",
						senderLogin: "info@pay-in.es",
						receiverLogin: refund.UserLogin
					));
					#endregion Notification push (Devolución pago)
				}
                else if (internalArguments.PaymentMediaCreateType == PaymentMediaCreateType.WebTicketPayAndPaymentMediaCreate)
                {
                    #region Ejecutar confirmación
                    var confirmResponse = await InternalService.PaymentMediaCreateWebCardConfirmSabadellAsync(
                        publicPaymentMediaId: internalArguments.PublicPaymentMediaId
                    );
					#endregion Ejecutar confirmación

					payment.Ticket.State = TicketStateType.Active;
				}
				else
				{
					payment.Ticket.State = TicketStateType.Active;
				}

				if (internalArguments.PaymentMediaCreateType != PaymentMediaCreateType.WebTicketPay)
                {
                    paymentMedia.State = PaymentMediaState.Active;

                    #region Notification push (Creación de tarjeta)
                    await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
                        type: NotificationType.PaymentMediaCreateSucceed,
                        message: PaymentMediaResources.PaymentMediaCreateSuccess,
                        referenceId: internalArguments.PublicPaymentMediaId,
                        referenceClass: "PaymentMedia",
                        senderLogin: "info@pay-in.es",
                        receiverLogin: payment.UserLogin
                    ));
                    #endregion Notification push (Creación de tarjeta)
                }

                #region Notification api (Creación de tarjeta)
                if (!payment.Ticket.Concession.OnPaymentMediaCreatedUrl.IsNullOrEmpty())
                    await SendPaymentMediaCreatedNotificationAsync(payment.Ticket.Concession.OnPaymentMediaCreatedUrl, payment, now);
                #endregion Notification api (Creación de tarjeta)

                await MobileTicketPayV3Handler.UpdateTicketWhenPaidAsync(payment.Ticket, now);

                return null;
			}
			catch (Exception e)
			{
				#region Notification api (Error creación de tarjeta)
				if (
					(payment != null) &&
					(!payment.Ticket.Concession.OnPaymentMediaErrorCreatedUrl.IsNullOrEmpty())
				)
					await SendPaymentMediaCreationErrorNotificationAsync(
						payment.Ticket.Concession.OnPaymentMediaCreatedUrl,
						payment,
						now,
						"",
						e.GetXpMessage()
					);
				#endregion Notification api (Error creación de tarjeta)

				throw;
			}
		}
		#endregion ExecuteAsync

		#region SendPaymentMediaCreatedNotificationAsync
		public async Task SendPaymentMediaCreatedNotificationAsync(string url, Payment payment, DateTime datetime)
		{
			try
			{
				var message = await ApiCallbackService.OnPaymentMediaCreatedAsync(url, payment.UserLogin, datetime, payment.TicketId, payment.Id);

#if DEBUG || TEST || HOMO
				payment.ErrorText = message;
				await EmailService.SendAsync(
					payment.Ticket.Concession.OnPayedEmail,
					"Notificada la creación de medio de pago de {0} con el ticket {1}".FormatString(payment.UserLogin, payment.TicketId),
					message
				);
#endif // DEBUG || TEST || HOMO
			}
			catch (AggregateException ex)
			{
				var message = "";
				foreach (var exception in ex.InnerExceptions)
					message += exception.Message;

				payment.ErrorText = message;
				await EmailService.SendAsync(
					payment.Ticket.Concession.OnPayedEmail,
					"Error notificando la creación de medio de pago de {0} con el ticket {1}".FormatString(payment.UserLogin, payment.TicketId),
					message
				);
			}
			catch (Exception ex)
			{
				var message = ex.Message;

				payment.ErrorText = message;
				await EmailService.SendAsync(
					payment.Ticket.Concession.OnPayedEmail,
					"Error notificando la creación de medio de pago de {0} con el ticket {1}".FormatString(payment.UserLogin, payment.TicketId),
					message
				);
			}
		}
		#endregion SendPaymentMediaCreatedNotificationAsync

		#region SendPaymentMediaCreationErrorNotificationAsync
		public async Task SendPaymentMediaCreationErrorNotificationAsync(string url, Payment payment, DateTime datetime, string errorCode, string errorMessage)
		{
			try
			{
				var message = await ApiCallbackService.OnPaymentMediaCreationErrorAsync(url, payment.UserLogin, datetime, payment.TicketId, payment.Id, errorCode, errorMessage);

#if DEBUG || TEST || HOMO
				await EmailService.SendAsync(
					payment.Ticket.Concession.OnPayedEmail,
					"Notificado el error en la creación de medio de pago de {0} con el ticket {1}".FormatString(payment.UserLogin, payment.TicketId),
					message
				);
#endif // DEBUG || TEST || HOMO
			}
			catch (AggregateException ex)
			{
				var message = "";
				foreach (var exception in ex.InnerExceptions)
					message += exception.Message;
				
				await EmailService.SendAsync(
					payment.Ticket.Concession.OnPayedEmail,
					"Error notificando el error en la creación de medio de pago de {0} con el ticket {1}".FormatString(payment.UserLogin, payment.TicketId),
					message
				);
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				
				await EmailService.SendAsync(
					payment.Ticket.Concession.OnPayedEmail,
					"Error notificando el error en la creación de medio de pago de {0} con el ticket {1}".FormatString(payment.UserLogin, payment.TicketId),
					message
				);
			}
		}
		#endregion SendPaymentMediaCreationErrorNotificationAsync
	}
}
