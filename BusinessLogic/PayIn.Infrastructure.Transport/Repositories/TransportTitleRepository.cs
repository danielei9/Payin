using PayIn.BusinessLogic.Common;
using PayIn.Domain.Security;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class TransportTitleRepository : PublicRepository<TransportTitle>
	{
		public readonly ISessionData SessionData;

		#region Contructors
		public TransportTitleRepository(
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
		public override IQueryable<TransportTitle> CheckHorizontalVisibility(IQueryable<TransportTitle> that)
		{
			if (SessionData.Roles.Contains(AccountRoles.Superadministrator))
				return that;
			else
			{
				//TODO Ver si se permite el acceso a todos los títulos desde un TransportConcession
				return that;
			}
		}
		#endregion CheckHorizontalVisibility
	}
}
