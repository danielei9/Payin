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
using PayIn.Domain.Promotions;
using PayIn.Application.Payments.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionUpdateHandler :
		IServiceBaseHandler<PromotionUpdateArguments>
	{
		private readonly PromotionService PromotionService;
		private readonly ISessionData SessionData;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<Promotion> Repository;
		private readonly IEntityRepository<PromoCondition> ConditionRepository;
		private readonly IEntityRepository<PromoAction> ActionRepository;
		private readonly IEntityRepository<PromoLauncher> LauncherRepository;
		private readonly IEntityRepository<PromoExecution> ExecutionRepository;


		#region Constructors
		public PromotionUpdateHandler(
			PromotionService promotionService,
			ISessionData sessionData,
			IUnitOfWork unitOfWork,
			IEntityRepository<Promotion> repository,
			IEntityRepository<PromoCondition> conditionRepository,
			IEntityRepository<PromoAction> actionRepository,
			IEntityRepository<PromoLauncher> launcherRepository,
			IEntityRepository<PromoExecution> executionRepository
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

			SessionData = sessionData;
			PromotionService = promotionService;
			UnitOfWork = unitOfWork;
			Repository = repository;
			ConditionRepository = conditionRepository;
			LauncherRepository = launcherRepository;
			ExecutionRepository = executionRepository;
			ActionRepository = actionRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PromotionUpdateArguments arguments)
		{
			var now = new XpDate(DateTime.Now);

			if (arguments.StartDate.Value >= arguments.EndDate.Value)
				throw new ApplicationException(CampaignResources.UntilPreviousThanSinceException);

			if (arguments.EndDate.Value <= now)
				throw new ApplicationException(CampaignResources.UntilInvalidDateException);

			var promotion = (await Repository.GetAsync())
				         .Where(x => x.Id == arguments.Id)
						 .FirstOrDefault();

			promotion.Name = arguments.Name;
			promotion.Acumulative = arguments.Acumulative;
			promotion.EndDate = arguments.EndDate;
			promotion.StartDate = arguments.StartDate;
			
			await UnitOfWork.SaveAsync();

			//Actualizar condiciones
			var Oldconditions = (await ConditionRepository.GetAsync())
				.Where(x => x.PromotionId == promotion.Id);

			foreach (var condition in Oldconditions)
				await ConditionRepository.DeleteAsync(condition);

			foreach (var item in arguments.PromoConditions)
			{				
				var promoCondition = new PromoCondition
				{
					Quantity = item.quantity,
					Type = item.type,
					Promotion = promotion
				};
				await ConditionRepository.AddAsync(promoCondition);	
				
			}

			//Actualizar acciones
			var oldActions = (await ActionRepository.GetAsync())
				.Where(x => x.PromotionId == promotion.Id);

			foreach (var action in oldActions)
				await ActionRepository.DeleteAsync(action);

			var promoAction = new PromoAction
			{
				Quantity = arguments.PromoActions,
				Type = PromoActionType.MoreTravel,
				Promotion = promotion
			};
			await ActionRepository.AddAsync(promoAction);


			//Actualizar lanzadores
			var oldLaunchers = (await LauncherRepository.GetAsync())
				.Where(x => x.PromotionId == promotion.Id);

			foreach (var launcher in oldLaunchers)
				await LauncherRepository.DeleteAsync(launcher);

			if(arguments.PromoLaunchers == true)
			{
				var promoLauncher = new PromoLauncher
				{
					Quantity = 1,
					Type = PromoLauncherType.Recharge,
					Promotion = promotion
				};
				await LauncherRepository.AddAsync(promoLauncher);
			}
			return promotion;
		}
		#endregion ExecuteAsync
	}
}

