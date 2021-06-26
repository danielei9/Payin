using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class PaymentConcessionPurseRepository : PublicRepository<PaymentConcessionPurse>
	{

		#region Contructors
		public PaymentConcessionPurseRepository(
			IPublicContext context
		)
			: base(context)
		{

		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<PaymentConcessionPurse> CheckHorizontalVisibility(IQueryable<PaymentConcessionPurse> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
