using System.Linq;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class CampaignLineServiceGroupRepository : PublicRepository<CampaignLineServiceGroup>
	{
		#region Contructors
		public CampaignLineServiceGroupRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<CampaignLineServiceGroup> CheckHorizontalVisibility(IQueryable<CampaignLineServiceGroup> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
