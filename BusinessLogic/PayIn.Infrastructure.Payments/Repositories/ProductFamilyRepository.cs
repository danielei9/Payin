using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Payments
{
	public class ProductFamilyRepository : PublicRepository<ProductFamily>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ProductFamilyRepository(
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
		public override IQueryable<ProductFamily> CheckHorizontalVisibility(IQueryable<ProductFamily> that)
		{
			return that
				.Where(x => x.State != ProductFamilyState.Deleted);
		}
		#endregion CheckHorizontalVisibility
	}
}
