using PayIn.Application.Dto.Payments.Arguments.CampaignLine;
using PayIn.Application.Dto.Payments.Results.CampaignLine;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiCampaignLineGetAllHandler :
		IQueryBaseHandler<ApiCampaignLineGetAllArguments, ApiCampaignLineGetAllResult>
	{
		private readonly IEntityRepository<Campaign> Repository;

		#region Constructors
		public ApiCampaignLineGetAllHandler(IEntityRepository<Campaign> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ApiCampaignLineGetAllResult>> IQueryBaseHandler<ApiCampaignLineGetAllArguments, ApiCampaignLineGetAllResult>.ExecuteAsync(ApiCampaignLineGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
			.Where(x =>
				x.Id == arguments.Id &&
				x.State != CampaignState.Deleted
			)
			.Select(x => new
			{
				x.Title,
				Data = x.CampaignLines
					.Where(y => y.State == CampaignLineState.Active)
					.Select(y => new
					{
						y.Id,
						y.Quantity,
						y.SinceTime,
						y.UntilTime,
						y.Type,
						y.CampaignId,
						y.AllProduct,
						y.AllEntranceType,
						ProductCount = y.Products
							.Where(z => z.State == ProductState.Active)
							.Count(),
						ProductFamilyCount = y.ProductFamilies
							.Where(z => z.State == ProductFamilyState.Active)
							.Count(),
						ServiceUserCount = y.ServiceUsers
							.Count(),
						ServiceGroupCount = y.ServiceGroups
							.Count(),
						UserCount = 0,
						UsersGroupCount = 0,
						EntranceTypeCount = y.EntranceTypes
							.Where(z => z.State != EntranceTypeState.Deleted)
							.Count(),
					})
			})
			.ToList()
			.Select(x => new {
				x.Title,
				Data = x.Data
					.Select(y => new ApiCampaignLineGetAllResult
					{
						Id = y.Id,
						Quantity = y.Quantity,
						SinceTime = y.SinceTime.ToUTC(),
						UntilTime = y.UntilTime.ToUTC(),
						Type = y.Type,
						TypeAlias = y.Type.ToEnumAlias(),
						CampaignId = y.CampaignId,
						AllProduct = y.AllProduct,
						AllEntranceType = y.AllEntranceType,
						ServiceUserCount = y.ServiceUserCount,
						ServiceGroupCount = y.ServiceGroupCount,
						ProductCount = y.ProductCount,
						ProductFamilyCount = y.ProductFamilyCount,
						EntranceTypeCount = y.EntranceTypeCount
					})
			})
			.FirstOrDefault();

			return new ApiCampaignLineGetAllResultBase
			{
				Title = items.Title,
				Data = items.Data
			};
		}
		#endregion ExecuteAsync
	}
}
