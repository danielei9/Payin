using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
				public class ServiceAddressNameRepository : PublicRepository<ServiceAddressName>
				{
								#region Contructors
								public ServiceAddressNameRepository(IPublicContext context)
												: base(context)
								{
								}
								#endregion Contructors
				}
}
