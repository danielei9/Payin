using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentUserMobileRejectAssignmentHandler : IServiceBaseHandler<PaymentUserMobileRejectAssignmentArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentUser> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentUserMobileRejectAssignmentHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentUser> repository,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");

			SessionData = sessionData;
			Repository = repository;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PaymentUserMobileRejectAssignmentArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			item.State = PaymentUserState.Unsuscribed;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculationRefused,
				message: PaymentUserResources.AssociationRefused.FormatString(item.Name, item.Concession.Concession.Supplier.Name),
				referenceId: arguments.Id,
				referenceClass: "PaymentUser",
				senderLogin: item.Login,
				receiverLogin: item.Concession.Concession.Supplier.Login
			));
			return item;
		}
		#endregion ExecuteAsync
	}
}