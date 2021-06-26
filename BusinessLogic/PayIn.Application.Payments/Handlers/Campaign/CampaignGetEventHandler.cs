using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignGetEventHandler :
		IQueryBaseHandler<ApiCampaignGetEventArguments, ApiCampaignGetEventResult>
	{
		private readonly IEntityRepository<Campaign> Repository;

		#region Constructors
		public CampaignGetEventHandler(IEntityRepository<Campaign> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ApiCampaignGetEventResult>> ExecuteAsync(ApiCampaignGetEventArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x =>
					x.Id == arguments.CampaignId &&
					x.State != CampaignState.Deleted
				)
				.Select(x => new
				{
					x.Id,
					x.Title,
					Events = x.TargetEvents
						.Select(y => new
						{
							Id = y.Id,
							SinceTime = y.EventStart,
							UntilTime = y.EventEnd,
							y.Name
						})
				})
				.ToList()
				.Select(x => new
				{
					x.Id,
					x.Title,
					Events = x.Events
						.Select(y => new ApiCampaignGetEventResult {
							Id = y.Id,
							SinceTime = (y.SinceTime == XpDateTime.MinValue) ? (DateTime?)null : y.SinceTime.ToUTC(),
							UntilTime = (y.UntilTime == XpDateTime.MinValue) ? (DateTime?)null : y.UntilTime.ToUTC(),
							Name = y.Name
						})
				})
				.FirstOrDefault();
			return new ApiCampaignGetEventResultBase {
				Id = items.Id,
				Title = items.Title,
				Data = items.Events
			};
		}
		#endregion ExecuteAsync
	}
}

