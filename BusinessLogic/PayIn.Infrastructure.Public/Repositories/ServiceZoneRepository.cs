using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceZoneRepository : PublicRepository<ServiceZone>
	{
		#region Contructors
		public ServiceZoneRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
