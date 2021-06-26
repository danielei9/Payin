using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class CampaignLineRepository : PublicRepository<CampaignLine>
	{
		#region Contructors
		public CampaignLineRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<CampaignLine> CheckHorizontalVisibility(IQueryable<CampaignLine> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
