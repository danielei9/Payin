using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceOperationRepository : PublicRepository<ServiceOperation>
	{
		#region Contructors
		public ServiceOperationRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
