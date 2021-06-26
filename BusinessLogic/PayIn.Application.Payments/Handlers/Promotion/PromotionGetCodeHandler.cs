using PayIn.Application.Dto.Payments.Results.Promotion;
using PayIn.Application.Dto.Payments.Arguments.Promotion;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using PayIn.Domain.Payments;
using Xp.Domain;
using PayIn.Common;
using PayIn.Domain.Transport;
using PayIn.Domain.Promotions;

namespace PayIn.Application.Payments.Handlers
{
	public class PromotionGetCodeHandler :
		IQueryBaseHandler<PromotionGetCodeArguments, PromotionGetCodeResult>
	{
		private readonly IEntityRepository<PromoExecution> Repository;
		private readonly IEntityRepository<TransportOperation> OperationRepository;
		private readonly IEntityRepository<TransportPrice> PriceRepository;

		#region Constructor
		public PromotionGetCodeHandler(
			IEntityRepository<PromoExecution> repository,
			IEntityRepository<TransportOperation> operationRepository,
			IEntityRepository<TransportPrice> priceRepository
		)
		{
			if (repository == null) throw new ArgumentNullException("PromoExecution");
			if (operationRepository == null) throw new ArgumentNullException("operationRepository");
			if (priceRepository == null) throw new ArgumentNullException("priceRepository");

			Repository = repository;
			OperationRepository = operationRepository;
			PriceRepository = priceRepository;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<ResultBase<PromotionGetCodeResult>> ExecuteAsync(PromotionGetCodeArguments arguments)
		{
			var items = (await Repository.GetAsync("PromoUser"))
				.Where(x => x.PromotionId == arguments.Id);

			var operations = (await OperationRepository.GetAsync());
			var title = (await PriceRepository.GetAsync("Title"));

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Code,
					Login = x.PromoUser.Login,
					AppliedDate = x.AppliedDate,
					Title = title.Where(z => z.Id == operations.Where(y => y.Id == x.TransportOperationId).FirstOrDefault().TransportPriceId).FirstOrDefault().Title.Name,
					Zone = title.Where(z => z.Id == operations.Where(y => y.Id == x.TransportOperationId).FirstOrDefault().TransportPriceId).FirstOrDefault().Zone,
					Price = (decimal?)title.Where(z => z.Id == operations.Where(y => y.Id == x.TransportOperationId).FirstOrDefault().TransportPriceId).FirstOrDefault().Price,
                    Travels = x.Promotion.PromoActions.Select( y => y.Quantity).Sum(),
                    TitleQuantity = title.Where(z => z.Id == operations.Where(y => y.Id == x.TransportOperationId).FirstOrDefault().TransportPriceId).FirstOrDefault().Title.Quantity
                })
				.ToList()
				.OrderByDescending(x => x.AppliedDate)
				.Select(x => new PromotionGetCodeResult
				{
					Id = x.Id,
					Name = x.Name,
					Login = x.Login,
					AppliedDate = x.AppliedDate.ToUTC(),
					Title = x.Title,
					Zone = x.Zone,
					ZoneName = x.Zone.ToEnumAlias(),
					Price = x.Price	,
                    Travels = x.Travels,
                    TravelsPrice = ((x.Price * x.Travels)/x.TitleQuantity)

                });

			return new PromotionGetCodeResultBase {
				Data = result,
				TotalApplied = items.Count(y => y.AppliedDate != null),
				Total = items.Count()


			};
		}
		#endregion ExecuteAsync
	}
}
