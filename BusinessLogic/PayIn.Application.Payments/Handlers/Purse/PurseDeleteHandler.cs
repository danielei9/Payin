using PayIn.Application.Dto.Arguments.Notification;
using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class PurseDeleteHandler :
		IServiceBaseHandler<PurseDeleteArguments>
	{
		private readonly IEntityRepository<Purse> Repository;
		private readonly IEntityRepository<CampaignLine> CampaignLineRepository;

		#region Constructors
		public PurseDeleteHandler(IEntityRepository<Purse> repository, IEntityRepository<CampaignLine> campaignLineRepository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (campaignLineRepository == null) throw new ArgumentNullException("campaignLineRepository");
			
			Repository = repository;
			CampaignLineRepository = campaignLineRepository;
		}
		#endregion Constructors

		#region PurseDelete
		public async Task<dynamic> ExecuteAsync(PurseDeleteArguments arguments)
		{
			var item = (await Repository.GetAsync(arguments.Id));

			var campaignLines = (await CampaignLineRepository.GetAsync())
				.Where(x => x.PurseId == item.Id && x.State == CampaignLineState.Active);

			if (campaignLines.Count() > 0 )
				throw new Exception(PaymentConcessionPurseResources.PurseWithCampaignLineError);		

			item.State = PurseState.Deleted;

			return null;
		}
		#endregion PaymentUserDelete
	}
}
