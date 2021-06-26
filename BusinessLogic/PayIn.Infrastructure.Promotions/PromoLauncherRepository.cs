using PayIn.Domain.Promotions;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Promotions.Repositories
{
	public class PromoLauncherRepository : PublicRepository<PromoLauncher>
	{
		#region Contructors
		public PromoLauncherRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<PromoLauncher> CheckHorizontalVisibility(IQueryable<PromoLauncher> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
