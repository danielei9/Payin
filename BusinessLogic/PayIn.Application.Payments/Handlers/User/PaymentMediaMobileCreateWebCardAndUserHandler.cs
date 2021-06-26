using PayIn.Application.Dto.Payments.Arguments.User;
using PayIn.Application.Dto.Payments.Arguments.PaymentMedia;
using PayIn.BusinessLogic.Common;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using PayIn.Application.Public.Handlers;
using PayIn.Application.Dto.Payments.Arguments.PaymentUser;

namespace PayIn.Application.Payments.Handlers.User
{
	[XpLog("User", "Create")]
	[XpAnalytics("User", "Create")]
	public class PaymentMediaMobileCreateWebCardAndUserHandler : 
		IServiceBaseHandler<PaymentMediaMobileCreateWebCardAndUserArguments>
	{
        private readonly ISessionData SessionData;

        //private readonly IEntityRepository<PaymentConcession> RepositoryPaymentConcession;
        //private readonly IUnitOfWork UnitOfWork;
        //public readonly IInternalService InternalService;
        //public readonly IEntityRepository<PaymentMedia> PaymentMediaRepository;
        //public readonly IEntityRepository<Ticket> TicketRepository;
        //public readonly IEntityRepository<Payment> PaymentRepository;
        //public readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
        //public readonly IEntityRepository<ServiceOption> ServiceOptionRepository;

        //private readonly IEntityRepository<PaymentUser> PaymentUserRepository;
        //private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
        private readonly PaymentMediaMobileCreateWebCardHandler PaymentMediaHandler;
        private readonly PaymentUserCreateHandler PaymentUserHandler;


        #region Constructors
        public PaymentMediaMobileCreateWebCardAndUserHandler(
            ISessionData sessionData,
            //IInternalService internalService,
            //IUnitOfWork unitOfWork,
            //IEntityRepository<PaymentMedia> paymentMediaRepository,
            //IEntityRepository<Ticket> ticketRepository,
            //IEntityRepository<Payment> paymentRepository,
            //IEntityRepository<PaymentConcession> paymentConcessionRepository,
            //IEntityRepository<ServiceOption> serviceOptionRepository,
            
            //IEntityRepository<PaymentUser> paymentUserRepository,
            //ServiceNotificationCreateHandler serviceNotificationCreate,

            PaymentMediaMobileCreateWebCardHandler paymentMediaHandler,
            PaymentUserCreateHandler paymentUserHandler

        )
		{
            if (sessionData == null)throw new ArgumentNullException("sessionData");
            //if (internalService == null)throw new ArgumentNullException("internalService");
            //         if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            //         if (paymentMediaRepository == null) throw new ArgumentNullException("paymentMediaRepository");
            //         if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
            //         if (paymentRepository == null) throw new ArgumentNullException("paymentRepository");
            //         if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentConcessionRepository");
            //         if (serviceOptionRepository == null) throw new ArgumentNullException("serviceOptionRepository");


            //         if (paymentUserRepository == null) throw new ArgumentNullException("paymentUserRepository");
            //         if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");


            if (paymentMediaHandler == null) throw new ArgumentNullException("paymentMediaHandler");
            if (paymentUserHandler == null) throw new ArgumentNullException("paymentUserHandler");

            SessionData = sessionData;
            //         UnitOfWork = unitOfWork;
            //         PaymentMediaRepository = paymentMediaRepository;
            //         TicketRepository = ticketRepository;
            //         PaymentRepository = paymentRepository;
            //         PaymentConcessionRepository = paymentConcessionRepository;
            //         InternalService = internalService;
            //         ServiceOptionRepository = serviceOptionRepository;

            //         PaymentUserRepository = paymentUserRepository;
            //         ServiceNotificationCreate = serviceNotificationCreate;


            PaymentMediaHandler = paymentMediaHandler;
            PaymentUserHandler = paymentUserHandler;

        }
		#endregion Constructors
		
		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentMediaMobileCreateWebCardAndUserArguments arguments)
		{
            await PaymentUserHandler.ExecuteAsync(new PaymentUserCreateArguments(
                login: SessionData.Login,
                name: SessionData.Name
            ));

            var result = await PaymentMediaHandler.ExecuteAsync(new PaymentMediaMobileCreateWebCardArguments(
                name: arguments.Name,
                pin: arguments.Pin,
                bankEntity: "",
                deviceManufacturer: arguments.DeviceManufacturer,
                deviceModel: arguments.DeviceModel,
                deviceName: arguments.DeviceName,
                deviceSerial: arguments.DeviceSerial,
                deviceId: arguments.DeviceId,
                deviceOperator: arguments.DeviceOperator,
                deviceImei: arguments.DeviceImei,
                deviceMac: arguments.DeviceMac,
                operatorSim: arguments.OperatorSim,
                operatorMobile : arguments.OperatorMobile
            ));

            return result;

            //var now = DateTime.Now;

            //var concessions = await RepositoryPaymentConcession.GetAsync("Concession.Supplier");
            //var concession = concessions
            //    .Where(x => x.Concession.Supplier.Login == SessionData.Login)
            //    .FirstOrDefault();

            ////No pueden haber 2 clientes con el mismo mail
            //var user = (await PaymentUserRepository.GetAsync())
            //    .Where(x => x.Login == arguments.Login)
            //    .FirstOrDefault();

            //if (user != null && (user.State == PaymentUserState.Active || user.State == PaymentUserState.Pending || user.State == PaymentUserState.Blocked))
            //    throw new Exception(PaymentUserResources.ExceptionUserMailAlreadyExists);

            //if (user != null && (user.State == PaymentUserState.Deleted || user.State == PaymentUserState.Unsuscribed))
            //{
            //    user.State = PaymentUserState.Pending;

            //    await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
            //        type: NotificationType.ConcessionVinculation,
            //        message: PaymentUserResources.AcceptAssociationMessage.FormatString(concession.Concession.Supplier.Name),
            //        referenceId: user.Id,
            //        referenceClass: "PaymentUser",
            //        senderLogin: concession.Concession.Supplier.Login,
            //        receiverLogin: arguments.Login
            //    ));
            //}
            //else
            //{
            //    var paymentuser = new PaymentUser
            //    {
            //        Name = SessionData.Name,
            //        Login = SessionData.Login,
            //        State = PaymentUserState.Pending,
            //        Concession = concession
            //    };
            //    await PaymentUserRepository.AddAsync(paymentuser);
            //    await UnitOfWork.SaveAsync();

            //    await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
            //        type: NotificationType.ConcessionVinculation,
            //        message: PaymentUserResources.AcceptAssociationMessage.FormatString(concession.Concession.Supplier.Name),
            //        referenceId: paymentuser.Id,
            //        referenceClass: "PaymentUser",
            //        senderLogin: concession.Concession.Supplier.Login,
            //        receiverLogin: arguments.Login
            //    ));
            //}

            //var max = (await PaymentMediaRepository.GetAsync())
            //    .Max(x => (int?)x.VisualOrder) ?? 0;

            //// Calcular OrderId
            //var lastOrderId = (await ServiceOptionRepository.GetAsync())
            //    .Where(x => x.Name == "LastOrderId")
            //    .FirstOrDefault();
            //var order = Convert.ToInt32(lastOrderId.Value) + 1;
            //lastOrderId.Value = Convert.ToString(order);

            //// Cargar concession
            //var payinConcession = (await PaymentConcessionRepository.GetAsync("Concession.Supplier"))
            //    .Where(x =>
            //        x.Concession.Supplier.Login == "info@pay-in.es" &&
            //        x.Concession.Type == ServiceType.Charge
            //    ).FirstOrDefault();

            //if (payinConcession == null)
            //    throw new Exception("info@pay-in.es needs a charge concession.");

            //// Crear Tarjeta
            //var paymentMedia = new PaymentMedia
            //{
            //    Name = arguments.Name,
            //    NumberHash = "**** **** **** ****",
            //    Type = PaymentMediaType.WebCard,
            //    VisualOrder = max + 1,
            //    VisualOrderFavorite = null,
            //    State = PaymentMediaState.Pending,
            //    BankEntity = arguments.BankEntity,
            //    Login = SessionData.Login
            //};
            //await PaymentMediaRepository.AddAsync(paymentMedia);

            //// Crear ticket
            //var ticket = new Ticket
            //{
            //    Amount = 0,
            //    Concession = payinConcession,
            //    Date = now.ToUTC(),
            //    Reference = "",
            //    State = TicketStateType.Active,
            //    SupplierName = payinConcession.Concession.Supplier.Name,
            //    TaxAddress = payinConcession.Concession.Supplier.TaxAddress,
            //    TaxName = payinConcession.Concession.Supplier.TaxName,
            //    TaxNumber = payinConcession.Concession.Supplier.TaxNumber,
            //    Title = "Card validation",
            //    Since = now.ToUTC(),
            //    Until = now.AddHours(6).ToUTC(),
            //    TextUrl = "",
            //    PdfUrl = ""
            //};
            //await TicketRepository.AddAsync(ticket);

            //// Crear pago
            //var payment = new Payment
            //{
            //    AuthorizationCode = "",
            //    Amount = 1,
            //    Date = now.ToUTC(),
            //    Order = order,
            //    Payin = 0,
            //    PaymentMedia = paymentMedia,
            //    ErrorCode = "",
            //    ErrorText = "",
            //    ErrorPublic = "",
            //    State = PaymentState.Pending,
            //    TaxAddress = SessionData.TaxAddress,
            //    TaxName = SessionData.TaxName,
            //    TaxNumber = SessionData.TaxNumber,
            //    Ticket = ticket,
            //    UserLogin = SessionData.Login,
            //    UserName = SessionData.Name
            //};
            //await PaymentRepository.AddAsync(payment);
            //await UnitOfWork.SaveAsync();

            //// Ejecutar pago
            //var result = await InternalService.PaymentMediaCreateWebCardAsync(arguments.Pin, arguments.Name, order, paymentMedia.Id, ticket.Id, payment.Id, arguments.BankEntity,
            //    arguments.DeviceManufacturer, arguments.DeviceModel, arguments.DeviceName, arguments.DeviceSerial, arguments.DeviceId, arguments.DeviceOperator, arguments.DeviceImei, arguments.DeviceMac, arguments.OperatorSim, arguments.OperatorMobile);

            //return result;
        }
        #endregion ExecuteAsync
    }
}
