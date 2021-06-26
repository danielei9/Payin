using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	class ServiceUserRepository : PublicRepository<ServiceUser>
	{
		public ISessionData SessionData { get; set; }

		#region Constructors
		public ServiceUserRepository (
			IPublicContext context,
			ISessionData sessionData
		)
			: base(context)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			SessionData = sessionData;
		}
		#endregion Constructors

		#region CheckHorizontalVisibility
		public override IQueryable<ServiceUser> CheckHorizontalVisibility(IQueryable<ServiceUser> that)
		{
			return that
				.Where(x =>
					(
						SessionData.Roles.Contains(AccountRoles.Superadministrator) ||
						x.Concession.Supplier.Login == SessionData.Login ||
                        x.Concession.Supplier.Workers
                            .Any(y => y.Login == SessionData.Login)
                    ) &&
                    (x.State != Common.ServiceUserState.Deleted)
				);
		}
		#endregion CheckHorizontalVisibility
	}
}
