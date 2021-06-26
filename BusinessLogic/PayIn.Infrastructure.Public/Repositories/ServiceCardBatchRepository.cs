using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	class ServiceCardBatchRepository : PublicRepository<ServiceCardBatch>
	{
		#region Constructors
		public ServiceCardBatchRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
