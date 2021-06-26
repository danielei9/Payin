using PayIn.BusinessLogic.Common;
using PayIn.Domain.Promotions;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Promotions.Repositories
{
	public class PromotionRepository : PublicRepository<Promotion>
	{

		public readonly ISessionData SessionData;		

		#region Contructors
		public PromotionRepository(
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
		public override IQueryable<Promotion> CheckHorizontalVisibility(IQueryable<Promotion> that)
		{
			if (SessionData.Roles.Contains(AccountRoles.Superadministrator))
				return that;
			else
			{
				that = that
					.Where(x =>
						(x.Concession.Concession.Supplier.Login == SessionData.Login) ||
						(x.PromoPrices
							.Where(y => y.TransportPrice.Title.TransportConcession.Concession.Concession.Supplier.Login == SessionData.Login))
							.Any()
					);
				return that;
			}
		}
		#endregion CheckHorizontalVisibility
	}
}
