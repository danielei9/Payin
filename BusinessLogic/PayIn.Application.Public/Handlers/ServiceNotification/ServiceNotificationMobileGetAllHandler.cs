using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.ServiceNotification;
using PayIn.Application.Dto.Results.ServiceNotification;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceNotificationMobileGetAllHandler :
		IQueryBaseHandler<ServiceNotificationMobileGetAllArguments, ServiceNotificationMobileGetAllResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IUnitOfWork UnitOfWork { get; set; }
		[Dependency] public IEntityRepository<ServiceNotification> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentWorker> PaymentWorkerRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentUser> PaymentUserRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceWorker> ServiceWorkerRepository { get; set; }
		[Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcessionCampaign> PaymentConcessionCampaignRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcessionPurse> PaymentConcessionPurseRepository { get; set; }
		//[Dependency] public IEntityRepository<Notice> NopticeRepository { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<ServiceNotificationMobileGetAllResult>> ExecuteAsync(ServiceNotificationMobileGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.ReceiverLogin == SessionData.Login);

			var paymentWorkers = await PaymentWorkerRepository.GetAsync();
			var paymentUsers = await PaymentUserRepository.GetAsync();
			var controlWorkers = await ServiceWorkerRepository.GetAsync();
			var tickets = await TicketRepository.GetAsync();
			var paymentConcessionCampaigns = await PaymentConcessionCampaignRepository.GetAsync();
			var paymentConcessionPurses = await PaymentConcessionPurseRepository.GetAsync();
			//var notices = await NopticeRepository.GetAsync();

			var result2 = items
				.Select(x => new
				{
					Id = x.Id,
					Date = x.CreatedAt,
					State = x.State,
					Type = x.Type,
					ReferenceId = x.ReferenceId,
					ReferenceClass = x.ReferenceClass,
					SenderLogin = x.SenderLogin,
					PersonalizedMessage = x.Message,
					ControlWorker = controlWorkers
						.Where(y =>
							x.ReferenceClass == "ServiceWorker" &&
							y.Id == x.ReferenceId
						)
						.FirstOrDefault(),
					PaymentWorker = paymentWorkers
						.Where(y =>
							x.ReferenceClass == "PaymentWorker" &&
							y.Id == x.ReferenceId
						)
						.FirstOrDefault(),
					PaymentUser = paymentUsers
						.Where(y =>
							x.ReferenceClass == "PaymentUser" &&
							y.Id == x.ReferenceId
						)
						.FirstOrDefault(),
					PaymentConcessionCampaign = paymentConcessionCampaigns
						.Where(y =>
							x.ReferenceClass == "PaymentConcessionCampaign" &&
							y.Id == x.ReferenceId
						)
						.FirstOrDefault(),
					PaymentConcessionPurse = paymentConcessionPurses
						.Where(y =>
							x.ReferenceClass == "PaymentConcessionPurse" &&
							y.Id == x.ReferenceId
						)
						.FirstOrDefault(),
					Ticket = tickets
						.Where(y =>
							x.ReferenceClass == "Ticket" &&
							y.Id == x.ReferenceId
						)
						.FirstOrDefault(),
					//Notice = notices
					//	.Where(y =>
					//		(x.ReferenceClass == "Notice" || x.ReferenceClass == "Page" || x.ReferenceClass == "Edict") &&
					//		y.Id == x.ReferenceId
					//	)
					//	.FirstOrDefault(),
				})
				.OrderByDescending(x => x.Date)
				.Skip(arguments.Skip)
				.Take(arguments.Top)
				.Select(x => new
				{
					Id = x.Id,
					Date = x.Date,
					State = x.State,
					Type = x.Type,
					ReferenceId = x.ReferenceId,
					ReferenceClass = x.ReferenceClass,
					SenderLogin = x.SenderLogin,
					PersonalizedMessage = x.PersonalizedMessage,
					ControlWorker = x.ControlWorker == null ? "" : x.ControlWorker.Name,
					ControlWorkerSupplier = x.ControlWorker == null ? "" : x.ControlWorker.Supplier.Name,
					PaymentWorker = x.PaymentWorker == null ? "" : x.PaymentWorker.Name,
					PaymentWorkerSupplier = x.PaymentWorker == null ? "" : x.PaymentWorker.Concession.Concession.Supplier.Name,
					PaymentUser = x.PaymentUser == null ? "" : x.PaymentUser.Name,
					PaymentUserSupplier = x.PaymentUser == null ? "" : x.PaymentUser.Concession.Concession.Supplier.Name,
					PaymentConcessionCampaign = x.PaymentConcessionCampaign == null ? "" : x.PaymentConcessionCampaign.Campaign.Title,
					PaymentConcessionCampaignUser = x.PaymentConcessionCampaign == null ? "" : x.PaymentConcessionCampaign.PaymentConcession.Concession.Supplier.Name,
					PaymentConcessionPurse = x.PaymentConcessionPurse == null ? "" : x.PaymentConcessionPurse.Purse.Name,
					PaymentConcessionPurseUser = x.PaymentConcessionPurse == null ? "" : x.PaymentConcessionPurse.PaymentConcession.Concession.Supplier.Name,
					TicketAmount = x.Ticket == null ? 0m : x.Ticket.Amount,
					TicketPayer = x.Ticket == null ? "" : x.Ticket.Payments
						.Where(y => y.Amount > 0)
						.Select(y => y.UserName)
						.FirstOrDefault() ?? "",
					PaymentMediaName = (x.Ticket == null || x.Ticket.Payments.Count == 0) ? "" : x.Ticket.Payments.FirstOrDefault().PaymentMedia.Name,
					TicketSupplier = x.Ticket == null ? "" : x.Ticket.Concession.Concession.Supplier.Name,
					TicketSince = x.Ticket == null ? (DateTime?) null : x.Ticket.Since,
					TicketUntil = x.Ticket == null ? (DateTime?)null : x.Ticket.Until,
					TicketErrorCode = x.Ticket == null ? "" : x.Ticket.Payments
						.Select(y => y.ErrorCode)
						.FirstOrDefault() ?? "",
					TicketErrorText = x.Ticket == null ? "" : x.Ticket.Payments
						.Select(y => y.ErrorText)
						.FirstOrDefault() ?? "",
					//Notice = x.Notice
				})
				.ToList();
			var result = result2
				.Select(x => new ServiceNotificationMobileGetAllResult
				{
					Id = x.Id,
					Date = x.Date.ToUTC(),
					State = x.State,
					Type = x.Type,
					Message =
						(x.Type == NotificationType.TicketActive && x.TicketUntil != null) ?
							TicketResources.TicketActive.FormatString(x.TicketAmount, x.TicketSupplier,x.TicketUntil.Value) :
						(x.Type == NotificationType.TicketActiveToday && x.TicketSince != null && x.TicketUntil != null) ?
							TicketResources.TicketActiveToday.FormatString(x.TicketAmount, x.TicketSupplier, x.TicketSince.Value.ToUTC().TimeOfDay,x.TicketUntil.Value) :
						(x.Type == NotificationType.TicketLastDay) ?
							TicketResources.TicketLastDay.FormatString(x.TicketAmount,x.TicketSupplier,x.TicketUntil.Value.ToUTC().TimeOfDay) :
						(x.Type == NotificationType.ConcessionVinculation && x.ReferenceClass == "PaymentWorker") ?
							PaymentWorkerResources.AcceptAssociationMessage.FormatString(x.PaymentWorkerSupplier) :
						(x.Type == NotificationType.PaymentConcessionCampaign && x.ReferenceClass == "PaymentConcessionCampaign") ?
							PaymentConcessionCampaignResources.AcceptAssociationMessage.FormatString(x.PaymentConcessionCampaignUser) :
						(x.Type == NotificationType.PaymentConcessionPurse && x.ReferenceClass == "PaymentConcessionPurse") ?
							PaymentConcessionCampaignResources.AcceptAssociationMessagePurse.FormatString(x.PaymentConcessionPurseUser) :
						(x.Type == NotificationType.ConcessionVinculationAccepted && x.ReferenceClass == "PaymentConcessionCampaign") ?
							PaymentConcessionCampaignResources.AssociationAccepted.FormatString(x.PaymentConcessionCampaignUser,x.PaymentConcessionCampaign) :
						(x.Type == NotificationType.ConcessionVinculationRefused && x.ReferenceClass == "PaymentConcessionCampaign") ?
							PaymentConcessionCampaignResources.AssociationRefused.FormatString(x.PaymentConcessionCampaignUser, x.PaymentConcessionCampaign) :
						(x.Type == NotificationType.ConcessionVinculationAccepted && x.ReferenceClass == "PaymentConcessionPurse") ?
							PaymentConcessionPurseResources.AssociationAccepted.FormatString(x.PaymentConcessionPurseUser, x.PaymentConcessionPurse) :
						(x.Type == NotificationType.ConcessionVinculationRefused && x.ReferenceClass == "PaymentConcessionPurse") ?
							PaymentConcessionPurseResources.AssociationRefused.FormatString(x.PaymentConcessionPurseUser, x.PaymentConcessionPurse) :
						(x.Type == NotificationType.ConcessionVinculation && x.ReferenceClass == "PaymentUser") ?
							PaymentUserResources.AcceptAssociationMessage.FormatString(x.PaymentUserSupplier) :
						(x.Type == NotificationType.AcceptConcessionVinculation && x.ReferenceClass == "PaymentWorker") ?
							PaymentWorkerResources.WorkerAssociationAccepted.FormatString(x.PaymentWorkerSupplier) :
						(x.Type == NotificationType.AcceptConcessionVinculation && x.ReferenceClass == "PaymentUser") ?
							PaymentUserResources.UserAssociationAccepted.FormatString(x.PaymentUserSupplier) :
						(x.Type == NotificationType.ConcessionVinculationAccepted && x.ReferenceClass == "PaymentWorker") ?
							PaymentWorkerResources.AssociationAccepted.FormatString(x.PaymentWorker, x.PaymentWorkerSupplier) :
						(x.Type == NotificationType.ConcessionVinculationRefused && x.ReferenceClass == "PaymentWorker") ?
							PaymentWorkerResources.AssociationRefused.FormatString(x.PaymentWorker, x.PaymentWorkerSupplier) :
						(x.Type == NotificationType.ConcessionVinculationAccepted && x.ReferenceClass == "ServiceWorker") ?
							ServiceWorkerResources.AssociationAccepted.FormatString(x.ControlWorker, x.ControlWorkerSupplier) :
						(x.Type == NotificationType.ConcessionVinculationRefused && x.ReferenceClass == "ServiceWorker") ?
							ServiceWorkerResources.AssociationRefused.FormatString( x.ControlWorker, x.ControlWorkerSupplier) :
						(x.Type == NotificationType.ConcessionVinculationAccepted && x.ReferenceClass == "PaymentUser") ?
							PaymentUserResources.AssociationAccepted.FormatString(x.PaymentUser, x.PaymentUserSupplier) :
						(x.Type == NotificationType.ConcessionVinculationRefused && x.ReferenceClass == "PaymentUser") ?
							PaymentUserResources.AssociationRefused.FormatString(x.PaymentUser, x.PaymentUserSupplier) :
						(x.Type == NotificationType.ConcessionVinculationAccepted && x.ReferenceClass == "PaymentConcessionCampaign") ?
							PaymentUserResources.AssociationAccepted.FormatString(x.PaymentConcessionCampaignUser, x.PaymentConcessionCampaign) :
						(x.Type == NotificationType.ConcessionVinculationRefused && x.ReferenceClass == "PaymentConcessionCampaign") ?
							PaymentUserResources.AssociationRefused.FormatString(x.PaymentConcessionCampaignUser, x.PaymentConcessionCampaign) :
						(x.Type == NotificationType.ConcessionVinculationAccepted && x.ReferenceClass == "PaymentConcessionPurse") ?
							PaymentUserResources.AssociationAccepted.FormatString(x.PaymentConcessionPurseUser, x.PaymentConcessionPurse) :
						(x.Type == NotificationType.ConcessionVinculationRefused && x.ReferenceClass == "PaymentConcessionPurse") ?
							PaymentUserResources.AssociationRefused.FormatString(x.PaymentConcessionPurseUser, x.PaymentConcessionPurse) :
						(x.Type == NotificationType.PaymentMediaCreateError) ?
							PaymentMediaResources.PaymentMediaCreateExceptionRefund :
						(x.Type == NotificationType.PaymentMediaCreateSucceed) ?
							PaymentMediaResources.PaymentMediaCreateSuccess :
						(x.Type == NotificationType.PaymentSucceed && x.SenderLogin == "info@pay-in.es") ?
							PaymentResources.PaymentMediaCreationPaymentPushMessage  :
						(x.Type == NotificationType.PaymentSucceed) ?
							PaymentResources.PaidPushMessage.FormatString(x.TicketAmount, x.TicketPayer, x.PaymentMediaName) :
						(x.Type == NotificationType.RefundSucceed && x.SenderLogin == "info@pay-in.es") ? 
							PaymentResources.PaymentMediaCreationRefundPushMessage :
						(x.Type == NotificationType.RefundSucceed) ? 
							PaymentResources.RefundedPushMessage.FormatString(x.TicketAmount, x.TicketSupplier) :
						(x.Type == NotificationType.RefundError) ?
							PaymentResources.PaymentMediaCreationRefundErrorPushMessage.FormatString(x.TicketAmount,
								ServiceNotificationResources.GatewayError.FormatString(x.TicketErrorCode, x.TicketErrorText)
							) :
						(x.Type == NotificationType.PaymentError) ? 
							PaymentResources.PaymentMediaCreationPaymentErrorPushMessage.FormatString(x.TicketAmount, 
								ServiceNotificationResources.GatewayError.FormatString(x.TicketErrorCode, x.TicketErrorText)
							) :
						(x.Type == NotificationType.ConcessionDissociation && x.ReferenceClass == "PaymentConcessionPurse") ?
							PaymentConcessionPurseResources.DissociationMessage.FormatString(x.PaymentConcessionPurse) :
						(x.Type == NotificationType.ConcessionDissociation && x.ReferenceClass == "PaymentConcessionCampaign") ?
							PaymentConcessionCampaignResources.DissociationMessage.FormatString(x.PaymentConcessionCampaign) :
						(x.Type == NotificationType.ConcessionDissociation  && x.ReferenceClass == "PaymentWorker") ? 
							PaymentWorkerResources.DissociationMessage.FormatString(x.PaymentWorkerSupplier) :
						(x.Type == NotificationType.ConcessionDissociation && x.ReferenceClass == "PaymentUser") ?
							PaymentUserResources.DissociationMessage.FormatString(x.PaymentUserSupplier) :
						(x.Type == NotificationType.PaymentWorkerConcessionDissociation) ? 
							PaymentWorkerResources.WorkerDissociationMessage.FormatString(x.PaymentWorker, x.PaymentWorkerSupplier) :
						(x.Type == NotificationType.PaymentUserConcessionDissociation) ?
							PaymentUserResources.UserDissociationMessage.FormatString(x.PaymentUser, x.PaymentUserSupplier) :
						(x.Type == NotificationType.Personalized || 
						 x.Type == NotificationType.EdictCreate || 
						 x.Type == NotificationType.EdictUpdate ||
						 x.Type == NotificationType.NoticeCreate ||
						 x.Type == NotificationType.NoticeUpdate ||
						 x.Type == NotificationType.PageCreate ||
						 x.Type == NotificationType.PageUpdate ||
						 x.Type == NotificationType.ServiceNotificationCreate) ?
#if VILAMARXANT
							PaymentUserResources.Personalized_TurismeVilamarxant.FormatString(x.PersonalizedMessage) :
#elif FINESTRAT
							PaymentUserResources.Personalized_Finestrat.FormatString(x.PersonalizedMessage) :
#else
							PaymentUserResources.Personalized.FormatString(x.PersonalizedMessage) :
#endif
						"",
					ReferenceId = x.ReferenceId,
					ReferenceClass = x.ReferenceClass,
					//ReferenceName = (x.Notice != null) ? x.Notice.Name : null
				});

			foreach (ServiceNotification item in items.Where(x => x.State == NotificationState.Actived))
				item.State = NotificationState.Read;

			await UnitOfWork.SaveAsync();

			return new ResultBase<ServiceNotificationMobileGetAllResult> { Data = result };
		}
#endregion ExecuteAsync
	}
}

