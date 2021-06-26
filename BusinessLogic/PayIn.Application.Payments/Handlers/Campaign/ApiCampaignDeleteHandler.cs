using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignDeleteHandler :
		IServiceBaseHandler<ApiCampaignDeleteArguments>
	{
		private readonly IEntityRepository<Campaign> Repository;

		#region Constructors
		public CampaignDeleteHandler(IEntityRepository<Campaign> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ApiCampaignDeleteArguments arguments)
		{
			var now = DateTime.Now;
			var item = (await Repository.GetAsync(arguments.Id, "CampaignLines"));

			item.State = CampaignState.Deleted;

			foreach (var paymentConcessionCampaign in item.PaymentConcessionCampaigns)
				paymentConcessionCampaign.State = PaymentConcessionCampaignState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}
