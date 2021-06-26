using PayIn.Domain.Promotions;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Promotions.Repositories
{
	public class PromoUserRepository : PublicRepository<PromoUser>
	{
		#region Contructors
		public PromoUserRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<PromoUser> CheckHorizontalVisibility(IQueryable<PromoUser> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
