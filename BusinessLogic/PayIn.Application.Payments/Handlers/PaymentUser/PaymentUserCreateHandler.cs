using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common.Resources;
using PayIn.Common;
using Xp.Common.Exceptions;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentUserCreateHandler :
		IServiceBaseHandler<PaymentUserCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentUser> Repository;
		private readonly IEntityRepository<PaymentConcession> RepositoryPaymentConcession;
		private readonly IUnitOfWork UnitOfWork;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentUserCreateHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentUser> repository,
			IEntityRepository<PaymentConcession> repositoryPaymentConcession,
			IUnitOfWork unitOfWork,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPaymentConcession == null) throw new ArgumentNullException("repositoryPaymentConcession");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");		
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

			SessionData = sessionData;
			Repository = repository;
			RepositoryPaymentConcession = repositoryPaymentConcession;
			UnitOfWork = unitOfWork;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
        #endregion Constructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(PaymentUserCreateArguments arguments)
        {
			var concessions = await RepositoryPaymentConcession.GetAsync("Concession.Supplier");
			var concession = concessions
				.Where(x => x.Concession.Supplier.Login == SessionData.Login).FirstOrDefault();

			var securityRepository = new SecurityRepository();
			var user = await securityRepository.GetUserAsync(arguments.Login);
			if (user == null)
				throw new Exception(ApplicationUserResources.EmailNotRegistered);
			if (!user.EmailConfirmed)
				throw new Exception(ApplicationUserResources.EmailNotConfirmed);

			if (arguments.Login == SessionData.Login)
				throw new Exception(PaymentUserResources.EmailClientCommerce);

			//No pueden haber 2 clientes con el mismo mail
			var client = (await Repository.GetAsync())
				.Where(x => x.Login == arguments.Login)
				.FirstOrDefault();

			if (client != null && (client.State == PaymentUserState.Active || client.State == PaymentUserState.Pending || client.State == PaymentUserState.Blocked))
				throw new Exception(PaymentUserResources.ExceptionUserMailAlreadyExists);

			if (client != null && (client.State == PaymentUserState.Deleted || client.State == PaymentUserState.Unsuscribed))
			{
				client.State = PaymentUserState.Pending;

				await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculation,
				message: PaymentUserResources.AcceptAssociationMessage.FormatString(concession.Concession.Supplier.Name),
				referenceId: client.Id,
				referenceClass: "PaymentUser",
				senderLogin: concession.Concession.Supplier.Login,
				receiverLogin: arguments.Login
				));
				return client;
			}

			var paymentuser = new PaymentUser
			{
				Name = arguments.Name,
				Login = arguments.Login,
				State = PaymentUserState.Pending,
				Concession = concession
			};
			await Repository.AddAsync(paymentuser);
			await UnitOfWork.SaveAsync();


			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculation,
				message: PaymentUserResources.AcceptAssociationMessage.FormatString(concession.Concession.Supplier.Name),
				referenceId: paymentuser.Id,
				referenceClass: "PaymentUser",
				senderLogin: concession.Concession.Supplier.Login,
				receiverLogin: arguments.Login
			));

			return paymentuser;
		}
		#endregion ExecuteAsync
	}
}

