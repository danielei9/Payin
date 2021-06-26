using PayIn.Domain.Internal;
using PayIn.Infrastructure.Internal.Db;

namespace PayIn.Infrastructure.Internal.Repositories
{
	public class UserRepository : InternalRepository<User>
	{
		#region Contructors
		public UserRepository(IInternalContext context)
			: base(context)
		{
		}
		#endregion Contructors
	}
}
