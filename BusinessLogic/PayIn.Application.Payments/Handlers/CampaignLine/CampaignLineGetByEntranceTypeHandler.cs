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
	class CampaignLineGetByEntranceTypeHandler :
		IQueryBaseHandler<ApiCampaignLineGetByEntranceTypeArguments, ApiCampaignLineGetByEntranceTypeResult>
	{
		private readonly IEntityRepository<CampaignLine> Repository;

		#region Constructors
		public CampaignLineGetByEntranceTypeHandler(
			IEntityRepository<CampaignLine> repository
			)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ApiCampaignLineGetByEntranceTypeResult>> ExecuteAsync(ApiCampaignLineGetByEntranceTypeArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x =>
					x.Id == arguments.CampaignLineId &&
					x.State == CampaignLineState.Active &&
					x.Campaign.State != CampaignState.Deleted
				)
				.Select(x => new
				{
					CampaignId = x.Campaign.Id,
					CampaignTitle = x.Campaign.Title,
					CampaignLineId = x.Id,
					EntranceTypes = x.EntranceTypes
						.Select(y => new ApiCampaignLineGetByEntranceTypeResult
						{
							Id = y.Id,
							Name = y.Name,
							EventName = y.Event.Name
						})
				})
				.FirstOrDefault();

			return new ApiCampaignLineGetByEntranceTypeResultBase
			{
				CampaignId = result.CampaignId,
				CampaignTitle = result.CampaignTitle,
				CampaignLineId = result.CampaignLineId,
				Data = result.EntranceTypes
			};
		}
		#endregion ExecuteAsync
	}
}
