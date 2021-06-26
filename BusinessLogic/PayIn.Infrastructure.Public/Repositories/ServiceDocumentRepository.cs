using PayIn.BusinessLogic.Common;
using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;
using System;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceDocumentRepository : PublicRepository<ServiceDocument>
	{
		public ISessionData SessionData { get; set; }

		#region Constructors
		public ServiceDocumentRepository(
			IPublicContext context,
			ISessionData sessionData
		)
			: base(context)
		{
			SessionData = sessionData ?? throw new ArgumentNullException("sessionData");
		}
		#endregion Constructors
	}
}
