using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
    public class ServiceIncidencyRepository: PublicRepository<ServiceIncidence>
	{
		#region Constructors
		public ServiceIncidencyRepository(IPublicContext context)
			: base(context)
		{
			
		}
		#endregion Constructors
	}
}
