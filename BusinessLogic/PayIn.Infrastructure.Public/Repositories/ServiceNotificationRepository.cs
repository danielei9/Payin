using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
    public class ServiceNotificationRepository: PublicRepository<ServiceNotification>
	{
		#region Constructors
		public ServiceNotificationRepository(IPublicContext context)
			: base(context)
		{
			
		}
		#endregion Constructors
	}
}
