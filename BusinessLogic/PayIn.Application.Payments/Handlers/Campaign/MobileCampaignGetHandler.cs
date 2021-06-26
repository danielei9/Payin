using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Common;
using PayIn.Domain.Payments;
using Xp.Application.Attributes;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("Campaign", "MobileGet")]
	public class MobileCampaignGetHandler :
		IQueryBaseHandler<MobileCampaignGetArguments, MobileCampaignGetResult>
	{
		private readonly IEntityRepository<Campaign> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public MobileCampaignGetHandler(
			IEntityRepository<Campaign> repository, 
			ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<MobileCampaignGetResult>> ExecuteAsync(MobileCampaignGetArguments arguments)
		{
			var now = new XpDate(DateTime.Now);

			var result = (await Repository.GetAsync("CampaignLines"))
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
				CampaignLines = x.CampaignLines.Select(y => new MobileCampaignGetResult_CampaignLine
				{
					Max = y.Max,
					Min = y.Min,
					Quantity = y.Quantity,
					SinceTime = y.SinceTime,
					State = y.State,
					Type = y.Type,
					UntilTime = y.UntilTime
				})
				.ToList()
			})
			.OrderByDescending(x => x.Since)
			.ToList()
			.Select(x => new MobileCampaignGetResult
			{
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Since = x.Since != XpDate.MinValue ? x.Since : (DateTime?)null,
				Until = x.Until != XpDate.MaxValue ? x.Until : (DateTime?)null,
				NumberOfTimes = x.NumberOfTimes != int.MaxValue ? x.NumberOfTimes : (int?)null,
				Started = x.Started,
				Finished = x.Finished,
				PhotoUrl = x.PhotoUrl,
				CampaignLines = x.CampaignLines
			});
			
			return new ResultBase<MobileCampaignGetResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
