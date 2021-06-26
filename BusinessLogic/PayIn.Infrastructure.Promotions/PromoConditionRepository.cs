using PayIn.Domain.Promotions;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Promotions.Repositories
{
	public class PromoConditionRepository : PublicRepository<PromoCondition>
	{
		#region Contructors
		public PromoConditionRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<PromoCondition> CheckHorizontalVisibility(IQueryable<PromoCondition> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
