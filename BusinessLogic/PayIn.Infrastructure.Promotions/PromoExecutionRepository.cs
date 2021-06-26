using PayIn.Domain.Promotions;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Promotions.Repositories
{
	public class PromoExecutionRepository : PublicRepository<PromoExecution>
	{
		#region Contructors
		public PromoExecutionRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<PromoExecution> CheckHorizontalVisibility(IQueryable<PromoExecution> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
