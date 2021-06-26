using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class ServiceCountryRepository : PublicRepository<ServiceCountry>
	{
		#region Contructors
		public ServiceCountryRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
