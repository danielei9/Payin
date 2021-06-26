using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceGroupEntranceTypeRepository : PublicRepository<ServiceGroupEntranceType>
	{
		#region Constructors
		public ServiceGroupEntranceTypeRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
