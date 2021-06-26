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
	public class PaymentConcessionCampaignGetAllHandler :
		IQueryBaseHandler<PaymentConcessionCampaignGetAllArguments, PaymentConcessionCampaignGetAllResult>
	{
		private readonly IEntityRepository<PaymentConcessionCampaign> Repository;
		private readonly ISessionData SessionData;


		#region Constructors
		public PaymentConcessionCampaignGetAllHandler(IEntityRepository<PaymentConcessionCampaign> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<PaymentConcessionCampaignGetAllResult>> IQueryBaseHandler<PaymentConcessionCampaignGetAllArguments, PaymentConcessionCampaignGetAllResult>.ExecuteAsync(PaymentConcessionCampaignGetAllArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.CampaignId == arguments.Id);

			var result = items
			.Where(x => x.PaymentConcession.Concession.Supplier.Login != SessionData.Login)
			.Select(x => new PaymentConcessionCampaignGetAllResult
			{
				Id = x.Id,
			    State = x.State,
				PaymentConcessionName = x.PaymentConcession.Concession.Supplier.Name,
				IsOwner = x.PaymentConcession.Concession.Supplier.Login	== SessionData.Login	
			});

			return new ResultBase<PaymentConcessionCampaignGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
