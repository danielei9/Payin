using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceTimeTableRepository : PublicRepository<ServiceTimeTable>
	{
		#region Contructors
		public ServiceTimeTableRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
