using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Internal.Results;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Promotions;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Transport.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Payments.Handlers
{
    [XpLog("Ticket", "Pay")]
	[XpAnalytics("Ticket", "Pay")]
	public class MobileTicketPayV3Handler : IServiceBaseHandler<MobileTicketPayV3Arguments>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public IEntityRepository<Payment> PaymentRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentMedia> PaymentMediaRepository { get; set; }
		[Dependency] public IEntityRepository<PromoExecution> ExecutionRepository { get; set; }
		[Dependency] public IEntityRepository<Recharge> RechargeRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceOption> ServiceOptionRepository { get; set; }
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }
		[Dependency] public IEntityRepository<TransportPrice> PriceRepository { get; set; }
		[Dependency] public IInternalService InternalService { get; set; }
		[Dependency] public ServiceNotificationCreateHandler ServiceNotificationCreate { get; set; }
		[Dependency] public IApiCallbackService PaymentCallbackService { get; set; }
		[Dependency] public EmailService EmailService { get; set; }
		[Dependency] public EntranceGenerateMailHandler EntranceGenerateMailHandler { get; set; }
		[Dependency] public GreyListRepository GreyListRepository { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobileTicketPayV3Arguments arguments)
        {
            var now = DateTime.Now.ToUTC();

			return await ExecuteInternalAsync(now, arguments.Id, SessionData.Login, SessionData.Name, SessionData.TaxNumber, SessionData.TaxName, SessionData.TaxAddress, arguments.PaymentMedias, arguments.Promotions, arguments.Pin);
		}
		#endregion ExecuteAsync

		#region ExecuteInternalAsync
		public async Task<dynamic> ExecuteInternalAsync(DateTime now, int id, string login, string name, string taxNumber, string taxName, string taxAddress, IEnumerable<MobileTicketPayV3Arguments_PaymentMedia> paymentMedias_, IEnumerable<MobileTicketPayV3Arguments_Promotion> promotions, string pin)
        {
            // Cargar ticket
            var ticket = await Repository.GetAsync(id,
				"Payments",
				"Concession.Concession.Supplier",
				"PaymentWorker",
				"Concession.Campaigns.CampaignLines.Purse.PaymentMedias",
                "Lines.Entrances.EntranceType",
				"Lines.Purse");
			if (ticket == null)
				throw new ArgumentNullException("Id");
			if (ticket.Concession.Concession.State != ConcessionState.Active)
				throw new ApplicationException(TicketResources.PayNonActiveConcessionException);
			//if (ticket.Payments.Where(x => x.State == PaymentState.Pending).Any())
			//	throw new ApplicationException(TicketResources.PayingWithPendingPaymentException);
			var paid = ticket.Payments
				.Where(x => x.State == PaymentState.Active)
				.Sum(x => (decimal?)x.Amount) ?? 0;

			// Es gratis (p.ej.promociones)
			if (ticket.Amount != 0)
			{
				// Check PIN
				var correct = await InternalService.UserCheckPinAsync(pin);
				if (correct == false)
					throw new ArgumentException(UserResources.IncorrectPin, "pin");

				// Cargar usuario
				//var securityRepository = new SecurityRepository();
				//var user = await securityRepository.GetUserAsync(SessionData.Login);
				// TODO: No nulo

				#region Get PaymentMedias
				if (paymentMedias_ == null || paymentMedias_.Count() == 0)
					throw new ArgumentNullException("No hay seleccionados métodos de pago");

				var paymentMediasIds = paymentMedias_
					.OrderBy(x => x.Order)
					.Select(y => y.Id)
					.ToList();
				var paymentMedias = (await PaymentMediaRepository.GetAsync("Purse"))
					.Where(x =>
						(paymentMediasIds
							.Contains(x.Id)) &&
						(
							(x.Login == SessionData.Login) ||
							(x.PaymentConcession.Concession.Supplier.Login == SessionData.Login)
						)
					)
					.ToList()
					.Select(x => new
					{
						PaymentMedia = x,
						Tipo = x.Type == PaymentMediaType.Purse ? 2 : 1,
						Argument = paymentMedias_.Where(y => y.Id == x.Id).FirstOrDefault()
					})
					.OrderByDescending(x => x.Tipo)
					.ThenBy(x => x.Argument.Order)
					.Select(x => x.PaymentMedia)
					.ToList();
				#endregion Get PaymentMedias

				#region Calulate commision
				var comission = ticket.Concession.PayinCommision == 0 ?
					0 :
					Math.Max(
						Math.Ceiling(ticket.Amount * ticket.Concession.PayinCommision) / 100m,
						0.02m
					);
				#endregion Calulate commision

				#region Check direct discount
				var directDiscount = ticket.Concession.Campaigns
					.Where(x =>
						x.State == CampaignState.Active &&
						x.Since.Date <= DateTime.Now.Date &&
						(
							(x.Until != null && x.Until.Date >= DateTime.Now.Date) ||
							x.Until == null
						)
					)
					.Sum(x => (decimal?)(x.CampaignLines
						.Where(y => y.Type == CampaignLineType.DirectDiscount)
						.Sum(y => (decimal?)y.Quantity) ?? 0)
					) ?? 0;
				#endregion Check direct discount

				// TODO: Guardar en ticket
				// Hay que guardar el descuento en el ticket

				var amountToPayInTicket = ticket.Amount - paid - directDiscount; // que pasará a ser ticket.Discount

				foreach (var paymentMedia in paymentMedias)
				{
					if (amountToPayInTicket <= 0)
						break;

					#region Get Amount to Pay
					var amountToPayInPayment = 0m;
					if (paymentMedia.Type == PaymentMediaType.Purse)
					{
						var purse = await InternalService.PaymentMediaGetBalanceToRefundAsync(paymentMedia.Id);
						if (purse.Balance <= 0)
							continue;

						amountToPayInPayment = Math.Min(purse.Balance, amountToPayInTicket);
					}
					else
						amountToPayInPayment = amountToPayInTicket;
					amountToPayInTicket -= amountToPayInPayment;
					#endregion Get Amount to Pay

					#region Get OrderId
					var lastOrderId = (await ServiceOptionRepository.GetAsync())
							.Where(x => x.Name == "LastOrderId")
							.FirstOrDefault();
					var orderPurse = Convert.ToInt32(lastOrderId.Value) + 1;
					#endregion Get OrderId

					var retries = 0;
					Payment payment;
					PaymentMediaPayResult result;
					do
					{
						#region Create Payment
						payment = new Payment
						{
							AuthorizationCode = "",
							CommerceCode = "",
							Amount = amountToPayInPayment,
							Date = now.ToUTC(),
                            ExternalLogin = "",
							Order = null,
							Payin = 0, //Revisar cuando el pago sea fraccionado. Mirar comisión
							PaymentMediaId = paymentMedia.Id,
							ErrorCode = "",
							ErrorText = "",
							ErrorPublic = "",
							State = PaymentState.Pending,
							UserLogin = SessionData.Login,
							UserName = SessionData.Name,
                            Uid = null,
                            UidFormat = null,
                            Seq = null,
							TaxAddress = SessionData.TaxAddress,
							TaxName = SessionData.TaxName,
							TaxNumber = SessionData.TaxNumber,
							Ticket = ticket
						};
						await PaymentRepository.AddAsync(payment);
						await UnitOfWork.SaveAsync();
						#endregion Create Payment

						#region Calcular OrderId
						lastOrderId.Value = Convert.ToString(orderPurse);
						await UnitOfWork.SaveAsync();
						#endregion Calcular OrderId

						#region Pay in internal
						result = await InternalService.PaymentMediaPayAsync(
							pin: pin,
							publicPaymentMediaId: paymentMedia.Id,
							publicTicketId: ticket.Id,
							publicPaymentId: payment.Id,
							order: orderPurse,
							amount: amountToPayInPayment,
							login: login
						);
						#endregion Pay in internal

						#region Update info if payed
						payment.AuthorizationCode = result.AuthorizationCode ?? "";
						payment.CommerceCode = result.CommerceCode ?? "";
						payment.Order = result.OrderId;
						if (result.IsError)
						{
							payment.ErrorCode = result.ErrorCode;
							payment.ErrorText = result.ErrorText;
                            payment.ErrorPublic = result.ErrorPublic;

                            payment.State = PaymentState.Error;
						}
						#endregion Update info if payed

						orderPurse++;
					} while (
						result.IsError &&
						payment.ErrorCode == "SIS0051" &&
						retries++ < 3
					);

					#region Mark payment as active
					payment.State = PaymentState.Active;
					if (
                        (
                            (ticket.Type == TicketType.Order) ||
                            (ticket.Type == TicketType.Shipment)
                        ) &&
                        (ticket.State == TicketStateType.Created)
                    )
						ticket.State = TicketStateType.Pending;
					#endregion Mark payment as active

					#region Throw exception if error
					await UnitOfWork.SaveAsync();
					if (result.IsError)
					{
						throw new ApplicationException(ServiceNotificationResources.GatewayError.FormatString(
							result.ErrorCode,
							result.ErrorText
						));
					}
					#endregion Throw exception if error

					#region Comment: Recharge purses
					//var campaignLines = ticket.Concession.Campaigns
					//	.Where(x =>
					//		x.State == CampaignState.Active &&
					//		x.Since.Date <= now &&
					//		x.Until.Date >= now
					//	)
					//	.SelectMany(x => x.CampaignLines
					//		.Where(y =>
					//			(
					//				y.Type == CampaignLineType.Money ||
					//				y.Type == CampaignLineType.Percent
					//			) &&
					//			y.Purse.State == PurseState.Active &&
					//			y.Purse.Validity >= now &&
					//			y.Min <= ticket.Amount &&
					//			y.Max >= ticket.Amount &&
					//			(y.SinceTime == null || y.SinceTime.Value.TimeOfDay <= now.TimeOfDay) &&
					//			(y.UntilTime == null || y.UntilTime.Value.TimeOfDay >= now.TimeOfDay)
					//		)
					//	);
					//foreach (var campaignLine in campaignLines)
					//{
					//	#region Calculate Amount to recharge
					//	var amountToRecharge = 0m;
					//	switch (campaignLine.Type)
					//	{
					//		case CampaignLineType.Money:
					//			amountToRecharge = campaignLine.Quantity;
					//			break;
					//		case CampaignLineType.Percent:
					//			amountToRecharge = ((payment.Amount * campaignLine.Quantity) / 100);
					//			break;
					//	}
					//	#endregion Calculate Amount to recharge

					//	#region Get purse to recharge
					//	var purse = (await PaymentMediaRepository.GetAsync())
					//		.Where(x =>
					//			x.PurseId == campaignLine.Purse.Id &&
					//			x.Login == SessionData.Login
					//		)
					//		.FirstOrDefault();
					//	if (purse == null)
					//	{
					//		purse = new PaymentMedia
					//		{
					//			Name = campaignLine.Purse.Name,
					//			Login = SessionData.Login,
					//			State = PaymentMediaState.Active,
					//			Type = PaymentMediaType.Purse,
					//			Purse = campaignLine.Purse,
					//			NumberHash = "",
					//			BankEntity = ""
					//		};

					//		await PaymentMediaRepository.AddAsync(purse);
					//		await UnitOfWork.SaveAsync();
					//	}
					//	#endregion Get purse to recharge

					//	#region Recharge purse internal
					//	await InternalService.RechargePaymentMediaAsync(new PaymentMediaRechargeArguments(
					//		Purse: purse.Id,
					//		Quantity: amountToRecharge,
					//		name: purse.Name,
					//		bankEntity: purse.BankEntity,
					//		number: purse.NumberHash
					//	));
					//	#endregion Recharge purse internal

					//	// TODO: Mirar que devuelve el paso antarior

					//	#region Crear recarga
					//	var recharge = new Recharge
					//	{
					//		Amount = amountToRecharge,
					//		Date = now,
					//		UserName = SessionData.Name,
					//		UserLogin = SessionData.Login,
					//		TaxName = ticket.Concession.Concession.Supplier.TaxName,
					//		TaxAddress = ticket.Concession.Concession.Supplier.TaxAddress,
					//		TaxNumber = ticket.Concession.Concession.Supplier.TaxNumber,
					//		//LiquidationId =  ,
					//		PaymentMediaId = purse.Id,
					//		CampaignLineId = campaignLine.Id,
					//		TicketId = ticket.Id,
					//		Payment = payment
					//	};
					//	await RechargeRepository.AddAsync(recharge);
					//	#endregion Crear recarga
					//}
					#endregion Comment: Recharge purses

					#region Notify
					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
						type: NotificationType.PaymentSucceed,
						message: PaymentResources.PaidPushMessage.FormatString(payment.Amount, SessionData.Name, payment.UserName, payment.PaymentMedia.Name),
						referenceId: ticket.Id,
						referenceClass: "Ticket",
						senderLogin: SessionData.Login,
						receiverLogin: (ticket.PaymentWorker != null) ? ticket.PaymentWorker.Login : ticket.Concession.Concession.Supplier.Login
					));
					#endregion Notify

					#region Notification api
					if (!ticket.Concession.OnPayedUrl.IsNullOrEmpty())
						await SendPaymentNotificationAsync(ticket.Concession.OnPayedUrl, payment, now, amountToPayInPayment);
					#endregion Notification api
				}
			}

            await UpdateTicketWhenPaidAsync(ticket, now);

			#region Promociones no directas
			// TODO: Esto hay que revisarlo cuando se activen las promocions no directas
			//if (arguments.Promotions != null) // Si es null falla en el foreach
			//{
			//foreach (var code in arguments.Promotions)
			//{
			//	var promotionCode = (await ExecutionRepository.GetAsync("Promotion.PromoActions"))
			//	.Where(x => x.Id == code.Id)
			//	.FirstOrDefault();

			//	var pricePay = ticket.Lines.Where(z => z.TransportPriceId != null).FirstOrDefault();

			//	var price = (await PriceRepository.GetAsync("Title"))
			//		.Where(x => x.Id == pricePay.TransportPriceId)
			//		.FirstOrDefault();

			//	var cost = (price.Price / price.Title.Quantity) * promotionCode.Promotion.PromoActions.FirstOrDefault().Quantity;

			//	ticket.Lines.Add(new TicketLine
			//	{
			//		Title = code.Type == PromoActionType.MoreTravel ? TicketResources.PromotionMoreTravel : TicketResources.PromotionOther,
			//		Amount = 0,
			//		Quantity = code.Quantity
			//	});
			//	await UnitOfWork.SaveAsync();

			//	promotionCode.AppliedDate = now;
			//	promotionCode.Cost = cost ?? 0;
			//	promotionCode.TicketLineId = ticket.Lines.LastOrDefault().Id;

			//	await UnitOfWork.SaveAsync();
			//}
			//}
			#endregion Promociones no directas

			return ticket;
		}
		#endregion ExecuteAsync

		#region SendPaymentNotificationAsync
		public async Task SendPaymentNotificationAsync(string url, Payment payment, DateTime datetime, decimal amount)
		{
			try
			{
				var message = await PaymentCallbackService.OnPayedAsync(url, payment.UserLogin, datetime, payment.TicketId, payment.Id, amount);

#if DEBUG || TEST || HOMO
				payment.ErrorText = message;
				await EmailService.SendAsync(
					payment.Ticket.Concession.OnPayedEmail,
					"Notificado el pago {1} del ticket {0}".FormatString(payment.TicketId, payment.Id),
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
					"Error notificando el pago {1} del ticket {0}".FormatString(payment.TicketId, payment.Id),
					message
				);
			}
			catch (Exception ex)
			{
				var message = ex.Message;

				payment.ErrorText = message;
				await EmailService.SendAsync(
					payment.Ticket.Concession.OnPayedEmail,
					"Error notificando el pago {1} del ticket {0}".FormatString(payment.TicketId, payment.Id),
					message
				);
			}
		}
        #endregion SendPaymentNotificationAsync

        #region UpdateTicketWhenPaidAsync
        /// <summary>
        /// Update ticket when it's payed
        /// </summary>
        /// <param name="ticket">Ticket to update. It has to has loadede: Lines.Entrances.EntranceType.Event</param>
        public async Task UpdateTicketWhenPaidAsync(Ticket ticket, DateTime now)
        {
            foreach (var line in ticket.Lines)
            {
                #region Activate and send entrances / recharges
                foreach (var entrance in line.Entrances)
                {
                    entrance.State = EntranceState.Active;

                    // Send email
                    var body = entrance.GenerateBody();
                    if (!body.IsNullOrEmpty())
                    {
                        await EmailService.SendAsync(
                            entrance.Login,
                            "{0} {1} aquí tiene su entrada".FormatString(
                                entrance.UserName,
                                entrance.LastName
                            ),
                            body
                        );
                    }
                }
                #endregion Activate and send entrances / recharges

                #region Recharges by GL
                if ((line.PurseId != null) && (line.Uid != null) && (line.Type == TicketLineType.Recharge))
                {
                    var greyList = new GreyList
                    {
                        Uid = line.Uid.Value,
                        RegistrationDate = now,
                        Action = PayIn.Domain.Transport.GreyList.ActionType.IncreaseBalance,
                        Field =
                            line.Purse.Slot == 0 ? "W0B" :
                            line.Purse.Slot == 1 ? "W1B" :
                            "W2B",
                        NewValue = Math.Ceiling(line.Quantity * line.Amount * 100).ToString(),
                        Resolved = false,
                        ResolutionDate = null,
                        Machine = GreyList.MachineType.All,
                        Source = GreyList.GreyListSourceType.PayFalles,
                        State = GreyList.GreyListStateType.Active
                    };
                    await GreyListRepository.AddAsync(greyList);
                }
                #endregion Recharges by GL
            }
        }
        #endregion UpdateTicketWhenPaidAsync
    }
}