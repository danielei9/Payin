using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Common;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Public.Handlers;
using PayIn.Common.Resources;
namespace PayIn.Application.Payments.Handlers

{
	[XpLog("Purse", "Close")]
	public class PurseCloseHandler :
		IServiceBaseHandler<PurseCloseArguments>
	{
		private readonly IEntityRepository<Purse> Repository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;

		#region Constructors
		public PurseCloseHandler(
			IEntityRepository<Purse> repository,
			ServiceNotificationCreateHandler serviceNotificationCreate
		)
		{
			if (repository == null) throw new ArgumentNullException("purseRepository");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			Repository = repository;
			ServiceNotificationCreate = serviceNotificationCreate;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PurseCloseArguments arguments)
		{
			var now = DateTime.Now;
			var Purses = (await Repository.GetAsync("Concession.Concession.Supplier"))
				.Where(x => (x.Expiration.Year == now.Year && x.Expiration.Month == now.Month && x.Expiration.Day == now.Day) ||
							(x.Validity.Year == now.Year && x.Validity.Month == now.Month && x.Validity.Day == now.Day));
			var LastDayPurses = (await Repository.GetAsync("Concession.Concession.Supplier"))
			.Where(x => (x.Expiration.Year == now.Year && x.Expiration.Month == now.Month && x.Expiration.Day+1 == now.Day) ||
						(x.Validity.Year == now.Year && x.Validity.Month == now.Month && x.Validity.Day+1 == now.Day));

			foreach (var lastDayPurse in LastDayPurses){
				foreach(var paymentMedia in lastDayPurse.PaymentMedias){
				await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.PaymentMediaLastDay,
					message: PurseResources.PaymentMediaLastDay.FormatString(lastDayPurse.Name),
					referenceId: paymentMedia.Id,
					referenceClass: "PaymentMedia",
					senderLogin: lastDayPurse.Concession.Concession.Supplier.Login,
					receiverLogin: paymentMedia.Login
				));
				}
			}

			foreach (var purse in Purses)
			{
				purse.State = PurseState.Deleted;

				foreach (var campaignLine in purse.CampaignLines)
				{
					campaignLine.State = CampaignLineState.Deleted;
				}

						await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
							type: NotificationType.PurseDeleted,
							message: PurseResources.PurseExpired.FormatString(purse.Name),
							referenceId: purse.Id,
							referenceClass: "Purse",
							senderLogin: "info@pay-in.es",
							receiverLogin: purse.Concession.Concession.Supplier.Login
					));
			}
			
			return null;
		}
		#endregion ExecuteAsync
	}
}
