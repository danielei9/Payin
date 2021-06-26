using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class CampaignCodeRepository : PublicRepository<CampaignCode>
	{
		#region Contructors
		public CampaignCodeRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<CampaignCode> CheckHorizontalVisibility(IQueryable<CampaignCode> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
