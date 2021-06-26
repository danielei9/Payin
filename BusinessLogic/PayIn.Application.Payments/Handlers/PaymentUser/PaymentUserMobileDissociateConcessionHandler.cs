using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.Application.Public.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentUserMobileDissociateConcessionHandler : IServiceBaseHandler<PaymentUserMobileDissociateConcessionArguments>
	{
		private readonly IEntityRepository<PaymentUser> Repository;
		private readonly ISessionData SessionData;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PaymentUserMobileDissociateConcessionHandler(
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
		public async Task<dynamic> ExecuteAsync(PaymentUserMobileDissociateConcessionArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
			    type: NotificationType.PaymentUserConcessionDissociation,
			    message: PaymentUserResources.UserDissociationMessage.FormatString(item.Name,item.Concession.Concession.Supplier.Name),
			    referenceId: item.Id,
			    referenceClass: "PaymentUser",
			    senderLogin: SessionData.Login,
			    receiverLogin: item.Concession.Concession.Supplier.Login
			));

			item.State = PaymentUserState.Unsuscribed;
			return null;
		}
		#endregion ExecuteAsync
	}

}
