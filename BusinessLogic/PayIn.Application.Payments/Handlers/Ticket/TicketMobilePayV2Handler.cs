using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "Pay")]
	[XpAnalytics("Ticket", "Pay")]
	public class TicketMobilePayV2Handler :
		IServiceBaseHandler<TicketMobilePayV2Arguments>
	{
		private readonly ISessionData SessionData;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<Ticket> Repository;
		private readonly IEntityRepository<Purse> PurseRepository;
		private readonly IEntityRepository<Payment> PaymentRepository;
		private readonly IEntityRepository<PaymentMedia> PaymentMediaRepository;
		private readonly IEntityRepository<ServiceOption> ServiceOptionRepository;
		private readonly IInternalService InternalService;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Contructors
		public TicketMobilePayV2Handler(
				ISessionData sessionData,
				IUnitOfWork unitOfWork,
				IEntityRepository<Ticket> repository,
				IEntityRepository<Purse> purseRepository,
				IEntityRepository<Payment> paymentRepository,
				IEntityRepository<PaymentMedia> paymentMediaRepository,
				IEntityRepository<ServiceOption> serviceOptionRepositoryRepository,
				IInternalService internalService,
				ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (repository == null) throw new ArgumentNullException("repository");
			if (purseRepository == null) throw new ArgumentNullException("purseRepository");
			if (paymentRepository == null) throw new ArgumentNullException("paymentrepository");
			if (paymentMediaRepository == null) throw new ArgumentNullException("paymentmediarepository");
			if (serviceOptionRepositoryRepository == null) throw new ArgumentNullException("serviceOptionRepositoryRepository");
			if (internalService == null) throw new ArgumentNullException("internalservice");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

			SessionData = sessionData;
			UnitOfWork = unitOfWork;
			Repository = repository;
			PurseRepository = purseRepository;
			PaymentRepository = paymentRepository;
			PaymentMediaRepository = paymentMediaRepository;
			ServiceOptionRepository = serviceOptionRepositoryRepository;
			InternalService = internalService;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketMobilePayV2Arguments arguments)
        {
            throw new ApplicationException("Temporaly blocked");
            var now = DateTime.UtcNow;

			// Check PIN
			var correct = await InternalService.UserCheckPinAsync(arguments.Pin);
			if (correct == false)
				throw new ArgumentException(UserResources.IncorrectPin, "pin");

			// Cargar medio de pago
			//En arguments.PaymentMedias tengo los ids que intervienen en el pago.
			if (arguments.PaymentMedias.ToList().Count == 0)
				throw new ArgumentNullException("No hay seleccionados métodos de pago");

			List<dynamic> paymentMediaPurse = new List<dynamic>();
			var paymentMedia = new PaymentMedia();
			foreach (MobileTicketPayV2Arguments_PaymentMedia payMedia in arguments.PaymentMedias.OrderBy(o => o.Order).ToList())
			{
				var media = await PaymentMediaRepository.GetAsync(Convert.ToInt16(payMedia.Id), "Purse");
				if (media != null)
				{
					if (media.Type != PaymentMediaType.Purse) //Tarjeta
						paymentMedia = media;
					else paymentMediaPurse.Add(media);
				}
			}

			if (paymentMedia == null)
				throw new ArgumentNullException("PaymentMediaId");

			// Cargar ticket
			var ticket = await Repository.GetAsync(arguments.Id, "Payments", "Concession.Concession.Supplier", "PaymentWorker", "Concession.Campaigns.CampaignLines.Purse.PaymentMedias");
			if (ticket == null)
				throw new ArgumentNullException("Id");
			if (ticket.Concession.Concession.State != ConcessionState.Active)
				throw new ArgumentException(TicketResources.PayNonActiveConcessionException);
			if (ticket.Payments.Where(x => x.State == PaymentState.Pending).Any())
				throw new ArgumentException(TicketResources.PayingWithPendingPaymentException);

			var paid = ticket.Payments
					.Where(x => x.State == PaymentState.Active)
					.Sum(x => (decimal?)x.Amount) ?? 0;
			// Cargar usuario
			var securityRepository = new SecurityRepository();
			var user = await securityRepository.GetUserAsync(SessionData.Login);

			// Calcular commision
			var comission = ticket.Concession.PayinCommision == 0 ?
					0 :
					Math.Max(
							Math.Ceiling(ticket.Amount * ticket.Concession.PayinCommision) / 100m,
							0.02m
					);
			//Descontamos le saldo de los monederos aplicados al total del ticket
			decimal discount = 0;
			//decimal directDiscount = 0;
			bool futureDiscount = false;
			bool overDiscount = false;
			int overDiscountPurse = 0;
			decimal differenceTotal = 0;
			//Comprobar si hay un descuento directo con una campaña.
			foreach (var cam in ticket.Concession.Campaigns.Where(x => x.State == CampaignState.Active && x.Since.Date <= DateTime.Now.Date && ((x.Until!=null && x.Until.Date >= DateTime.Now.Date) || x.Until==null)))
			{
				foreach (var camLine in cam.CampaignLines)
				{
					switch (camLine.Type)
					{
						case CampaignLineType.DirectDiscount:
							discount += camLine.Quantity;
							break;
						//case CampaignLineType.DirectPrice:
						//	directDiscount += camLine.Quantity;
						//	break;
							//case CampaignLineType.Money:
							//case CampaignLineType.Percent:
							//	futureDiscount = true;
							//	break;
					}
				}
			}
			if (discount < ticket.Amount)
			{
				dynamic resultPurse; //devuelve lo que ha cobrado
				foreach (var purse in paymentMediaPurse)
				{
					decimal toPay = ticket.Amount - discount;
					if (toPay > 0)
					{
						MobileTicketPayV2Arguments_PaymentMedia res = arguments.PaymentMedias.FirstOrDefault(x => x.Id == purse.Id);
						discount += res.Balance;
						if (discount >= ticket.Amount)
						{
							overDiscount = true;
							overDiscountPurse = purse.Id;
							differenceTotal = discount - ticket.Amount;

							discount -= differenceTotal;
						}
						else toPay = res.Balance;
						// Calcular OrderId
						var lastOrderIdPurse = (await ServiceOptionRepository.GetAsync())
								.Where(x => x.Name == "LastOrderId")
								.FirstOrDefault();
						var orderPurse = Convert.ToInt32(lastOrderIdPurse.Value) + 1;
						lastOrderIdPurse.Value = Convert.ToString(orderPurse);
						// Crear pago
						var paymentPurse = new Payment
						{
							AuthorizationCode = "",
							CommerceCode = "",
							Amount = toPay,
							Date = now.ToUTC(),
                            ExternalLogin = "",
							Order = null,
							Payin = 0, //Revisar cuando el pago sea fraccionado.
							PaymentMediaId = purse.Id,
							ErrorCode = "",
							ErrorText = "",
							ErrorPublic = "",
							State = PaymentState.Active,
							TaxAddress = user.TaxAddress,
							TaxName = user.Name,
							TaxNumber = user.TaxNumber,
							Ticket = ticket,
							UserLogin = SessionData.Login,
							UserName = SessionData.Name,
                            Uid = null,
                            UidFormat = null,
                            Seq = null
						};
						await PaymentRepository.AddAsync(paymentPurse);
						await UnitOfWork.SaveAsync();

						resultPurse = await InternalService.PaymentMediaPayAsync(
							pin: arguments.Pin,
							publicPaymentMediaId: purse.Id,
							publicTicketId: ticket.Id,
							publicPaymentId: paymentPurse.Id,
							order: orderPurse,
							amount: toPay,
							login: SessionData.Login
						);
						await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
							type: NotificationType.PaymentSucceed,
							message: PaymentResources.PaidPushMessage.FormatString(paymentPurse.Amount, paymentPurse.UserName, paymentPurse.PaymentMedia.Name),
							referenceId: paymentPurse.TicketId,
							referenceClass: "Ticket",
							senderLogin: paymentPurse.UserLogin,
							receiverLogin: (ticket.PaymentWorker != null) ? ticket.PaymentWorker.Login : ticket.Concession.Concession.Supplier.Login
						));
					}
				}
			}

			if (discount < ticket.Amount)
			{
				if (ticket.Amount <= paid)
					throw new ApplicationException(TicketResources.TicketAlreadyPaid);


				// Crear pago
				var payment = new Payment
				{
					AuthorizationCode = "",
					CommerceCode = "",
					Amount = ticket.Amount - discount,
					Date = now.ToUTC(),
                    ExternalLogin = "",
					Order = null,
					Payin = comission,
					PaymentMediaId = paymentMedia.Id,
					ErrorCode = "",
					ErrorText = "",
					ErrorPublic = "",
					State = PaymentState.Pending,
					TaxAddress = user.TaxAddress,
					TaxName = user.Name,
					TaxNumber = user.TaxNumber,
					Ticket = ticket,
					UserLogin = SessionData.Login,
					UserName = SessionData.Name,
                    Uid = null,
                    UidFormat = null,
                    Seq = null
				};
				await PaymentRepository.AddAsync(payment);
				await UnitOfWork.SaveAsync();

				// Calcular OrderId
				var lastOrderId = (await ServiceOptionRepository.GetAsync())
						.Where(x => x.Name == "LastOrderId")
						.FirstOrDefault();
				var order = Convert.ToInt32(lastOrderId.Value) + 1;
				lastOrderId.Value = Convert.ToString(order);
				// Ejecutar pago
				var result = await InternalService.PaymentMediaPayAsync(
					pin: arguments.Pin,
					publicPaymentMediaId: paymentMedia.Id,
					publicTicketId: ticket.Id,
					publicPaymentId: payment.Id,
					order: order,
					amount: ticket.Amount,
					login: SessionData.Login
				);

				// Actualizar datos cobro
				payment.AuthorizationCode = result.AuthorizationCode;
				payment.CommerceCode = result.CommerceCode;
				payment.Order = result.OrderId;
				if (result.IsError)
				{
					payment.ErrorCode = result.ErrorCode;
					payment.ErrorText = result.ErrorText;

					payment.State = PaymentState.Error;
					await UnitOfWork.SaveAsync();

					throw new ApplicationException(ServiceNotificationResources.GatewayError.FormatString(
							result.ErrorCode,
							result.ErrorText
					));
				}

				payment.State = PaymentState.Active;
				if ((ticket.Type == TicketType.Order) && (ticket.State == TicketStateType.Created))
					ticket.State = TicketStateType.Pending;

				await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
						type: NotificationType.PaymentSucceed,
						message: PaymentResources.PaidPushMessage.FormatString(payment.Amount, payment.UserName,payment.PaymentMedia.Name),
						referenceId: payment.TicketId,
						referenceClass: "Ticket",
						senderLogin: payment.UserLogin,
						receiverLogin: (ticket.PaymentWorker != null) ? ticket.PaymentWorker.Login : ticket.Concession.Concession.Supplier.Login
				));
			}
			//if (futureDiscount)
			//{
			//	decimal rechargeDone = 0;
			//	foreach (var cam in ticket.Concession.Campaigns.Where(x => x.State == CampaignState.Active && x.Since.Date <= DateTime.Now.Date && x.Until.Date >= DateTime.Now.Date))
			//	{
			//		decimal quantity = 0;
			//		foreach (var camLine in cam.CampaignLines)
			//		{
			//			if (rechargeDone > ticket.Amount)
			//				break;

			//			switch (camLine.Type)
			//			{
			//				case CampaignLineType.Money:
			//					quantity = camLine.Quantity;
			//					break;
			//				case CampaignLineType.Percent:
			//					quantity = ((ticket.Amount * camLine.Quantity) / 100);
			//					break;
			//			}

			//			if (camLine.Purse.State != PurseState.Active)
			//				continue;
			//			if (camLine.Purse.Validity!=null && camLine.Purse.Validity.Date < DateTime.Now.Date)
			//				continue;
			//			if (camLine.Min > ticket.Amount)
			//				continue;
			//			if (camLine.Max < ticket.Amount)
			//				continue;
			//			if ((camLine.SinceTime != null) && (camLine.SinceTime.Value.TimeOfDay > now.TimeOfDay))
			//				continue;
			//			if ((camLine.UntilTime != null) && (camLine.UntilTime.Value.TimeOfDay < now.TimeOfDay))
			//				continue;

			//				var paymentmedia = (await PaymentMediaRepository.GetAsync())
			//					.Where(x => x.PurseId == camLine.Purse.Id && x.Login == SessionData.Login)
			//					.FirstOrDefault();

			//				if (paymentmedia == null)
			//				{
			//					paymentmedia = new PaymentMedia
			//					{
			//						Name = camLine.Purse.Name,
			//						Login = SessionData.Login,
			//						State = PaymentMediaState.Active,
			//						Type = PaymentMediaType.Purse,
			//						PurseId = camLine.Purse.Id,
			//						NumberHash = "",
			//						BankEntity = "",
			//						UserName = SessionData.Name,
			//						UserLastName = SessionData.TaxName,
			//						UserBirthday = null,
			//						UserTaxNumber = SessionData.TaxNumber,
			//						UserAddress = SessionData.TaxAddress,
			//						UserPhone = "",
			//						UserEmail = SessionData.Email,
			//						Default = false
			//					};
			//					await PaymentMediaRepository.AddAsync(paymentmedia);
			//					await UnitOfWork.SaveAsync();
			//				}

			//				await InternalService.RechargePaymentMediaAsync(new PaymentMediaRechargeArguments(
			//					Purse: paymentmedia.Id,
			//					Quantity: quantity,
			//					name: paymentmedia.Name,
			//					bankEntity: paymentmedia.BankEntity,
			//					number: paymentmedia.NumberHash
			//				));

			//				rechargeDone += quantity;
			//			}
			//		if (rechargeDone > ticket.Amount)
			//			break;
			//	}
			//}
			if (overDiscount) // Recargar sobrante del monedero si la cantidad excede del total.
			{
				await InternalService.RechargePaymentMediaAsync(new PaymentMediaRechargeArguments(
								Purse: overDiscountPurse,
								Quantity: differenceTotal,
								name: null,
								bankEntity: null,
								number: null
								));
			}

			return ticket;
		}
		#endregion ExecuteAsync
	}
}
