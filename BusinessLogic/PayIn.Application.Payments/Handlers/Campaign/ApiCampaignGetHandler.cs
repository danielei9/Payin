using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;
using serV = PayIn.Domain.Payments.Campaign;

namespace PayIn.Application.Public.Handlers
{
	public class CampaignGetHandler :
		IQueryBaseHandler<ApiCampaignGetArguments, ApiCampaignGetResult>
	{
		private readonly IEntityRepository<serV> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public CampaignGetHandler(IEntityRepository<serV> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ApiCampaignGetResult>> ExecuteAsync(ApiCampaignGetArguments arguments)
		{
			var now = new XpDate(DateTime.Now);

			var result = (await Repository.GetAsync())
			.Where(x => x.Id == arguments.Id)
			.Select(x => new
			{
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Since = x.Since,
				Until = x.Until,
				NumberOfTimes = x.NumberOfTimes,
				Started = (x.Since <= now) ? true : false,
				Finished = (x.Until <= now) ? true : false,
				PhotoUrl = x.PhotoUrl,
				x.TargetConcessionId,
				TargetConcessionName = x.TargetConcession.Name,
				x.TargetSystemCardId,
				TargetSystemCardName = x.TargetSystemCard.Name
			})
			.OrderByDescending(x => x.Since)
			.ToList()
			.Select(x => new ApiCampaignGetResult
			{
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Since = x.Since,
				Until = x.Until != XpDate.MaxValue ? x.Until : (DateTime?)null,
				NumberOfTimes = x.NumberOfTimes != int.MaxValue ? x.NumberOfTimes : (int?)null,
				Started = x.Started,
				Finished = x.Finished,
				PhotoUrl = x.PhotoUrl,
				TargetConcessionId = x.TargetConcessionId,
				TargetConcessionName = x.TargetConcessionName,
				TargetSystemCardId = x.TargetSystemCardId,
				TargetSystemCardName = x.TargetSystemCardName
			});
			
			return new ResultBase<ApiCampaignGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
