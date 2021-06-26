using PayIn.Domain.Promotions;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Promotions.Repositories
{
	public class PromoPriceRepository : PublicRepository<PromoPrice>
	{
		#region Contructors
		public PromoPriceRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<PromoPrice> CheckHorizontalVisibility(IQueryable<PromoPrice> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
