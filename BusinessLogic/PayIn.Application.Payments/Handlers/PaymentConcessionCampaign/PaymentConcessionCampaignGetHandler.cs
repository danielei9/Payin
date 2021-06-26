using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PaymentConcessionCampaignGetHandler :
		IQueryBaseHandler<PaymentConcessionCampaignGetArguments, PaymentConcessionCampaignGetResult>
	{
		private readonly IEntityRepository<PaymentConcessionCampaign> Repository;
		private readonly ISessionData SessionData;


		#region Constructors
		public PaymentConcessionCampaignGetHandler(IEntityRepository<PaymentConcessionCampaign> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<PaymentConcessionCampaignGetResult>> IQueryBaseHandler<PaymentConcessionCampaignGetArguments, PaymentConcessionCampaignGetResult>.ExecuteAsync(PaymentConcessionCampaignGetArguments arguments)
		{
			var items = (await Repository.GetAsync())				
			.Where(x => x.CampaignId == arguments.Id);
						

			var result = items
			.Select(x => new PaymentConcessionCampaignGetResult
			{
				Id = x.Id,
			    State = x.State,
				PaymentConcessionName = x.PaymentConcession.Concession.Supplier.Name,
				
			});

			return new ResultBase<PaymentConcessionCampaignGetResult>{ Data = result };
		}
		#endregion ExecuteAsync
	}
}
