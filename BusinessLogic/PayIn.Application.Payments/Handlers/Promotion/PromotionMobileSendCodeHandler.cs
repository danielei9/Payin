using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common.Resources;
using PayIn.Common;
using Xp.Common.Exceptions;
using System.Collections.Generic;
using Xp.Common;
using PayIn.Domain.Payments;
using PayIn.Application.Payments.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PayIn.Application.Public.Handlers;
using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Domain.Promotions;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionMobileSendCodeHandler :
		IServiceBaseHandler<PromotionMobileSendCodeArguments>
	{		
		private readonly ISessionData SessionData;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<PromoExecution> Repository;
		private readonly IEntityRepository<PromoUser> PromoUserRepository;
		private readonly ServiceNotificationCreateHandler ServiceNotificationCreate;
		
		#region Constructors
		public PromotionMobileSendCodeHandler(
		    ServiceNotificationCreateHandler serviceNotificationCreate,
		    ISessionData sessionData,
			IUnitOfWork unitOfWork,
			IEntityRepository<PromoExecution> repository,		
			IEntityRepository<PromoUser> promoUserRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (serviceNotificationCreate == null) throw new ArgumentNullException("serviceNotificationCreate");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (repository == null) throw new ArgumentNullException("executionRepository");		
			if (promoUserRepository == null) throw new ArgumentNullException("userRepository");

			SessionData = sessionData;
			ServiceNotificationCreate = serviceNotificationCreate;
			UnitOfWork = unitOfWork;
			Repository = repository;			
			PromoUserRepository = promoUserRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PromotionMobileSendCodeArguments arguments)
		{
			
			if(arguments.Promotion.Count() >1)
			{
				throw new ApplicationException(PromotionResources.SendCodeError);
			}
			var promotion = (await Repository.GetAsync("PromoUser"));

			foreach (var promo in arguments.Promotion)
			{
				var exists = promotion
						 .Where(x => x.Id == promo.Id && x.Code == promo.Code && x.PromoUserId != null && x.PromoUser.Login == SessionData.Login)
						 .FirstOrDefault();

				if(exists != null)
				{
					exists.PromoUser.Login = arguments.Email;

					await ServiceNotificationCreate.ExecuteAsync(new ServiceNotificationCreateArguments(
					type: NotificationType.SendPromotionCode,
					message: PromotionResources.SendCodeMessage.FormatString(SessionData.Login),
					referenceId: exists.Id,
					referenceClass: "PromoExecution",
					senderLogin: SessionData.Login,
					receiverLogin: arguments.Email
				));
				}
			}
			

			
			return promotion;
		}
		#endregion ExecuteAsync
	}
}

