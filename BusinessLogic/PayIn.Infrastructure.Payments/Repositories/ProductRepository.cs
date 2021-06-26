using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class ProductRepository : PublicRepository<Product>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ProductRepository(
			ISessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<Product> CheckHorizontalVisibility(IQueryable<Product> that)
		{
			return that
				.Where(x => x.State != ProductState.Deleted);
		}
		#endregion CheckHorizontalVisibility
	}
}
