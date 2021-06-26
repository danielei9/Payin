using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;
using Xp.Domain;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceSupplierRepository : PublicRepository<ServiceSupplier>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ServiceSupplierRepository(
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
		public override IQueryable<ServiceSupplier> CheckHorizontalVisibility(IQueryable<ServiceSupplier> that)
		{
			if (!SessionData.Roles.Contains(AccountRoles.Superadministrator))
				that = that
					.Where(x =>						
						x.Login == SessionData.Login ||
						x.Workers.Select(y => y.Login).Contains(SessionData.Login) 
					);
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
