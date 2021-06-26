using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class GreyListRepository : PublicRepository<GreyList>
	{
		#region Contructors
		public GreyListRepository(
			IPublicContext context
		)
			: base(context)
		{
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<GreyList> CheckHorizontalVisibility(IQueryable<GreyList> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
