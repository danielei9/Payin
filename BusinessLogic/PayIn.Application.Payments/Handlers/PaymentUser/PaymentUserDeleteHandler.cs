using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments.PaymentUser;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentUserDeleteHandler :
		IServiceBaseHandler<PaymentUserDeleteArguments>
	{
		private readonly IEntityRepository<PaymentUser> Repository;
		private readonly ISessionData SessionData;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;


		#region Constructors
		public PaymentUserDeleteHandler(ISessionData sessionData, IEntityRepository<PaymentUser> repository, ServiceNotificationCreateHandler serviceNotificationCreate)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;

			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			ServiceNotificationCreate = serviceNotificationCreate;

			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region PaymentUserDelete
		public async Task<dynamic> ExecuteAsync(PaymentUserDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync("Concession.Concession.Supplier","Tickets"))		
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			if (item.Tickets.Where(x => x.State == TicketStateType.Pending || (x.Payments.Sum(y => y.Amount) < x.Amount)).Count() > 0)
				throw new ApplicationException(PaymentUserResources.DeleteException);


			await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
				type: NotificationType.ConcessionDissociation,
				message: PaymentUserResources.DissociationMessage.FormatString(item.Concession.Concession.Supplier.Name),
				referenceId: item.Id,
				referenceClass: "PaymentUser",
				senderLogin: SessionData.Login,
				receiverLogin: item.Login
			));

			item.State = PaymentUserState.Deleted;
			
			return null;
		}
		#endregion PaymentUserDelete
	}
}
