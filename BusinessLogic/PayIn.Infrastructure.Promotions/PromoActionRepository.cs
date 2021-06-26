using PayIn.Domain.Promotions;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Promotions.Repositories
{
	public class PromoActionRepository : PublicRepository<PromoAction>
	{
		#region Contructors
		public PromoActionRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<PromoAction> CheckHorizontalVisibility(IQueryable<PromoAction> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
