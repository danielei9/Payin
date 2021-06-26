using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class PaymentConcessionMobileGetAllWithPromotionsHandler :
		IQueryBaseHandler<PaymentConcessionMobileGetAllWithPromotionsArguments, PaymentConcessionMobileGetAllWithPromotionsResult>
	{
		private readonly IEntityRepository<PaymentConcession> Repository;
		private readonly IEntityRepository<Campaign> CampaignRepository;

		#region Constructors
		public PaymentConcessionMobileGetAllWithPromotionsHandler(
			IEntityRepository<PaymentConcession> repository,
			IEntityRepository<Campaign> campaignRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (campaignRepository == null) throw new ArgumentNullException("campaignRepository");

			Repository = repository;
			CampaignRepository = campaignRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PaymentConcessionMobileGetAllWithPromotionsResult>> ExecuteAsync(PaymentConcessionMobileGetAllWithPromotionsArguments arguments)
		{
			var random = new Random();

			var concessions = (await Repository.GetAsync())
				.Where(x =>
					x.Concession.State == ConcessionState.Active &&
					x.Address.Contains("vilamarxant")
				)
				.Select(x => new PaymentConcessionMobileGetAllWithPromotionsResult
				{
					Id = x.Id,
					Name = x.Concession.Name,
					Address = x.Address,
					PhotoUrl = ""
				})
				.ToList()
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					Address = x.Address,
					PhotoUrl = "",
					Random = random.Next(int.MaxValue)
				})
				.OrderBy(x => x.Random)
				.Select(x => new PaymentConcessionMobileGetAllWithPromotionsResult
				{
					Id = x.Id,
					Name = x.Name,
					Address = x.Address,
					PhotoUrl = x.PhotoUrl,
					Random = x.Random
				});

			var promotions = (await CampaignRepository.GetAsync())
				.Where(x =>
					x.State == CampaignState.Active &&
					x.Concession.Concession.State == ConcessionState.Active &&
					x.Concession.Address.Contains("vilamarxant")
				)
				.Select(x => new {
					Id = x.Id,
					Title = x.Title,
					ConcessionName = x.Concession.Concession.Name,
					Since = x.Since,
					Until = x.Until,
					Price = (decimal?)null,
					PhotoUrl = x.PhotoUrl
				})
				.ToList()
				.Select(x => new {
					Id = x.Id,
					Title = x.Title,
					ConcessionName = x.ConcessionName,
					Since = x.Since,
					Until = x.Until,
					Price = (decimal?)null,
					PhotoUrl = x.PhotoUrl,
					Random = random.Next(int.MaxValue)
				})
				.OrderBy(x => x.Random)
				.Select(x => new PaymentConcessionMobileGetAllWithPromotionsResultBase.Promotion
				{
					Id = x.Id,
					Title = x.Title,
					ConcessionName = x.ConcessionName,
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC(),
					Price = x.Price,
					PhotoUrl = x.PhotoUrl,
					Random = x.Random
				});

			return new PaymentConcessionMobileGetAllWithPromotionsResultBase
			{
				Data = concessions,
				Promotions = promotions
			};
		}
		#endregion ExecuteAsync
	}
}
