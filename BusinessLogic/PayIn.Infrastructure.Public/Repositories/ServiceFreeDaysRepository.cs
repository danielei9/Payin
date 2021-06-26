using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
				public class ServiceFreeDaysRepository : PublicRepository<ServiceFreeDays>
				{
								#region Contructors
								public ServiceFreeDaysRepository(IPublicContext context)
												: base(context)
								{
								}
								#endregion Contructors
				}
}
