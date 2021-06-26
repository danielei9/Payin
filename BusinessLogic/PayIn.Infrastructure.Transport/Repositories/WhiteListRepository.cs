using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Public.Db;
using System;
using System.Linq;

namespace PayIn.Infrastructure.Transport.Repositories
{
	public class WhiteListRepository : PublicRepository<WhiteList>
	{

		#region Contructors
		public WhiteListRepository(
			IPublicContext context
		)
			: base(context)
		{
			
		}
		#endregion Contructors

		#region CheckHorizontalVisibility
		public override IQueryable<WhiteList> CheckHorizontalVisibility(IQueryable<WhiteList> that)
		{
			return that;
		}
		#endregion CheckHorizontalVisibility
	}
}
