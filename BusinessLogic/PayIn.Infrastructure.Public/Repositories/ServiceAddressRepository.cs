using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceAddressRepository : PublicRepository<ServiceAddress>
	{
		#region Contructors
		public ServiceAddressRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
