using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class CampaignLineServiceUserGetSelectorHandler :
		IQueryBaseHandler<CampaignLineServiceUserGetSelectorArguments, CampaignLineServiceUserGetSelectorResult>
	{
		private readonly IEntityRepository<ServiceUser> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public CampaignLineServiceUserGetSelectorHandler(IEntityRepository<ServiceUser> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<CampaignLineServiceUserGetSelectorResult>> ExecuteAsync(CampaignLineServiceUserGetSelectorArguments arguments)
		{
			var items = (await Repository.GetAsync())
					.Where(x => 
						x.State != Common.ServiceUserState.Deleted
					);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						(x.Name + " " + x.LastName).Contains(arguments.Filter)
					);

			var result = items
				.Where(x => 
					x.Concession.Supplier.Login == SessionData.Login ||
					x.Concession.Supplier.Workers
						.Any(y => y.Login == SessionData.Login)
					)
				.Select(x => new CampaignLineServiceUserGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name + " " + x.LastName
				});

			return new ResultBase<CampaignLineServiceUserGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
