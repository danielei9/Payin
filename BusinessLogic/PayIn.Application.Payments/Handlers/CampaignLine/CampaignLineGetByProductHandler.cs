using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class CampaignLineGetByProductHandler :
		IQueryBaseHandler<CampaignLineGetByProductArguments, CampaignLineGetByProductResult>
	{
		private readonly IEntityRepository<Product> Repository;

		#region Constructors
		public CampaignLineGetByProductHandler(
			IEntityRepository<Product> repository
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<CampaignLineGetByProductResult>> ExecuteAsync(CampaignLineGetByProductArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x =>
					x.CampaignLines
						.Any(y => y.Id == arguments.Id)
				)
				.Select(x => new CampaignLineGetByProductResult
				{
					Id = x.Id,
					Name = x.Name
				});

			return new ResultBase<CampaignLineGetByProductResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
