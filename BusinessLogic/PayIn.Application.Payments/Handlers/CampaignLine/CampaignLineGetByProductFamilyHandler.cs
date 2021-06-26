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
	class CampaignLineGetByProductFamilyHandler :
		IQueryBaseHandler<CampaignLineGetByProductFamilyArguments, CampaignLineGetByProductFamilyResult>
	{
		private readonly IEntityRepository<ProductFamily> Repository;

		#region Constructors
		public CampaignLineGetByProductFamilyHandler(
			IEntityRepository<ProductFamily> repository
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<CampaignLineGetByProductFamilyResult>> ExecuteAsync(CampaignLineGetByProductFamilyArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x =>
					x.CampaignLines
						.Any(y => y.Id == arguments.Id)
				)
				.Select(x => new CampaignLineGetByProductFamilyResult
				{
					Id = x.Id,
					Name = x.Name
				});

			return new ResultBase<CampaignLineGetByProductFamilyResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
