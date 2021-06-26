using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PromotionGetPaymentConcessionSelectorHandler :
		IQueryBaseHandler<PromotionGetPaymentConcessionSelectorArguments, PromotionGetPaymentConcessionSelectorResult>
	{
		private readonly IEntityRepository<Domain.Payments.PaymentConcession> Repository;

		#region Constructors
		public PromotionGetPaymentConcessionSelectorHandler(IEntityRepository<Domain.Payments.PaymentConcession> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<PromotionGetPaymentConcessionSelectorResult>> ExecuteAsync(PromotionGetPaymentConcessionSelectorArguments arguments)
		{
			var items = await Repository.GetAsync("Concession");

			var result = items
				.Where(x => x.Concession.Name.Contains(arguments.Filter))
				.ToList()
				.Select(x => new PromotionGetPaymentConcessionSelectorResult
				{
					Id = x.Id,
					Value = x.Concession.Name
				});

			return new ResultBase<PromotionGetPaymentConcessionSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
