using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceCheckPointRepository : PublicRepository<ServiceCheckPoint>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ServiceCheckPointRepository(
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
		public override IQueryable<ServiceCheckPoint> CheckHorizontalVisibility(IQueryable<ServiceCheckPoint> that)
		{
			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
				that = that
					.Where(x =>
						x.Supplier.Login == SessionData.Login ||
						(
							x.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login) &&
							x.Item.Concession.State == Common.ConcessionState.Active
						)
					);

			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
