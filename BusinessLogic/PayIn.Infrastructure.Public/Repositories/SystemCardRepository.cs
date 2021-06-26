using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class SystemCardRepository : PublicRepository<SystemCard>
	{
		#region Constructors
		public SystemCardRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
