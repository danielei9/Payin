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
	public class CampaignLineServiceGroupGetSelectorHandler :
		IQueryBaseHandler<CampaignLineServiceGroupGetSelectorArguments, CampaignLineServiceGroupGetSelectorResult>
	{
		private readonly IEntityRepository<ServiceGroup> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public CampaignLineServiceGroupGetSelectorHandler(IEntityRepository<ServiceGroup> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<CampaignLineServiceGroupGetSelectorResult>> ExecuteAsync(CampaignLineServiceGroupGetSelectorArguments arguments)
		{
			var items = (await Repository.GetAsync());
					//.Where(x => 
					//	x.State != Common.ServiceGroupState.Deleted
					//);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			var result = items
				//.Where(x => 
				//	x.Concession.Supplier.Login == SessionData.Login ||
				//	x.Concession.Supplier.Workers
				//		.Any(y => y.Login == SessionData.Login)
				//	)
				.Select(x => new CampaignLineServiceGroupGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<CampaignLineServiceGroupGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
