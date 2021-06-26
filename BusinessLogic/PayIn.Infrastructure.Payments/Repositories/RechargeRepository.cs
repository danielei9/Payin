using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
	public class RechargeRepository : PublicRepository<Recharge>
	{
		#region Contructors
		public RechargeRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<Recharge> CheckHorizontalVisibility(IQueryable<Recharge> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
