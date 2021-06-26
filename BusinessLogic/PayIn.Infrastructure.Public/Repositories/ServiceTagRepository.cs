using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceTagRepository : PublicRepository<ServiceTag>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ServiceTagRepository(
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
		public override IQueryable<ServiceTag> CheckHorizontalVisibility(IQueryable<ServiceTag> that)
		{
			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
				that = that
					.Where(x =>
						x.Supplier.Login == SessionData.Login ||
						x.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					);

			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
