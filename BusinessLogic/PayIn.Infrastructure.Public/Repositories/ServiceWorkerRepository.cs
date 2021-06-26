using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;
using PayIn.Common.Resources;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceWorkerRepository : PublicRepository<ServiceWorker>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ServiceWorkerRepository(
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
		public override IQueryable<ServiceWorker> CheckHorizontalVisibility(IQueryable<ServiceWorker> that)
		{
			if (SessionData.Roles.Contains(AccountRoles.Operator))
				that = that
					.Where(x =>
						x.Supplier.Login == SessionData.Login ||
						x.Supplier.Workers
							.Select(y => y.Login)
							.Contains(SessionData.Login)
					);
			else
				that = that
					.Where(x =>
						x.Supplier.Login == SessionData.Login ||
						x.Login == SessionData.Login
					);		
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
