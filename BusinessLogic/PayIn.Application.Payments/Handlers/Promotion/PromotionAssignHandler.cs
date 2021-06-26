using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.Application.Dto.Payments.Results.Promotion;
using PayIn.Application.Payments.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Promotions;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionAssignHandler :
		IServiceBaseHandler<PromotionAsignArguments>
	{
		private readonly SessionData SessionData;
		private readonly PromotionService PromotionService;
		private readonly IEntityRepository<PromoExecution> PromoExecutionRepository;
		private readonly IEntityRepository<PromoUser> PromoUserRepository;

		#region Constructor
		public PromotionAssignHandler(
			SessionData sessionData,
			PromotionService promotionService,
			IEntityRepository<PromoExecution> promoExecutionRepository,
			IEntityRepository<PromoUser> promoUserRepository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (promotionService == null) throw new ArgumentNullException("promotionService");
			if (promoExecutionRepository == null) throw new ArgumentNullException("promoExecutionRepository");
			if (promoUserRepository == null) throw new ArgumentNullException("promoUserRepository");

			SessionData = sessionData;
			PromotionService = promotionService;
			PromoExecutionRepository = promoExecutionRepository;
			PromoUserRepository = promoUserRepository;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PromotionAsignArguments arguments)
		{
			var now = DateTime.Now.ToUTC();
			var code = PromotionService.ClearCode(arguments.Code);

			var executed = (await PromoExecutionRepository.GetAsync("PromoUser", "Promotion.PromoActions", "Promotion.Concession.Concession", "Promotion.PromoPrices.TransportPrice.Title.Prices"))
				.Where(x =>
					x.Code == code &&
					x.Type == arguments.PromotionCodeType &&
					x.State == PromotionCodeState.Active &&
					x.AppliedDate == null &&
					x.Promotion.State == PromotionState.Active &&
					x.Promotion.StartDate <= now &&
					x.Promotion.EndDate >= now &&
					(
						x.PromoUserId == null //||
						//x.PromoUser.Login == SessionData.Login
					) &&
					x.Promotion.PromoLaunchers
						.Where(y =>
							y.Type == PromoLauncherType.Instant
						)
						.Any()
				)
				.FirstOrDefault();
			if (executed == null)
				throw new ApplicationException(PromotionResources.CodeException);

			var promoUser = await GetPromoUserAsync(now);
			promoUser.LastChance = now;
			promoUser.Attemps = 1;
			executed.PromoUser = promoUser;

			// TODO: XAVI Revisar Promociones
			/*var result = new TransportOperationReadInfoResultBase.Promotion
			{
				Id = executed.Id,
				Name = executed.Promotion.Name,
				EndDate = executed.Promotion.EndDate,
				Concession = executed.Promotion.Concession.Concession.Name,
				Actions = executed.Promotion.PromoActions
					.Select(y => new TransportOperationReadInfoResultBase.PromotionAction
					{
						Type = y.Type,
						Quantity = y.Quantity
					}),
				Prices = executed.Promotion.PromoPrices
					.Select(y => new TransportOperationReadInfoResultBase.PromotionPrice
					{
						Id = y.TransportPriceId,
						Code = y.TransportPrice.Title.Code,
						Zone = y.TransportPrice.Zone,
						HasZone = y.TransportPrice.Title.HasZone,
						Name = y.TransportPrice.Title.Name + (y.TransportPrice.Title.HasZone ? " " + y.TransportPrice.Zone.ToEnumAlias("") : "")
					})
			};*/

			// PromoExecutionRepository.GetAsync("PromoUser", "Promotion.PromoActions", "Promotion.Concession.Concession", "Promotion.PromoPrices.TransportPrice.Title.Prices"))
			return new PromotionAssignResult
			{
				Name = executed.Promotion.Name,
				EndDate = executed.Promotion.EndDate,
				Id = executed.Id,
				Concession = executed.Promotion.Concession.Concession.Name,
				Actions = executed.Promotion.PromoActions
					.Select(y => new PromotionAssignResult.PromotionAction
					{
						Type = y.Type,
						Quantity = y.Quantity
					}),
				Titles = executed.Promotion.PromoPrices
					.Select(x => new PromotionAssignResult.RechargeTitle {
						Code = x.TransportPrice.Title.Code,
						Id = x.TransportPrice.Title.Id,
						MaxQuantity = x.TransportPrice.Title.MaxQuantity,
						MeanTransport = x.TransportPrice.Title.MeanTransport,
						Name = x.TransportPrice.Title.Name,
						OwnerCity = "Valencia",
						OwnerName = x.TransportPrice.Title.OwnerName,
						PaymentConcessionId = 1,
						Prices = x.TransportPrice.Title.Prices
							.Select(y => new PromotionAssignResult.RechargePrice
							{
								Id = y.Id,
								Zone = x.TransportPrice.Title.HasZone ? y.Zone : (x.TransportPrice.Title.Code == 96 ? EigeZonaEnum.A : (EigeZonaEnum?)null),
								ZoneName = x.TransportPrice.Title.HasZone ? y.Zone.ToEnumAlias("") : "",
								Price = y.Price,
								//ChangePrice = y.Config.ChangePrice,
								//RechargeType = y.Config.RechargeType,
								//Slot = y.Config.Slot
							}),
						TransportConcession = x.TransportPrice.Title.TransportConcessionId,
						TuiNMax = x.TransportPrice.Title.MaxQuantity,
						TuiNMin = x.TransportPrice.Title.MinCharge,
						TuiNStep = x.TransportPrice.Title.PriceStep,
						Quantity = x.TransportPrice.Title.Quantity
					})
					.Where(y => y.Prices.Any())
			};
		}
		#endregion ExecuteAsync

		#region GetPromoUser
		private async Task<PromoUser> GetPromoUserAsync(DateTime now)
		{
			var promoUser = (await PromoUserRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.FirstOrDefault();

			if (promoUser == null)
			{
				promoUser = new PromoUser
				{
					Login = SessionData.Login,
					Attemps = 1,
					LastChance = now
				};
				await PromoUserRepository.AddAsync(promoUser);
			}

			return promoUser;
		}
		#endregion GetPromoUser
	}
}
