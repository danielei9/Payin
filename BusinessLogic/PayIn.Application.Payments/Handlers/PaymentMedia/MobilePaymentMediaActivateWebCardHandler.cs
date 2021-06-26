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
	public class MobilePaymentMediaActivateWebCardHandler :
        IServiceBaseHandler<MobilePaymentMediaActivateWebCardArguments>
    {
        public readonly ISessionData SessionData;
        public readonly IInternalService InternalService;
        public readonly IUnitOfWork UnitOfWork;
        public readonly IEntityRepository<PaymentMedia> Repository;
        public readonly IEntityRepository<Ticket> TicketRepository;
        public readonly IEntityRepository<Domain.Payments.Payment> PaymentRepository;
        public readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
        public readonly IEntityRepository<ServiceOption> ServiceOptionRepository;

        #region Contructors
        public MobilePaymentMediaActivateWebCardHandler(
            ISessionData sessionData,
            IInternalService internalService,
            IUnitOfWork unitOfWork,
            IEntityRepository<PaymentMedia> repository,
            IEntityRepository<Ticket> ticketRepository,
            IEntityRepository<Domain.Payments.Payment> paymentRepository,
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
		public async Task<dynamic> ExecuteAsync(MobilePaymentMediaActivateWebCardArguments arguments)
		{
			var now = DateTime.Now;

			// Check PIN
			var correct = await InternalService.UserCheckPinAsync(arguments.Pin);
			if (correct == false)
				throw new ArgumentException(UserResources.IncorrectPin, "pin");

			var max = (await Repository.GetAsync())
				.Max(x => (int?)x.VisualOrder) ?? 0;

			// Calcular OrderId
			var lastOrderId = (await ServiceOptionRepository.GetAsync())
				.Where(x => x.Name == "LastOrderId")
				.FirstOrDefault();
			var order = Convert.ToInt32(lastOrderId.Value) + 1;
			lastOrderId.Value = Convert.ToString(order);

			// Cargar concession
			var payinConcession = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
				.Where(x => 
					x.Concession.Supplier.Login == "info@pay-in.es" &&
					x.Concession.Type == ServiceType.Charge
				)
				.FirstOrDefault();
			if (payinConcession == null)
				throw new Exception("info@pay-in.es needs a charge concession.");

            // Crear Tarjeta
            var paymentMedia = (await Repository.GetAsync())
                .Where(x =>
                       x.Id == arguments.Id
                ).FirstOrDefault();
            
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
			var payment = new Domain.Payments.Payment
			{
				AuthorizationCode = "",
				CommerceCode = "",
				Amount = 1,
				Date = now.ToUTC(),
				Order = order,
				Payin = 0,
				PaymentMedia = paymentMedia,
				ErrorCode = "",
				ErrorText = "",
				ErrorPublic = "",
				State = PaymentState.Pending,
				TaxAddress = SessionData.TaxAddress,
                TaxName = SessionData.Name,
				TaxNumber = SessionData.TaxNumber,
				Ticket = ticket,
				UserLogin = SessionData.Login,
				UserName = SessionData.Name
			};
			await PaymentRepository.AddAsync(payment);
			await UnitOfWork.SaveAsync();

			// Ejecutar pago
			var result = await InternalService.PaymentMediaCreateWebCardAsync(
				"",
				arguments.Pin, paymentMedia.Name, order, paymentMedia.Id, ticket.Id, payment.Id, paymentMedia.BankEntity,
				SessionData.Login, PaymentMediaCreateType.WebPaymentMediaCreate, payment.Amount,
				arguments.DeviceManufacturer, arguments.DeviceModel, arguments.DeviceName, arguments.DeviceSerial, arguments.DeviceId, arguments.DeviceOperator, arguments.DeviceImei, arguments.DeviceMac, arguments.OperatorSim, arguments.OperatorMobile
			);

			return result;
		}
		#endregion ExecuteAsync
	}
}
