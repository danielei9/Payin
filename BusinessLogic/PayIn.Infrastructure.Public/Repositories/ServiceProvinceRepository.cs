using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceProvinceRepository : PublicRepository<ServiceProvince>
	{
		#region Contructors
		public ServiceProvinceRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
