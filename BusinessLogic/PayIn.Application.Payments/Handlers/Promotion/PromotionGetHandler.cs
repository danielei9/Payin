using PayIn.Application.Dto.Payments.Results.Promotion;
using PayIn.Application.Dto.Payments.Arguments.Promotion;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using PayIn.Domain.Promotions;
using Xp.Domain;
using PayIn.BusinessLogic.Common;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionGetHandler :
		IQueryBaseHandler<PromotionGetArguments, PromotionGetResult>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Promotion> Repository;

		#region Constructor
		public PromotionGetHandler(
			ISessionData sessionData,
			IEntityRepository<Promotion> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("Promotion");
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<PromotionGetResult>> ExecuteAsync(PromotionGetArguments arguments)
		{
			var items = (await Repository.GetAsync("PromoConditions","Concession.Concession.Supplier", "Promotions.PromoPrices.TransportPrices.TransportTitles"))
				.Where(x => x.Id == arguments.Id);

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					StartDate = x.StartDate,
					EndDate = x.EndDate,
					Acumulative = x.Acumulative,
					State = x.State,
					Title = x.PromoPrices.FirstOrDefault().TransportPrice.Title.Name,
					Zone = x.PromoPrices.FirstOrDefault().TransportPrice.Zone,
					Price = x.PromoPrices.FirstOrDefault().TransportPrice.Price,
					ConcessionLogin = x.Concession.Concession.Supplier.Login,
					PromoConditions = x.PromoConditions,
					PromoActions = x.PromoActions.FirstOrDefault().Quantity,
					PromoLaunchers = x.PromoLaunchers,
					Concession = x.Concession.Concession.Name,
					PromoExecutions = x.PromoExecutions.Count(),
					TitleList = x.PromoPrices.Select( y => new
						{
							Zone =  y.TransportPrice.Zone,
							Price = y.TransportPrice.Price,
							Title = y.TransportPrice.Title.Name
						})
						
				})
				.ToList()
				.OrderByDescending(x => x.StartDate)
				.Select(x => new PromotionGetResult
				{
					Id = x.Id,
					Name = x.Name,
					StartDate = x.StartDate,
					EndDate = x.EndDate,
					Acumulative = x.Acumulative,
					State = x.State,
					Title = x.Title,
					Zone = x.Zone,
					Price = x.Price,
					PromoConditions = x.PromoConditions,
					PromoActions = x.PromoActions,
					PromoLaunchers = x.PromoLaunchers,
					ConcessionName = x.Concession,
					isOwner = SessionData.Login == x.ConcessionLogin ? true : false,
					TitleQuantity = x.PromoExecutions,
					TitlesList = x.TitleList.Select(y => new PromotionGetResult.TitleList
													{
														Zone = y.Zone == null ? "" : y.Zone.ToEnumAlias(),
														Price = y.Price,
														Title = y.Title
													})
				});

			return new ResultBase<PromotionGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
