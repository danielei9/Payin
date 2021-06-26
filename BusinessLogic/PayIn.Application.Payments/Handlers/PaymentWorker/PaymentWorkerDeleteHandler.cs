using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentWorkerDeleteHandler : IServiceBaseHandler<PaymentWorkerDeleteArguments>
	{
		private readonly IEntityRepository<PaymentWorker> Repository;
		private readonly ISessionData SessionData;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentWorkerDeleteHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentWorker> repository,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;

			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;

			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentWorkerDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
			type: NotificationType.ConcessionDissociation,
			message: PaymentWorkerResources.DissociationMessage.FormatString(item.Concession.Concession.Supplier.Name),
			referenceId: item.Id,
			referenceClass: "PaymentWorker",
			senderLogin: SessionData.Login,
			receiverLogin: item.Login
			));

			item.State = WorkerState.Deleted;

			var securityRepository = new SecurityRepository();
			await securityRepository.RemoveRole(item.Login, AccountRoles.PaymentWorker);
			return null;
		}
		#endregion ExecuteAsync
	}

}
