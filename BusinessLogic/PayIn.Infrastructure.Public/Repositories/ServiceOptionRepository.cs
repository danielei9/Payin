using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceOptionRepository : PublicRepository<ServiceOption>
	{
		#region Constructors
		public ServiceOptionRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
