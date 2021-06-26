using PayIn.Application.Dto.Payments.Arguments.CampaignLine;
using PayIn.Application.Dto.Payments.Results.CampaignLine;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class CampaignLineGetSelectorHandler :
		IQueryBaseHandler<CampaignLineGetSelectorArguments, CampaignLineGetSelectorResult>
	{
		private readonly IEntityRepository<Purse> _Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public CampaignLineGetSelectorHandler(IEntityRepository<Purse> repository,ISessionData sessionData)
		{
			if (repository == null)	throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			_Repository = repository;
			SessionData = sessionData;

		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<CampaignLineGetSelectorResult>> IQueryBaseHandler<CampaignLineGetSelectorArguments, CampaignLineGetSelectorResult>.ExecuteAsync(CampaignLineGetSelectorArguments arguments)
		{
			var items = await _Repository.GetAsync();
			var now = new XpDate(DateTime.Now);

			if (!arguments.Filter.IsNullOrEmpty())
				items = items
					.Where(x => x.Name.Contains(arguments.Filter));

			var result = items
				.Where(x =>
					(x.State == Common.PurseState.Active) &&
					(
						(x.Concession.Concession.Supplier.Login == SessionData.Login) ||
						(x.Concession.PaymentWorkers.Any(y => y.Login == SessionData.Login))
					)
				)
				.Select(x => new CampaignLineGetSelectorResult
				{
					Id = x.Id,
					Value = x.Name
				});

			return new ResultBase<CampaignLineGetSelectorResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
