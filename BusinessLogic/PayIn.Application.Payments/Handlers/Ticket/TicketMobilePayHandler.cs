using PayIn.Application.Dto.Arguments.Notification;
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
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "Pay")]
    [XpAnalytics("Ticket", "Pay")]
    public class TicketMobilePayHandler :
        IServiceBaseHandler<TicketMobilePayArguments>
    {
        private readonly ISessionData SessionData;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IEntityRepository<Ticket> Repository;
        private readonly IEntityRepository<Payment> PaymentRepository;
        private readonly IEntityRepository<PaymentMedia> PaymentMediaRepository;
        private readonly IEntityRepository<ServiceOption> ServiceOptionRepository;
        private readonly IInternalService InternalService;
        private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

        #region Contructors
        public TicketMobilePayHandler(
            ISessionData sessionData,
            IUnitOfWork unitOfWork,
            IEntityRepository<Ticket> repository,
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
            if (paymentRepository == null) throw new ArgumentNullException("paymentrepository");
            if (paymentMediaRepository == null) throw new ArgumentNullException("paymentmediarepository");
            if (serviceOptionRepositoryRepository == null) throw new ArgumentNullException("serviceOptionRepositoryRepository");
            if (internalService == null) throw new ArgumentNullException("internalservice");
            if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

            SessionData = sessionData;
            UnitOfWork = unitOfWork;
            Repository = repository;
            PaymentRepository = paymentRepository;
            PaymentMediaRepository = paymentMediaRepository;
            ServiceOptionRepository = serviceOptionRepositoryRepository;
            InternalService = internalService;
            ServiceNotificationCreate = serviceNotificationCreate;
        }
        #endregion Contructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(TicketMobilePayArguments arguments)
        {
            throw new ApplicationException("Temporaly blocked");

            var now = DateTime.Now;

            // Check PIN
            var correct = await InternalService.UserCheckPinAsync(arguments.Pin);
            if (correct == false)
                throw new ArgumentException(UserResources.IncorrectPin, "pin");

			// Cargar medio de pago
			var paymentMedia = await PaymentMediaRepository.GetAsync(arguments.PaymentMediaId);
            if (paymentMedia == null)
                throw new ArgumentNullException("PaymentMediaId");

            // Cargar ticket
            var ticket = await Repository.GetAsync(arguments.Id, "Payments", "Concession.Concession.Supplier", "PaymentWorker");
            if (ticket == null)
                throw new ArgumentNullException("Id");
            if (ticket.Concession.Concession.State != ConcessionState.Active)
                throw new ArgumentException(TicketResources.PayNonActiveConcessionException);

            var paid = ticket.Payments
                .Where(x => x.State == PaymentState.Active)
                .Sum(x => (decimal?)x.Amount) ?? 0;
            if (ticket.Amount <= paid)
                throw new ApplicationException(TicketResources.TicketAlreadyPaid);
            // Cargar usuario
            var securityRepository = new SecurityRepository();
            var user = await securityRepository.GetUserAsync(SessionData.Login);

            // Calcular OrderId
            var lastOrderId = (await ServiceOptionRepository.GetAsync())
                .Where(x => x.Name == "LastOrderId")
                .FirstOrDefault();
            var order = Convert.ToInt32(lastOrderId.Value) + 1;
            lastOrderId.Value = Convert.ToString(order);

            // Calcular commision
            var comission = ticket.Concession.PayinCommision == 0 ?
                0 :
                Math.Max(
                    Math.Ceiling(ticket.Amount * ticket.Concession.PayinCommision) / 100m,
                    0.02m
                );

            // Crear pago
            var payment = new Payment
            {
                AuthorizationCode = "",
				CommerceCode = "",
				Amount = ticket.Amount,
                Date = now.ToUTC(),
                ExternalLogin = "",
                Order = null,
                Payin = comission,
                PaymentMediaId = arguments.PaymentMediaId,
                ErrorCode = "",
                ErrorText = "",
                ErrorPublic = "",
                State = PaymentState.Pending,
                TaxAddress = user.TaxAddress,
                //TaxName = user.TaxName,
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
                message: PaymentResources.PaidPushMessage.FormatString(payment.Amount, payment.UserName, payment.PaymentMedia.Name),
                referenceId: payment.TicketId,
                referenceClass: "Ticket",
                senderLogin: payment.UserLogin,
                receiverLogin: (ticket.PaymentWorker != null) ? ticket.PaymentWorker.Login : ticket.Concession.Concession.Supplier.Login
            ));

            return ticket;
        }
        #endregion ExecuteAsync
    }
}
