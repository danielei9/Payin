using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceCityRepository : PublicRepository<ServiceCity>
	{
		#region Contructors
		public ServiceCityRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
