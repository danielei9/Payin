using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class CampaignEventGetSelectorHandler :
		IQueryBaseHandler<CampaignEventGetSelectorArguments, CampaignEventGetSelectorResult>
	{
		private readonly IEntityRepository<Event> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public CampaignEventGetSelectorHandler(IEntityRepository<Event> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<CampaignEventGetSelectorResult>> ExecuteAsync(CampaignEventGetSelectorArguments arguments)
		{
            var items = (await Repository.GetAsync())
                    .Where(x =>
                        x.State != Common.EventState.Deleted
                    );


            if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x =>
						x.Name.Contains(arguments.Filter)
					);

			var result = items
				.Where(x => 
					x.PaymentConcession.Concession.Supplier.Login == SessionData.Login ||
					x.PaymentConcession.Concession.Supplier.Workers
						.Any(y => y.Login == SessionData.Login)
					)
				.Select(x => new CampaignEventGetSelectorResult
                {
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<CampaignEventGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
