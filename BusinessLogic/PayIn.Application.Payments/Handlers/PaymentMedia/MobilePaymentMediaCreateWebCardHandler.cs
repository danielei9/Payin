using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Payments.Infrastructure;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("PaymentMedia", "CreateWebCard")]
	[XpAnalytics("PaymentMedia", "CreateWebCard")]
	public class MobilePaymentMediaCreateWebCardHandler :
        IServiceBaseHandler<MobilePaymentMediaCreateWebCardArguments>
    {
        public readonly ISessionData SessionData;
        public readonly IInternalService InternalService;
        public readonly IUnitOfWork UnitOfWork;
        public readonly IEntityRepository<PaymentMedia> Repository;
        public readonly IEntityRepository<Ticket> TicketRepository;
        public readonly IEntityRepository<Payment> PaymentRepository;
        public readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
        public readonly IEntityRepository<ServiceOption> ServiceOptionRepository;

        #region Contructors
        public MobilePaymentMediaCreateWebCardHandler(
            ISessionData sessionData,
            IInternalService internalService,
            IUnitOfWork unitOfWork,
            IEntityRepository<PaymentMedia> repository,
            IEntityRepository<Ticket> ticketRepository,
            IEntityRepository<Payment> paymentRepository,
            IEntityRepository<PaymentConcession> paymentConcessionRepository,
            IEntityRepository<ServiceOption> serviceOptionRepository
        )
        {
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (repository == null) throw new ArgumentNullException("repository");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (paymentRepository == null) throw new ArgumentNullException("paymentRepository");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentConcessionRepository");
			if (internalService == null) throw new ArgumentNullException("internalService");
			if (serviceOptionRepository == null) throw new ArgumentNullException("serviceOptionRepository");

			SessionData = sessionData;
			UnitOfWork = unitOfWork;
			Repository = repository;
			TicketRepository = ticketRepository;
			PaymentRepository = paymentRepository;
			PaymentConcessionRepository = paymentConcessionRepository;
			InternalService = internalService;
			ServiceOptionRepository = serviceOptionRepository;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(MobilePaymentMediaCreateWebCardArguments arguments)
		{
			var now = DateTime.Now;

			var result = await ExecuteInternalAsync(null, PaymentMediaCreateType.WebPaymentMediaCreate, now, null, arguments.Name, arguments.BankEntity, arguments.Pin,
				SessionData.Login, SessionData.Name, SessionData.TaxName, "", null, SessionData.TaxNumber, SessionData.TaxAddress, "", SessionData.Email,
				arguments.DeviceManufacturer, arguments.DeviceModel, arguments.DeviceName, arguments.DeviceSerial, arguments.DeviceId, arguments.DeviceOperator, arguments.DeviceImei, arguments.DeviceMac, arguments.OperatorSim, arguments.OperatorMobile
			);
			return result;
		}
		#endregion ExecuteAsync

		#region ExecuteInternalAsync
		public async Task<dynamic> ExecuteInternalAsync(
			int? ticketId, PaymentMediaCreateType paymentMediaCreateType,
			DateTime now, int? paymentConcessionId, string name, string bankEntity, string pin,
			string login, string userName, string userTaxName, string userTaxLastName, DateTime? userBirthday, string userTaxNumber, string userTaxAddress, string userPhone, string userEmail,
			string deviceManufacturer, string deviceModel, string deviceName, string deviceSerial, string deviceId, string deviceOperator, string deviceImei, string deviceMac, string operatorSim, string operatorMobile
		)
		{
            // Check PIN
            var correct = await InternalService.UserCheckPinAsync(pin);
            if (correct == false)
                throw new ArgumentException(UserResources.IncorrectPin, "pin");

            // Comprobar pago no seguro
            if (string.Compare(login, SessionData.Login, true) != 0)
            {
                if (paymentConcessionId == null)
                    throw new ApplicationException(PaymentMediaResources.PaymentNotSecureNotAllowedException);

                var paymentConcession = (await PaymentConcessionRepository.GetAsync())
                    .Where(x =>
                        x.Id == paymentConcessionId.Value &&
                        x.AllowUnsafePayments
                    )
                    .FirstOrDefault();
                if (paymentConcession == null)
                    throw new ApplicationException(PaymentMediaResources.PaymentNotSecureNotAllowedException);
            }

            var max = (await Repository.GetAsync())
                .Max(x => (int?)x.VisualOrder) ?? 0;

            // Crear Tarjeta
            var paymentMedia = new PaymentMedia
            {
                Name = name,
                NumberHash = "**** **** **** ****",
                Type = PaymentMediaType.WebCard,
                VisualOrder = max + 1,
                VisualOrderFavorite = null,
                State = PaymentMediaState.Pending,
                BankEntity = bankEntity,
                Login = login,
                UserName = userName,
                UserLastName = userTaxName,
                UserBirthday = userBirthday,
                UserTaxNumber = userTaxNumber,
                UserAddress = userTaxAddress,
                UserPhone = userPhone ?? "",
                UserEmail = userEmail,
                Default = false,
                PaymentConcessionId = paymentConcessionId
            };
            await Repository.AddAsync(paymentMedia);

            // Calcular OrderId
            var lastOrderId = (await ServiceOptionRepository.GetAsync())
                .Where(x => x.Name == "LastOrderId")
                .FirstOrDefault();
            var order = Convert.ToInt32(lastOrderId.Value) + 1;
            lastOrderId.Value = Convert.ToString(order);

			Payment payment = null;
			if (ticketId == null)
			{
				// Cargar concession
				var payinConcession = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
					.Where(x =>
						x.Concession.Supplier.Login == "info@pay-in.es" &&
						x.Concession.Type == ServiceType.Charge
					)
					.FirstOrDefault();
				if (payinConcession == null)
					throw new Exception("info@pay-in.es needs a charge concession.");

				// Crear ticket
				var ticket = new Ticket
				{
					Concession = payinConcession,
					Date = now.ToUTC(),
					Reference = "",
					State = TicketStateType.Active,
					SupplierName = payinConcession.Concession.Supplier.Name,
					TaxAddress = payinConcession.Concession.Supplier.TaxAddress,
					TaxName = payinConcession.Concession.Supplier.TaxName,
					TaxNumber = payinConcession.Concession.Supplier.TaxNumber,
					Since = now.ToUTC(),
					Until = now.AddHours(6).ToUTC(),
					TextUrl = "",
					PdfUrl = "",
					Type = TicketType.Ticket
				};
				ticket.Lines.Add(new TicketLine
				{
					Amount = 0,
					Title = "Card validation",
					Quantity = 1,
					Type = TicketLineType.Buy
				});
				await TicketRepository.AddAsync(ticket);

				// Crear pago
				payment = new Payment
				{
					AuthorizationCode = "",
					CommerceCode = "",
					Amount = 1,
					Date = now.ToUTC(),
                    ExternalLogin = "",
					Order = order,
					Payin = 0,
					PaymentMedia = paymentMedia,
					ErrorCode = "",
					ErrorText = "",
					ErrorPublic = "",
					State = PaymentState.Pending,
                    Uid = null,
                    UidFormat = null,
                    Seq = null,
					UserLogin = login,
					UserName = userName,
					TaxName = userTaxName,
					TaxNumber = userTaxNumber,
					TaxAddress = userTaxAddress,
					Ticket = ticket
				};
				await PaymentRepository.AddAsync(payment);
				await UnitOfWork.SaveAsync();

				ticketId = ticket.Id;
			}
			else
			{
                var ticket = (await TicketRepository.GetAsync(ticketId.Value));

				// Crear pago
				payment = new Payment
				{
					AuthorizationCode = "",
					CommerceCode = "",
					Amount = ticket.Amount,
					Date = now.ToUTC(),
                    ExternalLogin = "",
					Order = order,
					Payin = 0,
					PaymentMedia = paymentMedia,
					ErrorCode = "",
					ErrorText = "",
					ErrorPublic = "",
					State = PaymentState.Pending,
                    Uid = null,
                    UidFormat = null,
                    Seq = null,
					UserLogin = login,
					UserName = userName,
					TaxName = userTaxName,
					TaxNumber = userTaxNumber,
					TaxAddress = userTaxAddress,
					TicketId = ticketId.Value
				};
				await PaymentRepository.AddAsync(payment);
				await UnitOfWork.SaveAsync();
			}

			// Ejecutar pago
			var result = await InternalService.PaymentMediaCreateWebCardAsync(
				"",
				pin, name, order, paymentMedia.Id, ticketId.Value, payment.Id, bankEntity,
				login, paymentMediaCreateType, payment.Amount,
				deviceManufacturer, deviceModel, deviceName, deviceSerial, deviceId, deviceOperator, deviceImei, deviceMac, operatorSim, operatorMobile
			);

			// Cambiar el id del paymentConcession de internal por el de públic
			//result.Id = payinConcession.Id;

			return result;
		}
		#endregion ExecuteInternalAsync
	}
}
