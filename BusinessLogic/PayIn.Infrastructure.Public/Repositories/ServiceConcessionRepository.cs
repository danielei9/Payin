using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceConcessionRepository : PublicRepository<ServiceConcession>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public ServiceConcessionRepository(
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
		public override IQueryable<ServiceConcession> CheckHorizontalVisibility(IQueryable<ServiceConcession> that)
		{
            var result = that;
				/*.Where(x =>
					x.Supplier.Login == SessionData.Login ||
					SessionData.Roles.Contains(AccountRoles.Superadministrator) ||					
					(
						x.State == Common.ConcessionState.Active && 
						x.Supplier.Workers.Select(y => y.Login).Contains(SessionData.Login)
					) ||
					x.Members
						.Where(y => y.SystemCard.SystemCardMembers
							.Where(z => z.Concession.Supplier.Login == SessionData.Login)
							.Any()
						)
						.Any()
				)
			;*/
			return result;
		}
		#endregion CheckHorizontalVisibility
	}
}
