using PayIn.Application.Dto.Payments.Arguments.CampaignLine;
using PayIn.Application.Dto.Payments.Results.CampaignLine;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineGetHandler :
		IQueryBaseHandler<CampaignLineGetArguments, CampaignLineGetResult>
	{
		private readonly IEntityRepository<CampaignLine> Repository;

		#region Constructors
		public CampaignLineGetHandler(IEntityRepository<CampaignLine> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<CampaignLineGetResult>> ExecuteAsync(CampaignLineGetArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.Select(x => new 
				{
					Id = x.Id,
					//Min = x.Min,
					//Max = x.Max,
					Quantity = x.Quantity,
					Type = x.Type,
					//PurseId = x.PurseId,
					SinceTime = x.SinceTime,
					UntilTime = x.UntilTime,
					CampaignId = x.CampaignId,
					AllProduct = x.AllProduct,
					AllEntranceType = x.AllEntranceType,
					All = x.SinceTime==null && x.UntilTime==null
				})
				.ToList()
				.Select(x => new CampaignLineGetResult
				{
					Id = x.Id,
					//Min = x.Min,
					//Max = x.Max,
					Quantity = x.Quantity,
					Type = x.Type,
					//PurseId = x.PurseId,
					SinceTime = x.SinceTime.ToUTC(),
					UntilTime = x.UntilTime.ToUTC(),
					CampaignId = x.CampaignId,
					AllProduct = x.AllProduct,
					AllEntranceType = x.AllEntranceType,
					All = x.All
				});
			
			return new ResultBase<CampaignLineGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
