using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServicePriceRepository : PublicRepository<ServicePrice>
	{
		#region Contructors
		public ServicePriceRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
