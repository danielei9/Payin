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
	public class CampaignGetAddOwnersHandler :
		IQueryBaseHandler<ApiCampaignGetAddOwnersArguments, ApiCampaignGetAddOwnersResult>
	{
		private readonly IEntityRepository<PaymentConcessionCampaign> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public CampaignGetAddOwnersHandler(IEntityRepository<PaymentConcessionCampaign> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ApiCampaignGetAddOwnersResult>> IQueryBaseHandler<ApiCampaignGetAddOwnersArguments, ApiCampaignGetAddOwnersResult>.ExecuteAsync(ApiCampaignGetAddOwnersArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => x.PaymentConcession.Concession.Supplier.Login == SessionData.Login && x.State == Common.PaymentConcessionCampaignState.Active)
				.Select(x => new ApiCampaignGetAddOwnersResultBase.PaymentConcession
				{
					//Id = x.Campaig.Id,
					//Login = x.PaymentConcessionFollow.Concession.Supplier.Login,
					//Name = x.PaymentConcessionFollow.Concession.Supplier.Name
				})
				.GroupBy(x => x.Name)				
				.Select(group => group.First())
				.ToList();

			return new ApiCampaignGetAddOwnersResultBase { PaymentConcessions = result };
		}
		#endregion ExecuteAsync
	}
}
