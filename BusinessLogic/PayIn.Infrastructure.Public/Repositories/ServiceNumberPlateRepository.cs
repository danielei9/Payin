using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceNumberPlateRepository : PublicRepository<ServiceNumberPlate>
	{
		#region Contructors
		public ServiceNumberPlateRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
