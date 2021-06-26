using PayIn.Domain.Public;
using PayIn.Infrastructure.Public.Db;

namespace PayIn.Infrastructure.Public.Repositories
{
	public class SystemCardMemberRepository : PublicRepository<SystemCardMember>
	{
		#region Constructors
		public SystemCardMemberRepository(IPublicContext context)
			: base(context)
		{
		}
		#endregion Constructors
	}
}
