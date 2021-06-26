using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceGroupProductRepository : PublicRepository<ServiceGroupProduct>
	{
		#region Constructors
		public ServiceGroupProductRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
