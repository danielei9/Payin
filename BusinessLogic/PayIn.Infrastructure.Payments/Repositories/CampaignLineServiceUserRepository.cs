using System.Linq;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class CampaignLineServiceUserRepository : PublicRepository<CampaignLineServiceUser>
	{
		#region Contructors
		public CampaignLineServiceUserRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<CampaignLineServiceUser> CheckHorizontalVisibility(IQueryable<CampaignLineServiceUser> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
