using PayIn.Application.Dto.Payments.Arguments.CampaignLine;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class CampaignLineDeleteHandlercs :
		IServiceBaseHandler<CampaignLineDeleteArguments>
	{
		private readonly IEntityRepository<CampaignLine> Repository;

		#region Constructors
		public CampaignLineDeleteHandlercs(IEntityRepository<CampaignLine> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<CampaignLineDeleteArguments>.ExecuteAsync(CampaignLineDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id,"Campaign");

			var now = DateTime.Now;

			if (item.Campaign.Since <= now)
				throw new ApplicationException(CampaignLineResources.DeleteException);

			item.State = Common.CampaignLineState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}
