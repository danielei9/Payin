using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceConfigurationDataRepository : PublicRepository<ServiceConfigurationData>
	{
		#region Contructors
					public ServiceConfigurationDataRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
