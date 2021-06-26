using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments;
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
	public class PaymentConcessionPurseMobileAcceptAssignmentHandler : IServiceBaseHandler<PaymentConcessionPurseMobileAcceptAssignmentArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<PaymentConcessionPurse> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentConcessionPurseMobileAcceptAssignmentHandler(
			ISessionData sessionData,
			IEntityRepository<PaymentConcessionPurse> repository,
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
		public async Task<dynamic> ExecuteAsync(PaymentConcessionPurseMobileAcceptAssignmentArguments arguments)
		{
			var item = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new
				{
					Name = x.PaymentConcession.Concession.Supplier.Name,
					PurseName = x.Purse.Name,
					PurseLogin = x.Purse.Concession.Concession.Supplier.Login,
					Item = x
				})
				.FirstOrDefault();

			item.Item.State = PaymentConcessionPurseState.Active;

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionVinculationAccepted,
				message: PaymentConcessionPurseResources.AssociationAccepted.FormatString(item.Name, item.PurseName),
				referenceId: arguments.Id,
				referenceClass: "PaymentConcessionPurse",
				senderLogin: SessionData.Login,
				receiverLogin: item.PurseLogin
			));
			return item.Item;
		}
		#endregion ExecuteAsync
	}
}