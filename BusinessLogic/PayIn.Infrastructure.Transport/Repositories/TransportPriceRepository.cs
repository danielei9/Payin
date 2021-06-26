using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportPriceRepository : PublicRepository<TransportPrice>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public TransportPriceRepository(
			SessionData sessionData,
			IPublicContext context
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<TransportPrice> CheckHorizontalVisibility(IQueryable<TransportPrice> that)
		{
			//if (SessionData.Roles.Contains(AccountRoles.Superadministrator))
				return that;
			//else
			//	that = that;
			//		/*.Where(x =>
			//			x.Title.TransportConcession.Concession.Concession.Supplier.Login.Contains(SessionData.Login)
			//		);*/

			//return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
