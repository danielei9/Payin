using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	class ServiceCardRepository : PublicRepository<ServiceCard>
	{
		#region Constructors
		public ServiceCardRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
