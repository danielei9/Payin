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
using PayIn.Domain.Promotions;
using PayIn.Application.Payments.Services;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionCreateHandler :
		IServiceBaseHandler<PromotionCreateArguments>
	{
		private readonly PromotionService PromotionService;
		private readonly ISessionData SessionData;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<Promotion> Repository;
		private readonly IEntityRepository<PromoCondition> ConditionRepository;
		private readonly IEntityRepository<PromoAction> ActionRepository;
		private readonly IEntityRepository<PromoLauncher> LauncherRepository;
		private readonly IEntityRepository<PromoExecution> ExecutionRepository;
		private readonly IEntityRepository<PromoPrice> PromoPriceRepository;
		private readonly IEntityRepository<PaymentConcession> ConcessionRepository;

		#region Constructors
		public PromotionCreateHandler(
			PromotionService promotionService,
			ISessionData sessionData,
			IUnitOfWork unitOfWork,
			IEntityRepository<Promotion> repository,
			IEntityRepository<PromoCondition> conditionRepository,
			IEntityRepository<PromoAction> actionRepository,
			IEntityRepository<PromoLauncher> launcherRepository,
			IEntityRepository<PromoExecution> executionRepository,
			IEntityRepository<PromoPrice> promoPriceRepository,
			IEntityRepository<PaymentConcession> concessionRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (promotionService == null) throw new ArgumentNullException("promotionService");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (repository == null) throw new ArgumentNullException("repository");
			if (conditionRepository == null) throw new ArgumentNullException("conditionRepository");
			if (actionRepository == null) throw new ArgumentNullException("actionRepository");
			if (launcherRepository == null) throw new ArgumentNullException("launcherRepository");
			if (executionRepository == null) throw new ArgumentNullException("executionRepository");
			if (promoPriceRepository == null) throw new ArgumentNullException("promoPriceRepository");
			if(concessionRepository == null) throw new ArgumentNullException("concessionRepository");

			SessionData = sessionData;
			PromotionService = promotionService;
			UnitOfWork = unitOfWork;
			Repository = repository;
			ConditionRepository = conditionRepository;
			LauncherRepository = launcherRepository;
			ExecutionRepository = executionRepository;
			ActionRepository = actionRepository;
			PromoPriceRepository = promoPriceRepository;
			ConcessionRepository = concessionRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PromotionCreateArguments arguments)
		{
			var now = DateTime.Now;

			if (arguments.StartDate.Value >= arguments.EndDate.Value)
				throw new ApplicationException(CampaignResources.UntilPreviousThanSinceException);

			if (arguments.EndDate.Value <= now)
				throw new ApplicationException(CampaignResources.UntilInvalidDateException);

			var concession = (await ConcessionRepository.GetAsync())
				.Where(x => x.Id == arguments.Concession)
				.FirstOrDefault();

			var promotion = new Promotion
			{
				Name = arguments.Name,
				StartDate = arguments.StartDate,
				EndDate = arguments.EndDate,
				State = PromotionState.Active,
				Acumulative = 0,
				ConcessionId = concession.Id
			};
			await Repository.AddAsync(promotion);
			await UnitOfWork.SaveAsync();

			foreach(var price in arguments.PromoPrices)
			{
				var promoPrice = new PromoPrice
				{
					TransportPriceId = price.Id,
					PromotionId = promotion.Id
				};
				await PromoPriceRepository.AddAsync(promoPrice);
				await UnitOfWork.SaveAsync();
			}

			foreach (var item in arguments.PromoConditions)
			{
				if (item.condition != 4)
				{
					var promoCondition = new PromoCondition
					{
						Quantity = item.quantity,
						Type = item.condition,
						Promotion = promotion,
						//TransportTitleId = arguments.Title
					};
					await ConditionRepository.AddAsync(promoCondition);
				}
				else
				{
					promotion.Acumulative = item.quantity;
				}
			}

			var promoAction = new PromoAction
			{
				Quantity = arguments.PromoActions,
				Type = PromoActionType.MoreTravel,
				PromotionId = promotion.Id
			};
			await ActionRepository.AddAsync(promoAction);
			
			if(arguments.PromoLaunchers != -1)
			{
				var promoLauncher = new PromoLauncher
				{
					Quantity = 1,
					Type = (arguments.PromoLaunchers == 0) ? PromoLauncherType.Recharge : PromoLauncherType.Instant,
					Promotion = promotion
				};
				await LauncherRepository.AddAsync(promoLauncher);
			}

			for (int i = 0; i < arguments.Quantity; i++)
			{
				var code =  PromotionService.CreateCode(PromotionExecutionType.AlphaNumeric, promotion.Id, i);
				var execution = new PromoExecution
				{
			        Code = code,
					Promotion = promotion,
					Type = PromotionExecutionType.AlphaNumeric,
					Cost = 0,
					State =  PromotionCodeState.Active					
				};
				await ExecutionRepository.AddAsync(execution);
			}
			
			return promotion;
		}
		#endregion ExecuteAsync
	}
}

