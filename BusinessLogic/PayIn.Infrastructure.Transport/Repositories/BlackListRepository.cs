using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class BlackListRepository : PublicRepository<BlackList>
	{
		#region Contructors
		public BlackListRepository(
			IPublicContext context
		)
			: base(context)
		{
			
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<BlackList> CheckHorizontalVisibility(IQueryable<BlackList> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
