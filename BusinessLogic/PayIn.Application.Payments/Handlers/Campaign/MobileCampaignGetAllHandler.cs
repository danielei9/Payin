using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class MobileCampaignGetAllHandler :
		IQueryBaseHandler<MobileCampaignGetAllArguments, MobileCampaignGetAllResult>
	{
		private readonly IEntityRepository<Campaign> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public MobileCampaignGetAllHandler(IEntityRepository<Campaign> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<MobileCampaignGetAllResult>> ExecuteAsync(MobileCampaignGetAllArguments arguments)
		{
			var now = new XpDate(DateTime.Now);

			var items = (await Repository.GetAsync())
				.Where(x =>
					((x.Concession.Concession.Cards
						.Select(y => y.Uid)
						.Contains(arguments.Uid))
					) && 
					x.State == CampaignState.Active);

			var result = items
			.Select(x => new 
			{
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Since = x.Since,
				Until = x.Until,
				State = (x.Since <= now && x.Until >= now) ? true : false,
				PhotoUrl = x.PhotoUrl
			})
			.OrderByDescending(x => x.Since)
			.ToList()
			.Select(x => new MobileCampaignGetAllResult
			{
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Since = x.Since,
				Until = x.Until != XpDate.MaxValue ? x.Until : (DateTime?)null,
				State = x.State,
				PhotoUrl = x.PhotoUrl
			});

			return new ResultBase<MobileCampaignGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
